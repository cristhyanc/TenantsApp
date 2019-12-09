using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
   public interface IScheduleBl
    {
        bool SaveSchedule(SchedulePayment schedule);
        Task<bool> PayRent(Guid paymentID, bool sendEmail, string sender, int weeks, decimal totalPaid);
        List<Rent> GetRents(Guid scheduleID);
        SchedulePayment GetTenantSchedule(Guid tenantID);
        List<Rent> GetUpcomingRents();
        bool DeleteSchedule(Guid scheduleId);
        SchedulePayment GetScheduleByRent(Guid paymentID);
    }
}
