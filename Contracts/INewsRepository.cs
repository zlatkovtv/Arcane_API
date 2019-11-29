using System.Threading.Tasks;
using System.Collections.Generic;

public interface INewsRepository: IExternalApiRepository
{
    Task<dynamic> GetNewsInfo(string source);
    Task<IEnumerable<NewsAction>> AddToFavourites(NewsAction newsAction);
    Task<dynamic> GetNewsSources();
}