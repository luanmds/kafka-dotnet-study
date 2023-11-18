#!/bin/bash
curl -X POST -H "Content-Type: application/vnd.schemaregistry.v1+json" \
     --data "@message-data-schema.json" \
     http://localhost:8081/subjects/loader-topic-value/versions

echo "Schema created in loader-topic-value"

curl -X POST -H "Content-Type: application/vnd.schemaregistry.v1+json" \
     --data "@message-data-schema.json" \
     http://localhost:8081/subjects/worker-topic-value/versions

echo "Schema created in worker-topic-value"