# ASP.NET Core and React SPA ToDoList Application

This is a todo list application built using ASP.NET Core 5.0 for a REST/JSON API server and React for a web client.


## Overview of Tech Stack
- Server
  - ASP.NET Core 5.0 (Microservice architecture)
  - SQLite by Microsoft EntityFramework Core (version 5.0.15)
- Client
  - React 17
  - Fetch API for REST requests
- Testing
  - .NET Core xUnit for unit testing
  - .NET Core xUnit for integration testing


## Setup

1. To run this application in your system, you need to have the following:
   - [.NET Core 5.0](https://www.microsoft.com/net/core)
   - [Node.js >= v16](https://nodejs.org/en/download/)
   - Visual Studio or Visual Studio Code is recommended to view and run the code.
2. Clone this repository.
3. Run `npm install` after opening command line at `..\ToDoList\ToDoList\ClientApp` this location.
4. If you have Visual Studio to view this project:
   - Go inside `..\ToDoList` folder.
   - Open the `ToDoList.sln` file.
   - Run the project.
   - You will be navigated to [https://localhost:5001](https://localhost:5001).
   - UI screens can be viewed on [https://localhost:5001](https://localhost:5001).
   - API (Swagger) can be viewed on [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html).
5. If you are using command line to run this project:
   - Go inside `..\ToDoList\ToDoList` folder.
   - Open command line at this location.
   - Run the command `dotnet run`.
   - Navigate to [https://localhost:5001](https://localhost:5001).
   - UI screens can be viewed on [https://localhost:5001](https://localhost:5001).
   - API (Swagger) can be viewed on [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html).


## Scripts

### `npm install`

When first cloning the repo or adding new dependencies, run this command.  This will:

- Install Node dependencies from package.json
- Install .NET Core dependencies from api/api.csproj and api.test/api.test.csproj (using dotnet restore)
