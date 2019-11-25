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

public sealed class CryptoRepository : ExternalApiRepository, ICryptoRepository
{
    public CryptoRepository(IOptions<AppSettings> appSettings): base(appSettings)
    {

    }

    public override string GetApiUrl()
    {
        return "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=6&CMC_PRO_API_KEY={0}";
    }

    public async Task<dynamic> GetCryptoInfo()
    {
        var url = string.Format(GetApiUrl(), appSettings.CryptoApiKey);
        return await base.GetFromUrlAsync(url);
    }
}