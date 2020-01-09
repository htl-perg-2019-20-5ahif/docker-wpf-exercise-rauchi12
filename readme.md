# End-to-End Container API with UI

## Introduction

In this project, you have to...

* ...create a small Web API with ASP.NET Core and Entity Framework.
* ...put the API in a Docker container.
* ...write a small UI in WPF (*Windows Presentation Foundation*).

## API

In our fictitious sample, you have to write a database-based Web API which allows users to book eCars in a company's car pool. The following operations must be possible in the API:

* List all cars.
* Book a car for a given day (to simplify, we only support single-day bookings without time).
* List all available cars for a given day.

You have to store cars and bookings in a *SQL Server* database. You *have to* run the SQL Server database in a Docker container (see image [*mcr.microsoft.com/mssql/server*](https://hub.docker.com/_/microsoft-mssql-server)).

## API Docker Image

Write a multi-stage Dockerfile for your ASP.NET Core project. You can find a template for the Dockerfile in [Microsoft's documentation](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1#the-dockerfile-1). **Note**: The docs are parly still .NET Core 3.0. Use .NET Core 3.1 instead.

Use a [*Docker Bridge Network*](https://docs.docker.com/network/bridge/) to enable the API to talk to SQL Server running in a container.

Publish the Docker image that you have built with the Dockerfile in the Docker Hub ([`docker push`](https://docs.docker.com/engine/reference/commandline/push/)).

## WPF UI

Write a WPF app for the service. It *must work* with the API image mentioned above.

The WPF app needs to support the following functions:

* List all cars.
* List all cars that are available on a certain day.
* Book a car for a certain day.

## Extra Points

* Send me a link to your API container on the Docker Hub to receive one extra point. *Note* that the Docker Hub page *must* container a basic description about how to use the Docker image.
* Notify me via GitHub issue that you have also created the requested WPF UI. If it works, you will get a second extra point.


