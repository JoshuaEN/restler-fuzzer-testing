import json
import argparse
import os
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument('--log',
                        help='Log file',
                        type=str, required=True)

args = parser.parse_args()

endpoints = {}

log_file_path = Path(args.log)

def read_request_part(line):
    parts = line.split(': ', 1)
    
    return { name: parts[0].lstrip(), value: parts[1] }

with open(log_file_path) as file_in:
    current_request = None
    skip_next = 0
    for position, line in file_in:
        if skip_next > 0:
            if line.startswith('\t') != True:
                raise Exception(f'Tried skipping line {position}, but it does not appear to be valid to skip {line}')
            skip_next = skip_next - 1
            continue

        if current_request is not None:
            if line.startswith('\t') != True:
                raise Exception(f'Expected line {position} to be part of a previous log message, but it does not appear to be {line}')

            line_info = read_request_part(line)
            current_request[line_info['name']] = line_info['value']

        if not line.endswith('[INF] HTTP request information:'):
            continue
        
        if current_request is None:
            continue

