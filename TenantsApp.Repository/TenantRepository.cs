using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        public TenantRepository(DBContext context) : base(context)
        {

        }
    }
}
