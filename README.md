# Probem: Uniqueness Required
This branch has the [Server\restler-fuzzer\results](Server\restler-fuzzer\results) for just a Traditional (POST on root resource to create) REST resource. This run largely failed, with only 2 requests succeeding, one of which succeds by chance.

The core problem is that the RESTler-fuzzer grammar does not provide a unique value for `id` when calling `POST api/v1/traditionals`. Because the server expects and honors the `id`, rather than ignoring it, this leads for 409 errors when attempting to create more than one of the resource. In contrast, the demo_server included in restler-fuzzer appears to ignore the id provided.

Various attempts were made to inform RESTler-fuzzer that the value should be unique, primarily via custom annotations and `restler_custom_payload_uuid4_suffix` key in the dict.json. Editing `grammar.json` is understood to be another possible way of achieving this, but seems hard to maintain over time.

Further, it is noted that the docs mention unique ids in the [Fuzzing Dictionary](https://github.com/microsoft/restler-fuzzer/blob/main/docs/user-guide/FuzzingDictionary.md) guide, however this appears to be in the context of specifying a prefix to a unique ID rather than specifying what parameters should be unique.

It is also worth noting a similar problem is observed when `PUT api/v1/traditionals/{traditionalId}` is the only producer for a resource, wherein it expects the resource ID provided in the URL to match the resource ID provided in the body.

The endpoint logic described above matches that of a real production system.

## Desired Resolution
The desired resolution is to be able to declaratively instruct the compiler what params should be unique, and when the same unique value needs to appear more than once in a request.

## This run can be reproduced by:
1. Get [RESTler-fuzzer](https://github.com/microsoft/restler-fuzzer) and [build it](https://github.com/microsoft/restler-fuzzer#build-instructions)
1. Run `dotnet run -c TRADITIONALS` for the Server project
1. Once the server has started and is accepting requests, run `python Server/restler-fuzzer/run.py --restler_drop_dir {{path-to-restler-fuzzer-bin}}`, which will:
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
    * `dotnet run -c TRADITIONALS` to enable just `api/v1/traditionals` endpoints
    * `dotnet run -c PUT_ONLYS` to enable just `api/v1/putOnlys` endpoints
    * `dotnet run -c POST_ON_RESOURCES` to enable just `api/v1/postOnResources` endpoints

More generally, controllers are enabled/disabled using preprocessor directives:
 * ENDPOINT_TRADITIONALS
 * ENDPOINT_PUT_ONLYS
 * ENDPOINT_POST_ON_RESOURCES