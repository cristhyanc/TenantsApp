using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
   public interface ITenantsBl
    {
        bool SaveTenant(Place place, Tenant tenant);
        IList<Tenant> GetTenantsByPlace(Guid placeId);
    }
}
