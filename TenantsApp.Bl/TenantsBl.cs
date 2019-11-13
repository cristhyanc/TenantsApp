using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Bl
{
    public class TenantsBl : ITenantsBl
    {

        IUnitOfWork _uow;
        public TenantsBl(IUnitOfWork uow)
        {
            _uow = uow;
        }

       public bool DeleteTenant(Guid tenantId)
        {
            try
            {
                var tenant = _uow.TenantRepository.Get(tenantId);
                if (tenant != null)
                {
                    return tenant.Delete(_uow);
                }

                throw new ValidationException("The Tenants ID is required");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SaveTenant(Place place, Tenant tenant)
        {
            try
            {
                return place.AddTenant(tenant, _uow);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<Tenant > GetTenantsByPlace(Guid placeId)
        {
            try
            {
                var tenan= _uow.TenantRepository.GetAll(x => x.PlaceID == placeId);
                tenan = tenan.OrderByDescending(x => x.End).ToList();
                return tenan;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
