using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Bl
{
    public class PlacesBl : IPlacesBl
    {
        IUnitOfWork _uow;
        public PlacesBl(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<Place> GetCurrentPlaces()
        {
            try
            {
                var result = _uow.PlaceRepository.GetAll();
                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.Tenants = _uow.TenantRepository.GetAll(x => x.PlaceID == item.PlaceID);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePlace(Guid placeId)
        {
            try
            {
                var place = _uow.PlaceRepository.Get(placeId);
                if(place!=null)
                {
                    return place.Delete(_uow);
                }

                throw new ValidationException("The place ID is required");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SavePlace(Place place)
        {
            try
            {
                return place.Save(_uow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
