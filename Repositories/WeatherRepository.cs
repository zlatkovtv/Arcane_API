using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System;
using System.Text;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public sealed class WeatherRepository : ExternalApiRepository, IWeatherRepository
{
    public WeatherRepository(IOptions<AppSettings> appSettings): base(appSettings)
    {
        
    }

    public override string GetApiUrl()
    {
        return "https://api.openweathermap.org/data/2.5/forecast?q={0},{1}&appid={2}";
    }

    public async Task<dynamic> GetForecastForCity(string city, string country)
    {
        var url = string.Format(GetApiUrl(), city, country, appSettings.WeatherApiKey);
        return await base.GetFromUrlAsync(url);
    }
}