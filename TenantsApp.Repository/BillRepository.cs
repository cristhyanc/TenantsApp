using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
  public  class BillRepository : Repository<Bill>, IBillRepository
    {

        public BillRepository(DBContext context) : base(context)
        {

        }
    }
}
