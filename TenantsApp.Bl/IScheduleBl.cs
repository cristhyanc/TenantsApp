using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
   public interface IScheduleBl
    {
        bool CreateNewScheduleRent(ScheduleRent schedule);
        bool PayRent(Guid rentID);
        List<Rent> GetRents(Guid scheduleID);
        ScheduleRent GetTenantSchedule(Guid tenantID);
        List<Rent> GetUpcomingRents();
    }
}
