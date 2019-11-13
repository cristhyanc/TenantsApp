using System.Threading.Tasks;



namespace TenantsApp.Shared.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendRentEmail(decimal totalRent, string name, string email, string address);
    }
}
