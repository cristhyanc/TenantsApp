using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;
using TenantsApp.Shared.Interfaces;

namespace TenantsApp.Bl
{
    public class ScheduleBl : IScheduleBl
    {

        IUnitOfWork _uow;
        IEmailService _emailService;
        public ScheduleBl(IUnitOfWork uow, IEmailService emailService )
        {
            _uow = uow;
            _emailService = emailService;
        }


        public bool DeleteSchedule(Guid scheduleId)
        {
            try
            {
                var schedu = _uow.ScheduleRentRepositoy.Get(scheduleId);
                if (schedu == null)
                {
                    throw new ValidationException("Schedule does not exist");
                }

                _uow.Begin();
                if (schedu.Delete(_uow))
                {
                    _uow.Commit();
                    return true;
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

        public ScheduleRent GetTenantSchedule(Guid tenantID)
        {
            return _uow.ScheduleRentRepositoy.Get(x => x.TenantID == tenantID);
        }

        public List<Rent> GetRents(Guid scheduleID)
        {
            var rents = _uow.RentRepository.GetAll(x => x.ScheduleID == scheduleID).OrderBy (x => x.ExpiryDate).ToList();
            return rents;
        }


        public bool SavecheduleRent(ScheduleRent schedule)
        {
            try
            {
                bool  isNew = false; ;
                if (schedule == null)
                {
                    throw new ArgumentNullException(nameof(schedule));
                }

                if (schedule.ScheduleID == Guid.Empty)
                {
                    isNew = true;
                }

                _uow.Begin();
                if (schedule.Save(_uow))
                {
                    if (isNew)
                    {
                        if (schedule.CreateNextRent(_uow))
                        {
                            _uow.Commit();
                            return true;
                        }
                    }
                    else
                    {
                        _uow.Commit();
                        return true;
                    }
                }                

                _uow.RollBack();
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

      

        public async Task<bool > PayRent(Guid rentID)
        {
            try
            {
                var rent = _uow.RentRepository.Get(rentID);
                if (rent == null)
                {
                    throw new ValidationException("Rent could not be found");
                }

                var tenant = _uow.TenantRepository.Get(rent.TenantID);

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

                           await  _emailService.SendRentEmail(rent.Price, tenant.Name, tenant.Email, place.Address);

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
