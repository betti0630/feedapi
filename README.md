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
git clone https://github.com/betti0630/attrecto.git
cd attrecto
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
dotnet run
```

Frontend (WebApp/attrectoweb):

```bash
npm install
npm start
```

## Usage
**1. Register or login**
   
Send a POST request to /api/auth/register or /api/auth/login with JSON body:

```json
{
  "username": "alice",
  "password": "Passw0rd!"
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


