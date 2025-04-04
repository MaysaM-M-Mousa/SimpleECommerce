# SimpleECommerce

## Introduction
**SimpleECommerce** is a side project designed to explore Event-Driven Microservices architecture while incorporating Domain-Driven Design (DDD) principles where applicable.

## Services

### Order Service
The **Order Service** manages customer orders and their lifecycle. It follows best practices for software architecture and distributed systems:
* **Architecture**: Implements Clean Architecture for maintainability and separation of concerns.
* **Domain-Driven Design**:
  * Defines an **Order** aggregate that manages:
    *  Creating new orders
    *  Adding and removing line items
    *  Cancelling and placing orders
*  **Event-Driven**: Publishes domain events upon state changes to ensure consistency across services.
*  **Transactional Outbox Pattern**: Ensures reliable event publishing in distributed environments.
*  **Integration Events**: Uses integration events for external communication with other services.
*  **Messaging**: Utilizes **RabbitMQ** for inter-service communication.
* **Testing**: Implements robust unit testing to ensure service reliability.

### Inventory Service
The **Inventory Service** is responsible for stock management and ensuring product availability. It adheres to the following principles:
* **Architecture**: Implements Clean Architecture for structured and maintainable code.
* **Domain-Driven Design**:
  * Defines a **Product** aggregate responsible for:
    * Reserving stock
    * Releasing stock
    * Deducting stock
* **Event-Driven**: Publishes domain events to notify other services of stock changes.
* **Transactional Outbox Pattern**: Guarantees reliable event publication.
* **Integration Events**: Uses integration events for external communication with other services.
* **Messaging**: Leverages **RabbitMQ** for asynchronous communication.
* **Testing**: Implements robust unit testing to ensure service reliability.
* **Saga Pattern**: Implements saga/process managers for handling long-running workflows:
  * Currently supports the `ReserveStock` saga for coordinated stock reservations.
* **Inbox Pattern**: Leverages MassTransit's built-in support for inbox pattern for all consumer types (Consumers, Sagas, etc ..)

## Deployment & Infrastructure
The project includes a **Docker Compos**e configuration to simplify local development and deployment. The key components of the system include:

### Microservices
The system consists of two containerized services that expose relevant ports for communication:
* **Inventory Service**
  * Swagger UI: https://localhost:5000/swagger/index.html
* **Order Service**
  * Order Swagger UI: https://localhost:5003/swagger/index.html

### Database
* **PostgreSQL**: The primary database for both services, with persistent volume storage to ensure data durability.

### Database Management
* **PgAdmin**: A web-based interface for managing the PostgreSQL database.
  * **Access**: http://localhost:5050
  * **Login Credentials**:
    * **Email**: `admin@pgadmin.com`
    * **Password**: `MyStrong!Passw0rd`

### Message Broker
* **RabbitMQ**: Acts as the message broker for event-driven communication between services.
  * **Management UI**: http://localhost:15672/
  * Login **Credentials**
    * **Username**: `guest`
    * **Password**: `guest`

With this setup, all services are containerized and ready for communication within a Dockerized environment, simplifying development and testing workflows.
