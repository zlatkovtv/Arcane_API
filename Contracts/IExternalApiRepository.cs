using System.Threading.Tasks;

public interface IExternalApiRepository
{
    string GetApiUrl();
    Task<dynamic> GetFromUrlAsync(string url);
}