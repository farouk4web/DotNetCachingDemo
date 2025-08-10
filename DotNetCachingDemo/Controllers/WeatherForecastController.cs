using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace DotNetCachingDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IMemoryCache _cacheService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IMemoryCache cacheService, ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _cacheService = cacheService;
            _logger = logger;
            _cache = cache;
        }


        [HttpGet("Memory_Try")]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast1()
        {
            // way 1
            if (!_cacheService.TryGetValue("WeatherForecast", out IEnumerable<WeatherForecast>? weathers))
            {
                weathers = await GetFromApiOrDbAsync();
                _cacheService.Set("WeatherForecast", weathers, TimeSpan.FromMinutes(3));

                return weathers;
            }

            return weathers ?? [];
        }

        [HttpGet("Memory_GetOrCreate")]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast2()
        {
            // way 2 
            return await _cacheService.GetOrCreateAsync("WeatherForecast", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3);
                return await GetFromApiOrDbAsync();
            }) ?? [];
        }

        [HttpGet("Distributed_Redis")]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast3()
        {
            var cachedData = await _cache.GetStringAsync("WeatherForecast");

            if (!string.IsNullOrEmpty(cachedData))
                return JsonSerializer.Deserialize<List<WeatherForecast>>(cachedData)!;

            var weathers = await GetFromApiOrDbAsync();

            var serialized = JsonSerializer.Serialize(weathers);
            await _cache.SetStringAsync(
                "WeatherForecast",
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                }
            );

            return weathers;
        }

        [HttpGet("Distributed_Sql")]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast4()
        {
            var cachedData = await _cache.GetStringAsync("WeatherForecast");

            if (!string.IsNullOrEmpty(cachedData))
                return JsonSerializer.Deserialize<List<WeatherForecast>>(cachedData)!;

            var weathers = await GetFromApiOrDbAsync();

            var serialized = JsonSerializer.Serialize(weathers);
            await _cache.SetStringAsync(
                "WeatherForecast",
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                }
            );

            return weathers;
        }


        private async Task<IEnumerable<WeatherForecast>> GetFromApiOrDbAsync()
        {
            string[] Summaries =
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            Console.WriteLine("Generating data...");

            // Simulate an API or database call
            await Task.Delay(1000);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();
        }
    }
}
