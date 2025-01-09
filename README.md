# HotelScope Project

HotelScope is a modular microservices-based project for managing hotels, reports, and logging. It comprises the following services:

- **HotelScope.API**: Provides RESTful API endpoints for managing hotels and reports.
- **HotelScope.WEB**: An MVC-based frontend application that communicates with HotelScope.API using HTTP.
- **ReportBackgroundService**: A RabbitMQ consumer service for generating and processing reports asynchronously.
- **MySQL Database**: The central database for storing hotel and report data.
- **RabbitMQ**: Message broker for asynchronous communication.
- **ELK Stack**: Elasticsearch, Logstash, and Kibana for logging and visualization.

## Features

1. **Hotel Management**
   - Add, remove, and get hotels.
   - Add and remove hotel contact information.
   - Add and remove hotel staff.

2. **Report Management**
   - Generate reports asynchronously based on location data.
   - Monitor the status of reports (e.g., `Preparing`, `Completed`).

3. **Logging and Monitoring**
   - Centralized logging using the ELK stack.
   - RabbitMQ Management UI for monitoring messages and queues.

---

## Prerequisites

Ensure you have the following installed:

- **Docker**: [Download Docker](https://www.docker.com/get-started)
- **Docker Compose**: Typically included with Docker Desktop.
- **.NET SDK**: Version 8.0

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/ceyhundagli57/hotelscope.git
cd hotelscope
```

### 2. Set Up Environment Variables

Create a `.env` file in the root of the project with the following:

```env
MYSQL_ROOT_PASSWORD=ceyhun1010
MYSQL_DATABASE=HotelScope
ASPNETCORE_ENVIRONMENT=Development
```

### 3. Build and Start Services

Run the following command to build and start all services:

```bash
docker-compose up --build
```

### 4. Access Services

- **HotelScope.API**: [https://localhost:7006](https://localhost:7006)
- **HotelScope.WEB**: [https://localhost:7161](https://localhost:7161)
- **RabbitMQ Management UI**: [http://localhost:15672](http://localhost:15672) (default credentials: `guest` / `guest`)
- **Kibana (ELK)**: [http://localhost:5601](http://localhost:5601)

---

## Project Structure

```plaintext
HotelScope/
├── HotelScope.API/          # Backend API for managing hotels and reports
├── HotelScope.WEB/          # MVC frontend consuming HotelScope.API
├── ReportBackgroundService/ # RabbitMQ consumer for report generation
├── Application/             # Contains business logic and service layer implementations
├── Domain/                  # Defines core entities
├── Infrastructue/           # Handles data access, external integrations, and configurations
├── docker-compose.yml       # Docker Compose file to orchestrate services
├── .env                     # Environment variables file
└── README.md                # Documentation for the project
```

---

## Key Endpoints

### Hotel Management

- **GET** `/api/hotels` - List all hotels.
- **POST** `/api/hotels` - Add a new hotel.
- **DELETE** `/api/hotels/{id}` - Remove a hotel.

### Report Management

- **POST** `/api/reports` - Request a new report.
- **GET** `/api/reports` - List all reports.
- **GET** `/api/reports/{id}` - Get detailed report information.

---

## Logging and Monitoring

### ELK Stack

- **Elasticsearch**: Centralized storage for logs.
- **Logstash**: Processes and forwards logs.
- **Kibana**: Visualize logs at [http://localhost:5601](http://localhost:5601).

### RabbitMQ

Monitor message queues via the RabbitMQ Management UI at [http://localhost:15672](http://localhost:15672).

---

## Troubleshooting

### Common Issues

1. **Database Connection Error**:
   - Ensure MySQL is running and the credentials match the `.env` file.
   - Check the `ConnectionStrings__DefaultConnection` in `docker-compose.yml`.

2. **RabbitMQ Not Responding**:
   - Ensure the RabbitMQ service is running.
   - Verify port `15672` is not blocked.

3. **ELK Stack Fails to Start**:
   - Ensure Docker has enough memory allocated for Elasticsearch.
   - Check the logs for Elasticsearch, Logstash, or Kibana containers.

---

## Contact

For questions or feedback, please contact [ceyhundagli57@gmail.com].

