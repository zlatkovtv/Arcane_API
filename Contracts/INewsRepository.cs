using System.Threading.Tasks;

public interface INewsRepository: IExternalApiRepository
{
    Task<dynamic> GetNewsInfo(string country);
}