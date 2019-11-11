
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
 public   class ScheduleRentRepositoy : Repository<ScheduleRent>, IScheduleRentRepositoy
    {
        public ScheduleRentRepositoy(DBContext context) : base(context)
        {

        }
    }
}
