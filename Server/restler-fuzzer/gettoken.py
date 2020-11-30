import argparse
import requests
import json

parser = argparse.ArgumentParser()
parser.add_argument('--token_url',
                        help='URL to request tokens from',
                        type=str, required=True)

args = parser.parse_args()

token = str(requests.post(args.token_url).content.decode('utf-8'))
metadata = { 'app1': { }}
print(json.dumps(metadata))
print(f"Authentication: Timestamp {token!s}")