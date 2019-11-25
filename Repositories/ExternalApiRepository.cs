using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public abstract class ExternalApiRepository: IExternalApiRepository
{
    protected readonly AppSettings appSettings;

    protected ExternalApiRepository(IOptions<AppSettings> appSettings) 
    {
        this.appSettings = appSettings.Value;
    }

    public abstract string GetApiUrl();

    public async Task<dynamic> GetFromUrlAsync(string url)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Couldn't get data from weather API.");
        }
        
        string json;
        using (var content = response.Content)
        {
            json = await content.ReadAsStringAsync();
        }

        return JsonConvert.DeserializeObject<dynamic>(json);
    }
}