using System.Collections.Generic;
using Andoromeda.Kyubey.Incubator.Models;

namespace Andoromeda.Kyubey.Incubator.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
