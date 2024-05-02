# FacilaIT

## Knowledge


- For passowrd database need use below API for encrypt

> /api/Setting/encryptPlaintText

## Migration Database

```bash

# 1
dotnet tool install --global dotnet-ef

# 2
dotnet ef migrations add InitialCreate

# 3
dotnet ef database update


```

#### Admin user

{
"username": "admin",
"password": "QAZwsxedc@123"
}

## Templates

### Method body of API skeleton

```C#
string methodName = ControllerContext.ActionDescriptor.ActionName;
_logger.LogInformation($"Start {methodName} ");
RequestResult res = new RequestResult();
try
{

    // res.Response = logAnalyzerLines;
}
catch (System.Exception ex)
{
    res.IsSuccess = false;
    res.Message = ex.Message;
    _logger.LogError(ex.Message);
    _logger.LogError(ex.StackTrace);
}
_logger.LogInformation($"End {methodName} {res.IsSuccess}");

return res;
```

## Command

- Build without warning

  > dotnet build --property WarningLevel=0

- Run application

  > dotnet run --property WarningLevel=0

- code-generator controller

  > dotnet-aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc DBBContext -outDir Controllers

- Remove package using dotnet command

  > dotnet remove package [package_name]

- Publish to Production

  > dotnet publish --configuration Release

## Dockerazing

- Build image

```bash
docker build --tag facilait-img .
```

- Create container using previous image

```bash
docker run -d -p 8080:80 --name facilait-cotr facilait-img
```

- Execute internal docker to unzip bower library

```bash
docker exec -it facilait-cotr /bin/bash
```

- Remove un-used images

```bash
docker rmi $(docker images --filter "dangling=true" -q --no-trunc)
```

## docker compose

- Run command

```bash
docker-compose up
```

## References

[Create a web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code)

[add logger to file output in dotnet core 7 web api](https://github.com/nreco/logging)

[SingalR client](https://learn.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-7.0&tabs=visual-studio)

[JWT Authentication And Authorization](https://www.c-sharpcorner.com/article/jwt-authentication-and-authorization-in-net-6-0-with-identity-framework/)

[Docker & use PostgreSQL](https://mohamadlawand.medium.com/net-7-how-to-containerise-web-api-with-docker-use-postgresql-e3791293c4f0)
