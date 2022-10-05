# Final Project .NET WEB API - (Let's Code by ADA)

Final Project from Let's Code by ADA Bootcamp - Module 6 - Web Programming III | Web API.  
*Due Date: 18/09/2022 - 23:59:59*

You can check the detailed instructions (pt-br) on [final_project_web_III_instructions.md](https://github.com/fabioyamashita/.NET-WEB-API-Project/blob/main/final_project_web_III_instructions.md)

This project is a .NET Web-API application. The data source was the S&P historical data from 2012 to 2022. You can check the json [here](https://github.com/fabioyamashita/.NET-WEB-API-Project/blob/main/src/SPXData-2012-2022.json).

The following concepts were applied:

- API REST
- Unit Tests
- Controllers
- Model Binding
- Filters (Action, Result, Exception, Logger)
- Authentication and Authorization
- JWT (Bearer Token)
- CORS
- Environment variables
- Extension Methods
- Working with files
- SOLID Principles
- Repository Pattern
- Entity Framework Core
- Dapper
- Docker
- FluentValidation
- Serilog
- Microsoft Identity (soon...)

...

## 🖥️ Getting Started

There are two versions of the app that you can run the tests:
- Using [Docker](#docker) (SQL Server)
- Using [In-Memory Database](#in-memory-database)

## Docker

- Clone the repository:  
```
$ git clone https://github.com/fabioyamashita/.NET-WEB-API-Project.git
```

- Navigate to `src` directory:
```
$ cd .NET-WEB-API-Project\src
```

- Run all containers with the following command:
```
$ docker-compose up --build
```

## In-Memory Database

- Clone the repository:  
```
$ git clone https://github.com/fabioyamashita/.NET-WEB-API-Project.git
```

- Navigate to `.NET-WEB-API-Project` directory:
```
$ cd .NET-WEB-API-Project
```

- Checkout to commit `1d269118a03baf7e2f7a8001b6dd9b3999e2a141`:
```
$ git checkout 1d269118a03baf7e2f7a8001b6dd9b3999e2a141
```
- Run the solution *`SPX-WEBAPI.sln`* in `src` folder.

- In *appsettings.json*, configure the `Token` and `AdminAuthentication` Sections with the following code:

```
  "Token": {
    "Secret": "your_custom_secret",
    "Audience": "ada",
    "Issuer": "Fabio Yamashita",
    "ExpirationTimeInHours": 2
  },

  "AdminAuthentication": {
    "login": "usuario",
    "password": "m1nh@s3nh@"
  },
```

- **Run the application.** (`Ctrl + F5` in Visual Studio Commnunity)

## Endpoints Tests

You can test the endpoints using **Swagger** or **Postman**.

### Swagger

- After running the app, access the URL: http://localhost:5000/swagger/index.html

### Postman

- Import the following link to Postman: https://www.getpostman.com/collections/42fae1f2ef76aff91af3

## Instructions

- Only the route http://localhost:5000/Login **(POST /Login in Swagger)** can be accessed without any Authentication.

- To access all other endpoints, you need to generate a token using the **LOGIN POST method**, with the following request body:
```
{
  "login": "usuario",
  "password": "m1nh@s3nh@"
}
```

- Copy the token.
- In Swagger, click on `Authorize` button at top right and paste the token.
- In Postman, paste the token on `Auth` tab. See the example below:

![image](https://user-images.githubusercontent.com/98363297/190874281-7a52c95c-e4b3-423f-bd68-e66f645fd494.png)

