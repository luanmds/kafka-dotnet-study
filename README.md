# Study about Apache Kafka and Dotnet

This repository contains any sample projects to integration between .Net and Apache Kafka.

## Before running a Sample Project

**In your Machine**:

1. Install Docker and Docker Compose 
2. Install .NET Core >= 7.0
3. Starts Kafka running the **docker-compose.yml** located in the **/docker-kafka** directory with commmand:

```bash
$ docker-compose up -d
```
4. This command creates the necessaries topics to samples


References:
- [Kafka by Bitnami](https://github.com/bitnami/containers/tree/main/bitnami/kafka)
- [Zookeeper by Bitnami](https://github.com/bitnami/containers/tree/main/bitnami/zookeeper)
- [Kafka CLI](https://medium.com/@TimvanBaarsen/apache-kafka-cli-commands-cheat-sheet-a6f06eac01b#8c2f)
- [Kafdrop - Kafka Client](https://github.com/obsidiandynamics/kafdrop)
- [Configs to Kafka in various machines types](https://www.confluent.io/blog/kafka-client-cannot-connect-to-broker-on-aws-on-docker-etc/)
- [Schema Registry API](https://docs.confluent.io/platform/current/schema-registry/develop/api.html)
- [JsonSchema to Schema Registry](https://json-schema.org/)


## Sample 01 - Basic Consumer and Producer Services

First project is a .Net Console Producer that sends any data by Kafka for consumer project.
Second project is a .Net Worker Consumer that receives any data by Kafka.

### How Execute

First, follow instructions in **Before running a Sample Project** section.

In **sample-01** directory, run both projects using the following dotnet commands:

```bash
dotnet build
```
In separate terminals run each command to run project: 

- To run Worker as consumer listener
```bash
dotnet run --project ./WorkerConsumer/ 
```

- to run Producer and generate one message in topic. Each run produces a message in topic.
```bash
dotnet run --project ./ConsoleProducer/
```
Both projects will be run and send a default message instantly. You can see the generated message in Console terminal output and Kafka UI in **localhost:19000**.

## Sample 02 - Complete Example with CQRS, Kafka and MediatR framework

There are three .Net projects to simulate a simple ScoreCalculator application using 
the concepts of CQRS, integration Kafka/.Net and Schema Registry.

Kafka is responsible to integrate the apps in ScoreCalculator across messages.

### How Execute

First, follow instructions in **Before running a Sample Project** section.

1. Execute the docker-compose.yaml file in project root directory to run project;
2. Open your favorite browser in [http://localhost:8000/swagger];
3. Follow container logs in docker to see flow working;