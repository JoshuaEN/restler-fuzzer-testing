""" THIS IS AN AUTOMATICALLY GENERATED FILE!"""
from __future__ import print_function
import json
from engine import primitives
from engine.core import requests
from engine.errors import ResponseParsingException
from engine import dependencies
req_collection = requests.RequestCollection([])
# Endpoint: /api/v1/Payloads, method: Post
request = requests.Request([
    primitives.restler_static_string("POST "),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("api"),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("v1"),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("Payloads"),
    primitives.restler_static_string(" HTTP/1.1\r\n"),
    primitives.restler_static_string("Accept: application/json\r\n"),
    primitives.restler_static_string("Host: \r\n"),
    primitives.restler_static_string("Content-Type: application/json\r\n"),
    primitives.restler_refreshable_authentication_token("authentication_token_tag"),
    primitives.restler_static_string("\r\n"),
    primitives.restler_static_string("{"),
    primitives.restler_static_string("""
    "testType":
        """),
    primitives.restler_fuzzable_group("fuzzable_group_tag", ['0','1','2','3']  ,quoted=False),
    primitives.restler_static_string("""
    ,
    "testString":"""),
    primitives.restler_fuzzable_string("fuzzstring", quoted=True),
    primitives.restler_static_string(""",
    "testValueA":"""),
    primitives.restler_fuzzable_string("fuzzstring", quoted=True),
    primitives.restler_static_string(""",
    "testValueB":"""),
    primitives.restler_fuzzable_string("fuzzstring", quoted=True),
    primitives.restler_static_string(""",
    "testValueC":"""),
    primitives.restler_fuzzable_string("fuzzstring", quoted=True),
    primitives.restler_static_string(""",
    "letter":
        """),
    primitives.restler_fuzzable_group("fuzzable_group_tag", ['0','1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25']  ,quoted=False),
    primitives.restler_static_string("""
    ,
    "season":
        """),
    primitives.restler_fuzzable_group("fuzzable_group_tag", ['0','1','2','3']  ,quoted=False),
    primitives.restler_static_string("""
    ,
    "weather":
        """),
    primitives.restler_fuzzable_group("fuzzable_group_tag", ['0','1','2','3','4']  ,quoted=False),
    primitives.restler_static_string("""
    }"""),
    primitives.restler_static_string("\r\n"),

],
requestId="/api/v1/Payloads"
)
req_collection.add_request(request)

# Endpoint: /api/v1/PayloadWithExamples, method: Post
request = requests.Request([
    primitives.restler_static_string("POST "),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("api"),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("v1"),
    primitives.restler_static_string("/"),
    primitives.restler_static_string("PayloadWithExamples"),
    primitives.restler_static_string(" HTTP/1.1\r\n"),
    primitives.restler_static_string("Accept: application/json\r\n"),
    primitives.restler_static_string("Host: \r\n"),
    primitives.restler_static_string("Content-Type: application/json\r\n"),
    primitives.restler_refreshable_authentication_token("authentication_token_tag"),
    primitives.restler_static_string("\r\n"),
    primitives.restler_static_string("{"),
    primitives.restler_static_string("""
    "testType":
        1
    }"""),
    primitives.restler_static_string("\r\n"),

],
requestId="/api/v1/PayloadWithExamples"
)
req_collection.add_request(request)
