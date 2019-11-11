using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Bl
{
    public class ScheduleBl : IScheduleBl
    {

        IUnitOfWork _uow;
        public ScheduleBl(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public ScheduleRent GetTenantSchedule(Guid tenantID)
        {
            return _uow.ScheduleRentRepositoy.Get(x => x.TenantID == tenantID);
        }

        public List<Rent> GetRents(Guid scheduleID)
        {
            var rents = _uow.RentRepository.GetAll(x => x.ScheduleID == scheduleID).OrderBy (x => x.ExpiryDate).ToList();
            return rents;
        }


        public bool CreateNewScheduleRent(ScheduleRent schedule)
        {
            try
            {
                if (schedule == null)
                {
                    throw new ArgumentNullException(nameof(schedule));
                }

                _uow.Begin();
                if (schedule.Save(_uow))
                {
                    if (schedule.CreateNextRent(_uow))
                    {
                        _uow.Commit();
                        return true;
                    }
                    else
                    {
                        _uow.RollBack();

                    }
                }
                else
                {
                    _uow.RollBack();
                }
                return false;
            }
            catch (Exception ex)
            {
                _uow.RollBack();
                throw;
            }
        }

        public List<Rent> GetUpcomingRents()
        {
            var rents = _uow.RentRepository.GetAll(x => !x.Paid);

            //var rest2 = _uow.RentRepository.GetAll();
            //_uow.RentRepository.Delete(rest2);

            //var sche = _uow.ScheduleRentRepositoy.GetAll();
            //_uow.ScheduleRentRepositoy.Delete(sche);

            if (rents.Count > 0)
            {
                var ids = rents.Select(x => x.TenantID).ToList();
                var tenants = _uow.TenantRepository.GetAll(x => ids.Contains(x.TenantID)).ToList();
                tenants.ForEach(x => x.Place = _uow.PlaceRepository.Get(x.PlaceID));

                foreach (var item in rents)
                {
                    item.Tenant = tenants.Where(y => y.TenantID == item.TenantID).FirstOrDefault();
                }

            }

            return rents.OrderBy(x=> x.ExpiryDate).ToList();
        }

      

        public bool PayRent(Guid rentID)
        {
            try
            {
                var rent = _uow.RentRepository.Get(rentID);
                if (rent == null)
                {
                    throw new ValidationException("Rent could not be found");
                }

                var schedule = _uow.ScheduleRentRepositoy.Get(rent.ScheduleID);

                if (schedule == null)
                {
                    throw new ValidationException("Schedule could not be found");
                }

                var place = (from tn in _uow.TenantRepository.GetAll()
                             join pl in _uow.PlaceRepository.GetAll()
                                on tn.PlaceID equals pl.PlaceID
                             where tn.TenantID == rent.TenantID
                             select pl).FirstOrDefault();

                if (place == null)
                {
                    throw new ValidationException("Property could not be found");
                }

                place.TotalSaved += rent.Price;

                _uow.Begin();
                if (rent.PayRent(_uow))
                {
                    if (schedule.CreateNextRent(_uow))
                    {
                        if (place.Save(_uow))
                        {
                            _uow.Commit();
                            return true;
                        }
                    }
                }

                _uow.RollBack();
                return false;

            }
            catch (Exception)
            {
                _uow.RollBack();
                throw;
            }
        }
    }
}
