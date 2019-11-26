using System.Threading.Tasks;
using System.Collections.Generic;

public interface INewsRepository: IExternalApiRepository
{
    Task<dynamic> GetNewsInfo(string country);
    Task<IEnumerable<NewsAction>> AddToFavourites(NewsAction newsAction);
}