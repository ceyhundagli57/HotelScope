# HotelScope

HotelScope is a microservices-based hotel directory system designed for managing hotel records, contact information, and generating statistical reports. This project provides a scalable and asynchronous architecture to efficiently handle hotel information and reporting.

## Features

- **Hotel Management**
  - Create and remove hotel records.
  - Add and remove hotel contact information.
  - List hotel officials and contact details.

- **Report Management**
  - Generate statistical reports based on hotel location.
  - List previously generated reports with their status.
  - Retrieve detailed information about specific reports.

- **Asynchronous Reporting**
  - Report requests are handled asynchronously using a message queue system to avoid bottlenecks.

## Technologies Used

- **Backend Framework**: .NET Core
- **Database**: MySQL
- **Message Queue**: RabbitMQ
- **Logging**: ELK Stack

## Installation

### Prerequisites

- Docker and Docker Compose installed.
- Git installed.

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/ceyhundagli57/HotelScope.git
   cd HotelScope
   ```

2. Set up the environment:
   - Create a `.env` file in the root directory with the necessary environment variables.

3. Build and run the project:
   ```bash
   docker-compose up --build
   ```

4. The services will be available at:
   - Web Project `http://localhost:8080`
   - RabbitMQ Management: `http://localhost:15672`
   - Kibana Dashboard: `http://localhost:5601`

## Usage

#### Using the MVC Service
The MVC service provides a user-friendly web interface to interact with the HotelScope system. Below is a step-by-step guide on how to use it:

1. Access the MVC service at: `http://localhost:8080`
2. **Home Page**:
   - The home page displays a summary of available features and options.
3. **Hotel Management**:
   - Navigate to the "Hotels" section to:
     - View a list of all registered hotels.
     - Add a new hotel.
     - Details of hotel, view and add contact information and hotel staff.
     - Edit existing hotel details or remove hotels.
4. **Reports**:
   - Navigate to the "Reports" section to:
     - Request a new report based on location.
     - View the status of existing reports (e.g., Preparing, Completed).
     - View completed reports.

### Hotel Management Endpoints

- **Create a Hotel**: `POST /api/hotels`
- **Remove a Hotel**: `DELETE /api/hotels/{hotelId}`
- **Add Contact Information**: `POST /api/hotels/{hotelId}/contacts`
- **Remove Contact Information**: `DELETE /api/hotels/{hotelId}/contacts/{contactId}`
- **List Hotel Officials**: `GET /api/hotels/{hotelId}/officials`
- **Get Hotel Details**: `GET /api/hotels/{hotelId}`

### Report Management Endpoints

- **Request a Report**: `POST /api/reports`
- **List Reports**: `GET /api/reports`
- **Get Report Details**: `GET /api/reports/{reportId}`


## Report Asynchronous Handling

- Reports are queued using RabbitMQ.
- Worker services process queued reports and update the status to `Completed` upon finishing.

## Logging

- Logs are collected and visualized using the ELK stack.


## Contact

For questions or support, contact:
- **Author**: Ceyhun Dagli
- **Email**: [ceyhundagli57@example.com](mailto:ceyhundagli57@example.com)

