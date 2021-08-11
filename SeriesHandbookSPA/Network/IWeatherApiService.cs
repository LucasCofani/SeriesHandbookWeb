using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeriesHandbookShared.Models;

namespace SeriesHandbookSPA.Network
{
    public interface IWeatherApiService
    {
        [Get("/api/WeatherForecast")]
        Task<List<WeatherForecast>> GetWeather();
    }
}
