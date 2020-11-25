# Simple demo project to test various RESTler-fuzzer features

## Running Server + RESTler-fuzzer
1. Get [RESTler-fuzzer](https://github.com/microsoft/restler-fuzzer) and [build it](https://github.com/microsoft/restler-fuzzer#build-instructions)
1. Run `dotnet run` the Server project or use the UI of your choice
1. Once the server has started and is accepting requests, run `python Server/restler-fuzzer/run.py --restler_drop_dir {{path-to-restler-fuzzer-bin}}`, which will:
    1. Download the swagger.json from the local server
    1. Insert absolute paths into the [config.template.json](Server\restler-fuzzer\inputs\config.template.json) (note: logic is currently hard-coded of what paths to insert and where)
    1. Run restler compile with the [config.json](Server\restler-fuzzer\results\config.json) created in the last step
    1. Run restler test with the result of the compilation

## Controllers
There are three controllers designed to test three cases:

### api/v1/traditionals
Represents a normal or traditional REST API, with:
```
   GET api/v1/traditionals
  POST api/v1/traditionals
   GET api/v1/traditionals/{traditionalId}
   PUT api/v1/traditionals/{traditionalId}
DELETE api/v1/traditionals/{traditionalId}
```

### api/v1/putOnlys
Represents a REST API that does not have a POST on the resource root to create. Rather, PUT on a specific resource is used to both create and update.
Note that this API example requires the PUT resource ID in the URL must match the resource ID provided in the body of the request for both create and update.
Endpoints are:
```
   GET api/v1/putOnlys
  POST api/v1/putOnlys
   GET api/v1/putOnlys/{putOnlyId}
   PUT api/v1/putOnlys/{putOnlyId}
DELETE api/v1/putOnlys/{putOnlyId}
```

### api/v1/postOnResources
Represents a REST API that does not have a POST on the resource root to create. Rather, POST on a specific resource is used to create (PUT is still available to create or update as well).
Note that this API example requires the POST resource ID in the URL must match the resource ID provided in the body of the request for create.
Endpoints are:
```
   GET api/v1/postOnResources
  POST api/v1/postOnResources
   GET api/v1/postOnResources/{postOnResourceId}
   PUT api/v1/postOnResources/{postOnResourceId}
DELETE api/v1/postOnResources/{postOnResourceId}
```

## Subsets
Several configurations are available to enable just specific controllers:
    * `dotnet run -c TRADITIONALS` to enable just `api/v1/Traditionals` endpoints
    * `dotnet run -c PUT_ONLYS` to enable just `api/v1/PutOnlys` endpoints
    * `dotnet run -c POST_ON_RESOURCES` to enable just `api/v1/PostOnResources` endpoints
    * `dotnet run -c AS_EXPECTED` to enable just `api/v1/AsExpects` endpoints
    * `dotnet run -c RECURSIVE_MODELS` to enable just `api/v1/RecursiveModels` endpoints
    * `dotnet run -c NESTED_MODELS` to enable just `api/v1/NestedModels` endpoints

More generally, controllers are enabled/disabled using preprocessor directives:
 * ENDPOINT_TRADITIONALS
 * ENDPOINT_PUT_ONLYS
 * ENDPOINT_POST_ON_RESOURCES
 * ENDPOINT_AS_EXPECTED
 * ENDPOINT_RECURSIVE_MODELS
 * ENDPOINT_NESTED_MODELS