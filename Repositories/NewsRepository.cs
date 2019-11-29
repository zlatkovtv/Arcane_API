using System.Data;
using Dapper;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

public sealed class NewsRepository : ExternalApiRepository, INewsRepository
{
    const string SOURCES_PATH = "https://newsapi.org/v2/sources?apiKey={0}";
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
        return "https://newsapi.org/v2/top-headlines?sources={0}&apiKey={1}";
    }

    public async Task<dynamic> GetNewsInfo(string source)
    {
        var url = string.Format(GetApiUrl(), source, appSettings.NewsApiKey);
        return await base.GetFromUrlAsync(url);
    }

    public async Task<dynamic> GetNewsSources() 
    {
        var url = string.Format(SOURCES_PATH, appSettings.NewsApiKey);
        return await base.GetFromUrlAsync(url);
    }
}