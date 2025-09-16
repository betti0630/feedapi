# Attrecto Test App

Sample .NET API with React frontend and MariaDB.

## Run with Docker

```bash
git clone https://github.com/betti0630/attrecto.git
cd attrecto
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


