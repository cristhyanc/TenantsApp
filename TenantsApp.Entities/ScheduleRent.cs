using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Entities
{
   public class ScheduleRent
    {
        [PrimaryKey]
        public Guid ScheduleID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
      
        public Guid TenantID { get; set; }
        public int Period { get; set; }

        [Ignore]
        public Tenant Tenant { get; set; }

        [Ignore ]
        public List<Rent > Rents { get; set; }

        public bool Save(IUnitOfWork uow)
        {
            if(uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if(this.EndDate<this.StartDate  )
            {
                throw new ValidationException("The dates are invalid");
            }

            if(TenantID==Guid.Empty && (this.Tenant ==null || this.Tenant.TenantID==Guid.Empty  ) )
            {
                throw new ValidationException("The Tenant is invalid");
            }

            if(Period<1)
            {
                throw new ValidationException("The week period is invalid");
            }

            if (TenantID == Guid.Empty)
            {
                TenantID = this.Tenant.TenantID;
            }

            if (this.ScheduleID == Guid.Empty)
            {
                this.ScheduleID = Guid.NewGuid();
                return uow.ScheduleRentRepositoy.Insert(this);
            }

            return uow.ScheduleRentRepositoy.Update(this);
        }

        public bool Delete(IUnitOfWork uow)
        {
            var rents = uow.RentRepository.GetAll(x => x.ScheduleID == this.ScheduleID);
            if(rents?.Count>0)
            {
                uow.RentRepository.Delete(rents);
            }
            return uow.ScheduleRentRepositoy.Delete(this);
        }

        public bool CreateNextRent(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }
            var totalWeeks = this.Period;
            var lastRent = uow.RentRepository.GetAll(x => x.ScheduleID == this.ScheduleID && x.Paid).OrderByDescending(x => x.PaidDate).FirstOrDefault();
            DateTime startDate = DateTime.Parse(this.StartDate.ToShortDateString());

            if (lastRent != null)
            {
                startDate = lastRent.ExpiryDate;
                totalWeeks = lastRent.TotalPaidWeeks;
            }

            var tenant = uow.TenantRepository.Get(this.TenantID);

            if (tenant == null)
            {
                throw new ValidationException("Tenant does not exist");
            }

            var rent = new Rent();
            rent.Price = tenant.Rent*this.Period ;
            rent.Paid = false;
            rent.ScheduleID = this.ScheduleID;
            rent.TenantID = this.TenantID;
            rent.ExpiryDate = startDate.AddDays(7 * totalWeeks);

            if (DateTime.Compare(rent.ExpiryDate, this.EndDate) > 0)
            {
                rent.ExpiryDate = this.EndDate;
                var days = rent.ExpiryDate - startDate;
                rent.Price = Math.Round((tenant.Rent / 7) * days.Days, 0);

            }
            return rent.Save(uow);
        }

    }
}
