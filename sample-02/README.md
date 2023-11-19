# Sample 02 - CQRS and Kafka with Score Calculator Project

This sample project aims to demonstrate: *the concepts of Command Query Segregation of Responsibility, or CQRS, integration Kafka/.Net* through a microservice project called a Score Calculator.

This project uses a **JSON schema** from *Schema-Registry* to keep a message format compatible between apps.

The sample is simple and consists of Commands to calculate score to many clients based your debit values or cancel the calculate (using Compensating Transactions by Saga Pattern).

In Api, the Controller has endpoints that initiate a calculate, get it progress and cancel it progress. Those actions always make sending a Command as message to Kafka topic. 

The Loader project focuses on delegates actions to Worker like: 
- Generate customer data and send Command message for each customer to Worker calculates the score.
- Receive Command to cancel a calculation process and uses a Compensating Transaction to finish it.

The Worker project focuses on action to calculate score: 
- Receive Command to calculate score for customer and persists it in the database.
- Receive Command as Compensating Transaction to cancel a calculation process and remove all scores calculated in database.

## Basic diagrams (Make in Mermaid.js)
### Flow messages diagram

```mermaid
    flowchart LR
        B(SchemaRegistry)
        C(Kafka Topic) 
        A[Api] -->|De/Serialize Message| B
        A[Api] -->|Send Message <br/>| C 
        D[Loader] -->|Send/Receive Message| C
        D[Loader] -->|De/Serialize Message| B 
        E[Worker] -->|Receive Message| C
        E[Worker] -->|Deserialize Message| B 

```

### Sequence Diagrams
**Send Command to Start Calculate Score flow**

```mermaid
    sequenceDiagram
        actor User
        participant Api as Api
        participant K as Kafka 
        participant L as Loader <br/> (with MediatR)
        participant W as Worker <br/> (with MediatR)
        participant DB as Database <br/> (MongoDB)
        
        User->>Api: Init flow (/api/init)
        Api->>K: Send command as message with a process_id 
        activate K
        Api-->>User: Returns a process_id to access process status
        K->>L: Receives message
        deactivate K
        
        activate L
        L->>L: Select handler according <br/> command message 
        L->>L: Generate random data about list of customers
        loop For each client in list of customers
            L->>K: Send message as command with client data
        end
        deactivate L

        K->>W: Select handler according <br/> command message 
        W->>DW: Verify if process is cancelled
        DB-->>W: Return process status

        alt Status != Cancelled
            W->>W: Handler calculate score and persist client data in memory
        else
            W--xW: Cancel operation
        end
         
```

**Send Command to Cancel a calculate score progression by a id**
```mermaid
    sequenceDiagram
        actor User
        participant Api as Api
        participant K as Kafka 
        participant L as Loader <br/> (with MediatR)
        participant W as Worker <br/> (with MediatR)
        participant DB as Database <br/> (MongoDB)
        
        User->>Api: Requests Cancelling a process with process_id (/api/cancel)
        Api->>Api: Validate the request
        Api->>K: Send command with process_id 
        activate K
        Api-->>User: Returns accepted cancellation
        K->>L: Receives message
        deactivate K
        
        activate L
        L->>L: Select handler according <br/> command message 
        L->>DB: Update status to Cancel 
        deactivate L
```

---

## How to Run

### Before Run the project

- Run docker-compose.yml in **/docker-kafka** directory to start kafka platform and create topics necessary to this project. 
- Run the shell script (.sh) in **/docker-kafka/schemas** directory to create necessary schema to running this project!

### To Run this project

1. Execute the docker-compose.yaml file in project root directory to run project;
2. Open your favorite browser in [http://localhost:8000/swagger];
3. Follow container logs in docker to see flow working;

