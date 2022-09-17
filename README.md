# Final Project .NET WEB API - (Let's Code by ADA)

## 🖥️ Getting Started

There are two versions of the app that you can run the tests:
- Using a preloaded In-Memory Database
- Using Docker (SQL Server)

## In-Memory Database

- Clone the repository:  
```
$ git clone https://github.com/fabioyamashita/.NET-WEB-API-Project.git
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

- **Run the application.**

## Endpoints Tests

You can test the endpoints using **Swagger** or **Postman**.

### Swagger

After running the app, access the URL: http://localhost:5000/swagger/index.html

### Postman

Import the following link to Postman: https://www.getpostman.com/collections/42fae1f2ef76aff91af3

### Instructions

