using System.Threading.Tasks;

public interface IWeatherRepository: IExternalApiRepository
{
    Task<dynamic> GetForecastForCity(string city, string country);
}