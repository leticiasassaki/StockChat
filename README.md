# StockChat
This is a chat to get and process quotes, using SignalR and RabbitMQ

### Prerequisites
- .NET 6.0
```
https://dotnet.microsoft.com/download/dotnet/6.0
```

### Running the project
```bash

# Command to restore the dependencies and tools of a project
dotnet restore

# Command to run source code without any explicit compile or launch commands.
dotnet run

# Command to execute unit tests.
dotnet test

```
### Docker
To get things working run the following command `docker-compose up`

# Built With

* [.NET 6.0](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) - The framework used
* [EntityFrameworkCore](https://docs.microsoft.com/en-us/ef/core/) - ORM
* [SignalR](https://dotnet.microsoft.com/en-us/apps/aspnet/signalr) - Library for ASP.NET of real-time web functionality to applications
* [RabbitMQ](https://www.rabbitmq.com/) - Message-broker
* [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/) - IDE used to develop the project