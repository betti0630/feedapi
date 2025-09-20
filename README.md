# Attrecto Test App

Sample .NET API with React frontend and MariaDB.

## Prerequisites

Before you begin, ensure you have:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 
- Node.js + npm 
- Docker & Docker Compose  
- (Optional) MariaDB or other relational database if running without Docker 

## Clone the Repo

```bash
git clone https://github.com/betti0630/feedapi.git
cd feedapi
```
## Run with Docker

```bash
docker compose up --build
```

- API: http://localhost:5001
- Frontend: http://localhost:3001

## Run locally

Start MariaDB with:
- host: localhost
- port: 3306
- database: attrectotest
- user: myuser
- password: mypassword

Backend (AttrectoTest.ApiService):

```bash
dotnet build
dotnet run
```

Frontend (WebApp/attrectoweb):

```bash
npm install
npm start
```

- API: http://localhost:5481
- Frontend: http://localhost:3000


## Usage
**1. Register or login**
   
Send a POST request to /api/auth/register or /api/auth/login with JSON body:

User:
```json
{
  "username": "alice",
  "password": "Passw0rd!"
}
```

Admin:
User:
```json
{
  "username": "admin",
  "password": "AdminPassw0rd!"
}
```

**2. Get JWT token**

The response will include a JWT token.

**3. Use token in requests**

Include the token in the Authorization header:

```makefile
Authorization: Bearer <your-token>
```
**4. Create a feed**

POST to /api/feeds with a JSON body (or form-data if uploading image).

## CI Pipeline

The project runs **integration tests** in GitHub Actions:

- Docker Compose starts MariaDB and the API (IntegrationTest mode).
- The pipeline waits for the API `/health` endpoint.
- xUnit integration tests run against the live API.

The build fails if the services don’t start or any test fails.

![CI](https://github.com/betti0630/feedapi/actions/workflows/ci.yml/badge.svg)

