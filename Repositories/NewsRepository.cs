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

public sealed class NewsRepository : ExternalApiRepository, INewsRepository
{
    public NewsRepository(IOptions<AppSettings> appSettings): base(appSettings)
    {

    }

    public override string GetApiUrl()
    {
        return "https://newsapi.org/v2/top-headlines?country={0}&apiKey={1}";
    }

    public async Task<dynamic> GetNewsInfo(string country)
    {
        var url = string.Format(GetApiUrl(), country, appSettings.NewsApiKey);
        return await base.GetFromUrlAsync(url);
    }
}