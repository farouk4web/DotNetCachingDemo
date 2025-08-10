# CachingDemo

Demo ASP.NET Core app demonstrating different caching techniques:

- **In-Memory Cache** using `IMemoryCache` for fast, local caching.
- **Distributed Redis Cache** for scalable, cross-server caching.
- **Distributed SQL Server Cache** for persistent caching in a database.

## Features

- Example endpoints for each caching method.
- Shows how to store and retrieve cached data.
- Simple and practical for learning caching in .NET.

## Getting Started

1. Clone the repo  
2. Update Redis connection string in `Program.cs` if needed.  
3. Update SQL Server connection string and run cache table creation script:  

```bash
dotnet sql-cache create "<your-connection-string>" dbo CacheItems


Run the project and test the endpoints:

/WeatherForecast/Memory_Try
/WeatherForecast/Memory_GetOrCreate
/WeatherForecast/Distributed_Redis
/WeatherForecast/Distributed_Sql


Requirements
.NET 7 SDK or later

Redis server (for Redis caching)

SQL Server instance (for SQL Server caching)
