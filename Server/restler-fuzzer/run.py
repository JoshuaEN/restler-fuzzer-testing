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
    swagger = requests.get(url)
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

    output_config_path.write_text(json.dumps(config,sort_keys=True, indent=4))

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

def test_spec(ip, port, host, use_ssl, results_path, compile_path, restler_dll_path):
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
        f"dotnet {restler_dll_path} test --grammar_file {compile_path.joinpath('grammar.py')} --dictionary_file {compile_path.joinpath('dict.json')}"
        f" --settings {compile_path.joinpath('engine_settings.json')}"
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

    args = parser.parse_args()

    # Generate paths
    restler_dll_path = Path(os.path.abspath(args.restler_drop_dir)).joinpath('restler', 'Restler.dll')
    base_path = Path(os.path.abspath(os.path.dirname(__file__)))
    inputs_path = base_path.joinpath('inputs')
    results_path = base_path.joinpath('results')
    api_spec_path = results_path.joinpath('swagger.json')
    compile_path = results_path.joinpath('Compile')
    output_config_path = results_path.joinpath('config.json')

    # Get swagger.json
    download_spec('http://localhost:5000/swagger/v1/swagger.json', api_spec_path)

    # Get a config with the abs paths filled in
    transform_template_config(output_config_path, inputs_path, results_path, compile_path)

    # Compile
    compile_spec(output_config_path, results_path, restler_dll_path.absolute())

    # Test
    test_spec(args.ip, args.port, args.host, args.use_ssl, results_path, compile_path, restler_dll_path.absolute())

    print(f"Test complete.\nSee {results_path} for results.")