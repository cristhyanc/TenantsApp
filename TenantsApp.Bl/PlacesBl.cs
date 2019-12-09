using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;
using System.Linq;

namespace TenantsApp.Bl
{
    public class PlacesBl : IPlacesBl
    {
        IUnitOfWork _uow;
        public PlacesBl(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<Bill> GetBills(Guid placeId)
        {
            return _uow.BillRepository.GetAll(x => x.PlaceID == placeId).OrderByDescending(x=>!x.Paid).OrderByDescending(x=> x.ExpiryDate ).ToList() ;
        }


        public IList<Rent> GetRents(Guid placeID)
        {
            var rents = (from tn in _uow.TenantRepository.GetAll() join rent in _uow.RentRepository.GetAll() 
                       on tn.TenantID equals rent.TenantID where tn.PlaceID == placeID select rent ).ToList();

            rents.ForEach(x => x.Tenant = _uow.TenantRepository.Get(x.TenantID));
            rents = rents.OrderByDescending  (x => x.ExpiryDate).ToList();
            return rents;
        }

        public Place GetPlace(Guid placeId)
        {
            var result = _uow.PlaceRepository.Get(placeId);
            if(result!=null)
            {
                result.LoadTenants(_uow);
                foreach (var item in result.Tenants )
                {
                    item.ScheduleRent = _uow.ScheduleRentRepositoy.GetAll(x => x.ParentID == item.TenantID).FirstOrDefault();
                    if(item.ScheduleRent!=null)
                    {
                        item.ScheduleRent.Payments = _uow.RentRepository.GetAll(x => x.ScheduleID == item.ScheduleRent.ScheduleID);
                    }
                }
            }

            return result;
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
                        item.LoadTenants(_uow);
                        
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool DeletePlace(Guid placeId)
        {
            try
            {       
                if(placeId==null)
                {
                    throw new ValidationException("The place ID is required");
                }

                var place = _uow.PlaceRepository.Get(placeId);
                if(place!=null)
                {
                    _uow.Begin();
                    if( place.Delete(_uow))
                    {
                        _uow.Commit();
                        return true;
                    }
                    else
                    {
                        _uow.RollBack();
                    }
                }
                return false;
                
            }
            catch (Exception ex)
            {
                _uow.RollBack();
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
