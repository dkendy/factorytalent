# Factory Talent API Playground

## Overview
This project is built using .NET 8 and C#. It includes a Docker setup for containerization and uses Keycloak as the Identity Provider. Additionally, Jaeger is integrated for distributed tracing.

## Prerequisites
- .NET 8 SDK
- Docker
- Docker Compose

## Getting Started

### Clone the repository
```bash
git clone https://github.com/dkendy/factorytalent.git
cd factorytalent
```

### Build and Run the Project

#### Using .NET CLI
1. Restore the dependencies:
    ```bash
    dotnet restore FactoryTalent.sln
    ```
2. Build the project:
    ```bash
    dotnet build FactoryTalent.sln
    ```

#### Using Docker Compose
1. Build and start the Docker containers:
    ```bash
    docker-compose up --build
    ```
2. Stop the Docker containers:
    ```bash
    docker-compose down
    ```

### Running Tests
To run the unit tests, use the following command:
```bash
dotnet test FactoryTalent.sln
```

## Project Structure
- **/src**: Contains the source code of the application.
- **/tests**: Contains the unit tests for the application.
- **docker-compose.yml**: Docker Compose file to build and run the Docker containers.
- **Dockerfile**: Dockerfile to build the Docker image.
- **README.md**: Project documentation.

## Docker Compose Services
The `docker-compose.yml` file includes the following services:
- **factorytalent.api**: The main API service.
- **factorytalent.database**: PostgreSQL database service.
- **factorytalent.identity**: Keycloak service for identity management.
- **factorytalent.seq**: Seq service for structured log management.
- **factorytalent.redis**: Redis service for caching.
- **factorytalent.jaeger**: Jaeger service for distributed tracing.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License.