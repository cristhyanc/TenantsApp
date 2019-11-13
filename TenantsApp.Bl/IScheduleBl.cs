using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
   public interface IScheduleBl
    {
        bool SavecheduleRent(ScheduleRent schedule);
        Task<bool> PayRent(Guid rentID);
        List<Rent> GetRents(Guid scheduleID);
        ScheduleRent GetTenantSchedule(Guid tenantID);
        List<Rent> GetUpcomingRents();
        bool DeleteSchedule(Guid scheduleId);
    }
}
