namespace IpScanner.Infrastructure.Repositories.Abstract
{
    public interface IFeatureRepository
    {
        long GetStatusValueOrDefault(string status);
        long GetOuiValueOrDefault(string oui);
        long GetManufacturerValueOrDefault(string manufacturer);
    }
}
