using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Bl
{
    public class TenantsBl : ITenantsBl
    {

        IUnitOfWork _uow;
        public TenantsBl(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool SaveTenant(Place place, Tenant tenant)
        {
            try
            {
                return place.AddTenant(tenant, _uow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<Tenant > GetTenantsByPlace(Guid placeId)
        {
            try
            {
                return _uow.TenantRepository.GetAll(x => x.PlaceID == placeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
