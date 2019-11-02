using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

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
                return result;
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
