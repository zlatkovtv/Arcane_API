using System.Data;
using Dapper;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

public sealed class NewsRepository : ExternalApiRepository, INewsRepository
{
    private readonly IDatabaseRepository dbContext;

    public NewsRepository(IOptions<AppSettings> appSettings, IDatabaseRepository dbContext): base(appSettings)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<NewsAction>> AddToFavourites(NewsAction newsAction)
    {
         using (IDbConnection connection = this.dbContext.GetConnection()) {
            connection.Open();
            return await connection.QueryAsync<NewsAction>("INSERT INTO NEWS_FAVOURITES WHERE USERID = @UserId", new { UserId = newsAction.UserId });
        }
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