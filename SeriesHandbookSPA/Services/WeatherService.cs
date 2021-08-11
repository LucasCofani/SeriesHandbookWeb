using SeriesHandbookShared.Models;
using SeriesHandbookSPA.Network;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesHandbookSPA.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiService _api;
        public WeatherService(IWeatherApiService api)
        {
            _api = api;
        }
        public async Task<List<WeatherForecast>> GetWeather()
        {
            return await _api.GetWeather();
        }
    }
    public interface IWeatherService
    {
        Task<List<WeatherForecast>> GetWeather();
    }
}
