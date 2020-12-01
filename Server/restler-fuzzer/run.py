# Modified from https://github.com/microsoft/restler-fuzzer/blob/main/restler-quick-start.py
# Original copyright:
# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
import argparse
import contextlib
import os
import subprocess
import requests
import json
import time
import shutil
from jsonpointer import resolve_pointer, set_pointer, JsonPointer
from pathlib import Path

RESTLER_TEMP_DIR = 'restler_working_dir'

@contextlib.contextmanager
def usedir(dir):
    """ Helper for 'with' statements that changes the current directory to
    @dir and then changes the directory back to its original once the 'with' ends.
    Can be thought of like pushd with an auto popd after the 'with' scope ends
    """
    curr = os.getcwd()
    os.chdir(dir)
    try:
        yield
    finally:
        os.chdir(curr)

def download_spec(url, api_spec_path):
    print(f'> Downloading OpenAPI Specification from {url!s}')
    swagger = requests.get(url)
    print(f'> Saving OpenAPI Specification to {api_spec_path!s}')
    Path(api_spec_path).write_bytes(swagger.content)

def transform_template_config(output_config_path, inputs_path, results_path, compile_path):
    config = json.loads(inputs_path.joinpath('config.template.json').read_bytes())

    if "SwaggerSpecFilePath" in config:
        config["SwaggerSpecFilePath"] = [str(results_path.joinpath('swagger.json'))]
    if "GrammarOutputDirectoryPath" in config:
        config["GrammarOutputDirectoryPath"] = str(compile_path)
    if "CustomDictionaryFilePath" in config:
        config["CustomDictionaryFilePath"] = str(inputs_path.joinpath('dict.json'))
    if "AnnotationFilePath" in config:
        config["AnnotationFilePath"] = str(inputs_path.joinpath('annotations.json'))
    if "EngineSettingsFilePath" in config:
        config["EngineSettingsFilePath"] = str(results_path.joinpath('engine_settings.json'))
    if "GrammarInputFilePath" in config:
        config["GrammarInputFilePath"] = str(inputs_path.joinpath('grammar.json'))

    print(f'> Writing config to {output_config_path!s}')
    output_config_path.write_text(json.dumps(config,sort_keys=True, indent=4))

def transform_post_compile_config(output_config_path, compile_path, output_grammar_path):
    config = json.loads(compile_path.joinpath('config.json').read_bytes())
    
    config["GrammarInputFilePath"] = str(output_grammar_path)

    print(f'> Writing config to {output_config_path!s}')
    output_config_path.write_text(json.dumps(config,sort_keys=True, indent=4))

def transform_grammar_json(output_grammar_path, grammar_transforms_path, compile_path):
    grammar_transforms = json.loads(grammar_transforms_path.read_bytes())['transforms']
    grammar_path = compile_path.joinpath('grammar.json')
    grammar = json.loads(grammar_path.read_bytes())
    resolve_pointer_default = {}

    for grammar_transform in grammar_transforms:
        endpoint_grammar = next((e for e in grammar['Requests'] if e['id']['endpoint'] == grammar_transform['endpoint'] and e['id']['method'].lower() == grammar_transform['method'].lower()), None)

        grammar_transform_str = f"""{grammar_transform['method'].upper()} {grammar_transform['endpoint']}"""

        if endpoint_grammar is None:
            raise Exception(f'Transform {grammar_transform_str} not found in {str(grammar_path)}')

        transforms = grammar_transform['transforms']

        for transform in transforms:
            pointer = transform['pointer']

            val = resolve_pointer(endpoint_grammar, pointer, resolve_pointer_default)

            command = transform['action'][0]
            args = transform['action'][1:]

            if command == 'replace':
                if val is resolve_pointer_default:
                    raise Exception(f'Transform {grammar_transform_str} {pointer} path not found in {str(grammar_path)}')

                set_pointer(endpoint_grammar, pointer, args[0])

            elif command == 'remove':
                if val is resolve_pointer_default:
                    raise Exception(f'Transform {grammar_transform_str} {pointer} path not found in {str(grammar_path)}')

                if isinstance(val, list):
                    if not isinstance(args[0], int):
                        raise Exception(f'Transform {grammar_transform_str} {pointer} expects to be given an int for removing the index of an Array')
                    if len(val) > args[0]:
                        raise Exception(f'Transform {grammar_transform_str} {pointer} the index to remove is {args[0]}, but the array\'s max index is {len(val)-1}')

                    del val[args[0]]
                elif isinstance(val, dict):
                    if args[0] not in val:
                        raise Exception(f'Transform {grammar_transform_str} {pointer} the key {args[0]} did not appear in the object')
                    
                    del val[args[0]]
                else:
                    raise Exception(f'Transform {grammar_transform_str} {pointer} cannot remove from {val!s}')

            else:
                raise Exception(f'Transform {grammar_transform_str} {pointer} unknown command: {command}')

    output_grammar_path.write_text(json.dumps(grammar,sort_keys=False, indent=2))


def transform_template_engine_settings(fuzz, output_engine_settings_path, inputs_path, results_path):
    settings = json.loads(inputs_path.joinpath('engine_settings.template.json').read_bytes())

    if "per_resource_settings" in settings:
        for path in settings["per_resource_settings"]:
            resource_settings = settings["per_resource_settings"][path]

            if "custom_dictionary" in resource_settings:
                relative_custom_dictionary_path = resource_settings["custom_dictionary"]
                current_path = inputs_path.joinpath(relative_custom_dictionary_path).resolve()
                dest_path = results_path.joinpath(relative_custom_dictionary_path).resolve()

                shutil.copyfile(current_path, dest_path)

                resource_settings["custom_dictionary"] = str(dest_path)

    if "token_refresh_cmd" in settings:
        token_cmd_path = str(inputs_path.joinpath("..", "gettoken.py").resolve())
        settings["token_refresh_cmd"] = f"python {token_cmd_path} --token_url http://localhost:5000/api/v1/authenticate"

    # if "fuzzing_mode" in settings:
    #     if settings["fuzzing_mode"] == "directed-smoke-test" and fuzz:
    #         settings.pop("fuzzing_mode")
    #     if not fuzz:
    #         settings["fuzzing_mode"] = "directed-smoke-test"

    print(f'> Writing engine settings to {output_engine_settings_path!s}')
    output_engine_settings_path.write_text(json.dumps(settings,sort_keys=True, indent=4))

def compile_spec(config_path, results_path, restler_dll_path):
    """ Compiles a specified api spec
    @param restler_dll_path: The absolute path to the RESTler driver's dll
    @type  restler_dll_path: Str
    @return: None
    @rtype : None
    """
    if not os.path.exists(results_path):
        os.makedirs(results_path)

    with usedir(results_path):
        command = f"dotnet {restler_dll_path} compile {config_path}"
        print(f'> {command}')
        subprocess.run(command, shell=True, check=True)

def test_spec(fuzz, ip, port, host, use_ssl, fuzz_args, inputs_path, results_path, compile_path, restler_dll_path):
    """ Runs RESTler's test mode on a specified Compile directory
    @param ip: The IP of the service to test
    @type  ip: Str
    @param port: The port of the service to test
    @type  port: Str
    @param host: The hostname of the service to test
    @type  host: Str
    @param use_ssl: If False, set the --no_ssl parameter when executing RESTler
    @type  use_ssl: Boolean
    @param restler_dll_path: The absolute path to the RESTler driver's dll
    @type  restler_dll_path: Str
    @return: None
    @rtype : None
    """

    command = (
        f"dotnet {restler_dll_path} {'fuzz' if fuzz else 'test'} --grammar_file {compile_path.joinpath('grammar.py')} --dictionary_file {compile_path.joinpath('dict.json')}"
        f" --settings {compile_path.joinpath('engine_settings.json')} {fuzz_args if fuzz_args is not None else ''}"
    )
    if not use_ssl:
        command = f"{command} --no_ssl"
    if ip is not None:
        command = f"{command} --target_ip {ip}"
    if port is not None:
        command = f"{command} --target_port {port}"
    if host is not None:
        command = f"{command} --host {host}"

    with usedir(results_path):
        print(f'> {command}')
        subprocess.run(command, shell=True, check=True)

if __name__ == '__main__':

    parser = argparse.ArgumentParser()
    parser.add_argument('--ip',
                        help='The IP of the service to test',
                        type=str, required=False, default='localhost')
    parser.add_argument('--port',
                        help='The port of the service to test',
                        type=str, required=False, default='5000')
    parser.add_argument('--restler_drop_dir',
                        help="The path to the RESTler drop",
                        type=str, required=True)
    parser.add_argument('--use_ssl',
                        help='Set this flag if you want to use SSL validation for the socket',
                        action='store_true')
    parser.add_argument('--host',
                        help='The hostname of the service to test',
                        type=str, required=False, default='localhost:5000')
    parser.add_argument('--skip_download',
                        help='Skip downloading the OpenAPI Specification from the server',
                        action='store_true')
    parser.add_argument('--skip_transform_config',
                        help='Skip transforming the config.template.json',
                        action='store_true')
    parser.add_argument('--skip_compile',
                        help='Skip compiling the grammar',
                        action='store_true')
    parser.add_argument('--fuzz',
                        help='Enables fuzzing (vs. testing)',
                        action='store_true')
    parser.add_argument('--skip_fuzzing',
                        help='Skip running the fuzzer',
                        action='store_true')
    parser.add_argument('--fuzz_only',
                        help='Only run the fuzzer; skipping all other steps',
                        action='store_true')
    parser.add_argument('--grammar_transforms',
                        help='JSON file with transforms to perform on the grammar',
                        type=str, required=False, default=None)
    parser.add_argument('--fuzz_args',
                        help='Args to pass to the fuzzer',
                        type=str, required=False, default=None)

    args = parser.parse_args()

    if args.fuzz_only is True:
        if args.skip_fuzzing is True:
            raise Exception('Both --fuzz_only and --skip_fuzzing cannot be set to True')
        args.skip_download = args.skip_transform_config = args.skip_compile = True

    # Generate paths
    restler_dll_path = Path(os.path.abspath(args.restler_drop_dir)).joinpath('restler', 'Restler.dll')
    base_path = Path(os.path.abspath(os.path.dirname(__file__)))
    inputs_path = base_path.joinpath('inputs')
    results_path = base_path.joinpath('results') #, time.strftime("%Y-%m-%d-%H-%M-%S"))
    api_spec_path = results_path.joinpath('swagger.json')
    compile_path = results_path.joinpath('Compile')
    output_config_path = results_path.joinpath('config.json')
    output_config_with_grammar_path = results_path.joinpath('config-with-grammar.json')
    output_engine_settings_path = results_path.joinpath('engine_settings.json')

    if not os.path.exists(results_path):
        os.makedirs(results_path)

    # Get swagger.json
    if args.skip_download is not True:
        download_spec('http://localhost:5000/swagger/v1/swagger.json', api_spec_path)

    # Get a config with the abs paths filled in
    if args.skip_transform_config is not True:
        transform_template_config(output_config_path, inputs_path, results_path, compile_path)
        transform_template_engine_settings(args.fuzz, output_engine_settings_path, inputs_path, results_path)

    # Compile
    if args.skip_compile is not True:
        compile_spec(output_config_path, results_path, restler_dll_path.absolute())

        # We have to compile again after we transform the grammar
        if args.grammar_transforms is not None:
            first_pass_compile_path = results_path.joinpath('Compile-First-Pass')
            output_grammar_path = results_path.joinpath('transformed-grammar.json')

            # Remove anything left from previous run
            if os.path.exists(first_pass_compile_path):
                shutil.rmtree(str(first_pass_compile_path))

            # Move the first compile result someplace else
            os.rename(compile_path, first_pass_compile_path)

            # Actually transform the grammar
            transform_grammar_json(output_grammar_path, Path(args.grammar_transforms), first_pass_compile_path)

            # Make a new config.json pointing to the grammar
            transform_post_compile_config(output_config_with_grammar_path, first_pass_compile_path, output_grammar_path)

            # Compile again
            compile_spec(output_config_with_grammar_path, results_path, restler_dll_path.absolute())

            # Copy some files from the first pass that are not automatically added during the second pass
            shutil.copyfile(first_pass_compile_path.joinpath('dict.json'), compile_path.joinpath('dict.json'))
            shutil.copyfile(first_pass_compile_path.joinpath('engine_settings.json'), compile_path.joinpath('engine_settings.json'))
            shutil.copyfile(output_grammar_path, compile_path.joinpath('grammar.json'))

    # Test
    if args.skip_fuzzing is not True:
        test_spec(args.fuzz, args.ip, args.port, args.host, args.use_ssl, args.fuzz_args, inputs_path, results_path, compile_path, restler_dll_path.absolute())

    print(f"Run complete.\nSee {results_path} for results.")