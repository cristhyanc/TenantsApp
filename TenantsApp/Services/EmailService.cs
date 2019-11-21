using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TenantsApp.Entities;
using TenantsApp.Shared.Exceptions;
using TenantsApp.Shared.Interfaces;
using Xamarin.Essentials;
namespace TenantsApp.Services
{
    public class EmailService: IEmailService
    {
        public async Task<bool> SendRentEmail( string email, string address, string body)
        {
            try
            {
                var message = new EmailMessage();
                //if(rent==null || tenant == null || place==null)
                //{
                //    throw new ValidationException("Rent, Tenant or place not found");
                //}

                if (string.IsNullOrEmpty(email))
                {
                    throw new ValidationException("The Tenant has not got an Email");
                }

                var fn = "Rent Receipt.html";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllText(file,body);

                message.To = new List<string>();
                message.To.Add(email);
                message.Subject = "Rent Paid " + address;
                message.Body = "Please check the attachment, Thanks";
                message.BodyFormat = EmailBodyFormat.Html;
                message.Attachments.Add(new EmailAttachment(file));

                await Email.ComposeAsync(message);

                return true;
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                throw new ValidationException("Email is not supported on this device");

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
