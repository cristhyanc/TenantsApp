
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
 public   class SchedulePaymentRepositoy : Repository<SchedulePayment>, ISchedulePaymentRepositoy
    {
        public SchedulePaymentRepositoy(DBContext context) : base(context)
        {

        }
    }
}
