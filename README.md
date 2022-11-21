# .NET WEB API Project

Final Project from Let's Code by ADA Bootcamp - Module 6 - Web Programming III | Web API.

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

<br />

## 🖥️ Getting Started

There are two versions of the app that you can run the tests:

- Using [Docker](#docker) (SQL Server)
- Using [In-Memory Database](#in-memory-database)

<br />

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

<br />

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

- Run the solution _`SPX-WEBAPI.sln`_ in `src` folder.

- In _appsettings.json_, configure the `Token` and `AdminAuthentication` Sections with the following code:

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

<br />

## Endpoints Tests

You can test the endpoints using **Swagger** or **Postman**.

### Swagger

- After running the app, access the URL: http://localhost:5000/swagger/index.html

### Postman

- Import the following link to Postman: https://www.getpostman.com/collections/42fae1f2ef76aff91af3

<br />

## Base URL

Users must prepend all resource calls with this base URL:

```
http://localhost:5000
```

<br />

## Authentication

- Only the route http://localhost:5000/Login **(POST /login)** can be accessed without any Authentication.

- To access all other endpoints, you need to generate a token using the **POST /login**, with the following request body:

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

<br />

## Endpoints

### Login

| **Method**    | **Authentication?** | **Description**  |
| ------------- | ------------------- | ---------------- |
| `POST /login` | No                  | Generate a token |

### Users

| **Method**                             | **Authentication?** | **Description**                  |
| -------------------------------------- | ------------------- | -------------------------------- |
| `GET /users?page={page}&limit={limit}` | Yes                 | Search all users with pagination |
| `POST /users`                          | Yes                 | Create a new user                |

### Spx

| **Method**                                         | **Authentication?** | **Description**                                             |
| -------------------------------------------------- | ------------------- | ----------------------------------------------------------- |
| [`GET /spx/{id}`](#get-spxid---search-a-sp-record) | Yes                 | Search a S&P record                                         |
| `GET /spx?page={page}&limit={limit}`               | Yes                 | Search all S&P records with pagination                      |
| `POST /spx`                                        | Yes                 | Create a new S&P record                                     |
| `POST /spx/search?page={page}&limit={limit}`       | Yes                 | Search all S&P records from a date interval with pagination |
| `PUT /spx/{id}`                                    | Yes                 | Update an existing record or Create a new record            |
| `DELETE /spx/{id}`                                 | Yes                 | Delete an existing record                                   |

<br />

## GET /spx/{id} - Search a S&P record

This endpoint returns information about a specific S&P record from a day.

### Request

Request body is not necessary.

### Authentication

You need to append a token on header's request.

Use [key, value] as ["Authorization", "Your Token JWT"].

See more details at...

### Responses

<table>
<tr>
<td><strong>Status</strong></td> <td><strong>Description</strong></td><td><strong>Example</strong></td>
</tr>
<tr>
<td> 200 &ndash; OK </td>
<td> A single record is returned </td>
<td>

```json
{
  "data": {
    "id": 1,
    "date": "2022-09-14T00:00:00",
    "close": 2600.0,
    "open": 3900.0,
    "high": 4250.0,
    "low": 3850.52
  }
}
```

</td>
</tr>

<tr>
<td> 404 &ndash; Not Found </td>
<td> Invalid URL or ID not found </td>
<td>

```json
{
  "message": "Error message"
}
```

</td>
</tr>

</td>
</tr>

<tr>
<td> 500 &ndash; Internal Server Error </td>
<td> General error message </td>
<td>

```json
{
  "message": "An unexpected error has occurred!"
}
```

</td>
</tr>

</td>
</tr>
</table>
