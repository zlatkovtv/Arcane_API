using System.Threading.Tasks;

public interface ICryptoRepository: IExternalApiRepository
{
    Task<dynamic> GetCryptoInfo();
}