# CachingDemo

Demo ASP.NET Core app demonstrating different caching techniques:

- **In-Memory Cache** using `IMemoryCache` for fast, local caching.  
- **Distributed Redis Cache** for scalable, cross-server caching.  
- **Distributed SQL Server Cache** for persistent caching in a database.

## Requirements

- .NET 7 SDK or later  
- Redis server (for Redis caching)  
- SQL Server instance (for SQL Server caching)

## Features

- Example endpoints for each caching method.  
- Shows how to store and retrieve cached data.  
- Simple and practical for learning caching in .NET.

## Getting Started

1. Clone the repo  
2. Update Redis connection string in `Program.cs` if needed.  
3. Update SQL Server connection string and create the cache table by running the following SQL:

```sql
CREATE TABLE [dbo].[CacheItems] (
    [Id] NVARCHAR(449) NOT NULL PRIMARY KEY,
    [Value] VARBINARY(MAX) NOT NULL,
    [ExpiresAtTime] DATETIMEOFFSET NOT NULL,
    [SlidingExpirationInSeconds] BIGINT NULL,
    [AbsoluteExpiration] DATETIMEOFFSET NULL
);
```

4. Run the project and test the endpoints:

```bash
/WeatherForecast/Memory_Try
/WeatherForecast/Memory_GetOrCreate
/WeatherForecast/Distributed_Redis
/WeatherForecast/Distributed_Sql
```

---

If you have any questions or want to contribute, feel free to open issues or pull requests.
