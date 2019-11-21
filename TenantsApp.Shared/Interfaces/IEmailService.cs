using System.Threading.Tasks;



namespace TenantsApp.Shared.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendRentEmail( string email, string address, string body);
    }
}
