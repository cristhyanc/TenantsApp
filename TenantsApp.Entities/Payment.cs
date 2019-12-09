using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TenantsApp.Entities
{
   public interface IPayment
    {
       
         Guid PaymentID { get; set; }
         Guid ScheduleID { get; set; }
         decimal Price { get; set; }
         DateTime ExpiryDate { get; set; }
         bool Paid { get; set; }
         DateTime? PaidDate { get; set; }        
         Color ItemBackgroundColor { get; }
         DateTime RentToDate { get; set; }

    }
}
