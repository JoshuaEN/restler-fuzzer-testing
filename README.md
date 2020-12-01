# Example - PayloadBodyChecker default settings
This example/test is around the PayloadBodyChecker's default settings. Here the [Payload type](Server\Models\Payload.cs) has around 33,000 unique valid combinations (assuming a non-null value is provided for every parameter and the parameter order irrelevant). However, in the run, the /api/v1/Payloads endpoint was only called 58 times, and none of the enum values (`testType`, `letter`, `season`, `weather`) were ever included in the body of a request with a value aside from `0`. This was checked by searching the [server log](Server\logs\2020-12-01T19-21-29Z-server.log).


## This run can be reproduced by:
1. Get [RESTler-fuzzer](https://github.com/microsoft/restler-fuzzer) and [build it](https://github.com/microsoft/restler-fuzzer#build-instructions)
1. Run `dotnet run` the Server project or use the UI of your choice
1. Once the server has started and is accepting requests, run `python Server/restler-fuzzer/run.py --restler_drop_dir {{path-to-restler-fuzzer-bin}} --fuzz --fuzz_args "--enable_checkers 
PayloadBody"`, which will:
    1. Download the swagger.json from the local server
    1. Insert absolute paths into the [config.template.json](Server\restler-fuzzer\inputs\config.template.json) (note: logic is currently hard-coded of what paths to insert and where)
    1. Run restler compile with the [config.json](Server\restler-fuzzer\results\config.json) created in the last step
    1. Run restler test with the result of the compilation


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

### api/v1/Traditionals
Represents a normal or traditional REST API, with:
```
   GET api/v1/Traditionals
  POST api/v1/Traditionals
   GET api/v1/Traditionals/{traditionalId}
   PUT api/v1/Traditionals/{traditionalId}
DELETE api/v1/Traditionals/{traditionalId}
```

### api/v1/PutOnlys
Represents a REST API that does not have a POST on the resource root to create. Rather, PUT on a specific resource is used to both create and update.
Note that this API example requires the PUT resource ID in the URL must match the resource ID provided in the body of the request for both create and update.
Endpoints are:
```
   GET api/v1/PutOnlys
  POST api/v1/PutOnlys
   GET api/v1/PutOnlys/{putOnlyId}
   PUT api/v1/PutOnlys/{putOnlyId}
DELETE api/v1/PutOnlys/{putOnlyId}
```

### api/v1/PostOnResources
Represents a REST API that does not have a POST on the resource root to create. Rather, POST on a specific resource is used to create (PUT is still available to create or update as well).
Note that this API example requires the POST resource ID in the URL must match the resource ID provided in the body of the request for create.
Endpoints are:
```
   GET api/v1/PostOnResources
  POST api/v1/PostOnResources
   GET api/v1/PostOnResources/{postOnResourceId}
   PUT api/v1/PostOnResources/{postOnResourceId}
DELETE api/v1/PostOnResources/{postOnResourceId}
```

### api/v1/AsExpects
Represents a traditional REST API, but follows the rules that RESTler expects. RESTler is able to automatically test all endpoints.
Endpoints are:
```
   GET api/v1/AsExpects
  POST api/v1/AsExpects
   GET api/v1/AsExpects/{asExpectId}
   PUT api/v1/AsExpects/{asExpectId}
DELETE api/v1/AsExpects/{asExpectId}
```

### api/v1/AsExpects/{asExpectId}/NestedResources
Represents a traditional REST API that follows most of the rules that RESTler expects (it wants the `asExpectId` passed to the body for creation requests, which RESTler does not currently do automatically).
Endpoints are:
```
   GET api/v1/AsExpects/{asExpectId}/NestedResources
  POST api/v1/AsExpects/{asExpectId}/NestedResources
   GET api/v1/AsExpects/{asExpectId}/NestedResources/{nestedResourceId}
   PUT api/v1/AsExpects/{asExpectId}/NestedResources/{nestedResourceId}
DELETE api/v1/AsExpects/{asExpectId}/NestedResources/{nestedResourceId}
```

### api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents
Represents a traditional REST API that follows most of the rules that RESTler expects, and has a dependency on its nested peer (NestedResources).
This API wants the `asExpectId` AND `nestedResourceId` passed to the body for creation requests, which RESTler does not currently do automatically.
Endpoints are:
```
   GET api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents
  POST api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents
   GET api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents/{nestedResourceAdjacentId}
   PUT api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents/{nestedResourceAdjacentId}
DELETE api/v1/AsExpects/{asExpectId}/NestedResourceAdjacents/{nestedResourceAdjacentId}
```

### api/v1/AsExpects/{asExpectId}/AuthRequiredResources
Represents a traditional REST API that follows most of the rules that RESTler expects, and required a valid authentication token (from POST /api/v1/authenticate, provided via the `Authentication` header with the schema `Timestamp`).

Endpoints are:
```
   GET api/v1/AuthRequiredResources
  POST api/v1/AuthRequiredResources
   GET api/v1/AuthRequiredResources/{authRequiredResourceId}
   PUT api/v1/AuthRequiredResources/{authRequiredResourceId}
DELETE api/v1/AuthRequiredResources/{authRequiredResourceId}
```


## Subsets
Several configurations are available to enable just specific controllers:
    * `dotnet run -c TRADITIONALS` to enable just `api/v1/Traditionals` endpoints
    * `dotnet run -c PUT_ONLYS` to enable just `api/v1/PutOnlys` endpoints
    * `dotnet run -c POST_ON_RESOURCES` to enable just `api/v1/PostOnResources` endpoints
    * `dotnet run -c AS_EXPECTED` to enable just `api/v1/AsExpects` endpoints
    * `dotnet run -c RECURSIVE_MODELS` to enable just `api/v1/RecursiveModels` endpoints
    * `dotnet run -c NESTED_MODELS` to enable just `api/v1/NestedModels` endpoints
    * `dotnet run -c AUTHN` to enable just `api/v1/AuthRequiredResources` endpoints
    * `dotnet run -c NESTED` to enable the following endpoints:
      + `api/v1/AsExpects`
      + `api/v1/AsExpects/{asExpectId}/NestedResources`
      + `api/v1/AsExpects/{asExpectId}/NestedResourceAdjacent`

More generally, controllers are enabled/disabled using preprocessor directives:
 * ENDPOINT_TRADITIONALS
 * ENDPOINT_PUT_ONLYS
 * ENDPOINT_POST_ON_RESOURCES
 * ENDPOINT_AS_EXPECTED
 * ENDPOINT_RECURSIVE_MODELS
 * ENDPOINT_NESTED_MODELS
 * ENDPOINT_NESTED
 * ENDPOINT_AUTHN