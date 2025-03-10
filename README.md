
![Factory](./themes/factory.jpeg)

# Factory Talent API Playground

## Overview
This project is built using .NET 8 and C#. It includes a Docker setup for containerization and uses Keycloak as the Identity Provider. Additionally, Jaeger is integrated for distributed tracing. For proof of concept only – not ready for production


## Implementation flow: 

The Docker Compose setup initiates all services. Once Keycloak is ready, it can notify the user, and the .NET Core API registers the user adm@factory.com with the password P@ssw0rd1234. The frontend becomes available for login. Only authenticated users with the appropriate roles are authorized to register new users at the same level or lower. All security settings, including the secret key, are predefined and cannot be altered, as doing so could cause certain services to stop functioning.

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
To run the unit and integrations tests, use the following command:
```bash
dotnet test FactoryTalent.sln
```

## Project Structure
- **/src**: Contains the source code of the application.
- **/factory-react**: Contains the ReactJS frontend application.
- **./files**: Contains the Keycloak realm export file used to import and configure a realm in Keycloak.
- **./postman**: Contains Postman Collection file that stores a set of API requests for interact with APIs without manually entering endpoints, headers, or request bodies..
- **docker-compose.yml**: Docker Compose file to build and run the Docker containers.
- **README.md**: Project documentation.

## Docker Compose Services
The `docker-compose.yml` file includes the following services:
- **factorytalent.api**: The main API service.
- **factorytalent.database**: PostgreSQL database service.
- **factorytalent.identity**: Keycloak service for identity management.
- **factorytalent.seq**: Seq service for structured log management.
- **factorytalent.redis**: Redis service for caching.
- **factorytalent.jaeger**: Jaeger service for distributed tracing.
- **factorytalent.frontend**: Login and a simple screen in ReactJS.

## Service Endpoints

This table provides an overview of the available services, their descriptions, URLs, and credentials (if applicable).

| Service  | Description                     | URL                         | Credentials |
|----------|---------------------------------|-----------------------------|-------------|
| factorytalent.identity     | KeyCloak Authentication Service         | `http://localhost:18080/` | User: `admin` <br> Password: `admin` |
| factorytalent.api    | User Profile API Services        | `http://localhost:5080/swagger/index.html` | - |
| factorytalent.database   | Postgres Database       | `localhost` 5432 | DB: `factorytalent` <br> User: `postgres` <br> Password: `postgres` |
| factorytalent.jaeger  | Distributed Tracing        | `http://localhost:16686/` | -  |
| factorytalent.frontend  | ReactJS Login        | `http://localhost:18080/` | User: `adm@factory.com` <br> Password: `P@ssw0rd1234` |

 

## Access the API on Postman

Import the Postman Collection file from the `./postman` directory.


## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License.