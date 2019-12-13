using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Entities
{
   public class SchedulePayment
    {
      
        [PrimaryKey]
        public Guid ScheduleID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }            
        public Guid ParentID { get; set; }
        public int Period { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public SchedulePeriodType SchedulePeriodType { get; set; } = SchedulePeriodType.Weeks;

        [Ignore]
        public Tenant Tenant { get; set; }

        [Ignore ]
        public IEnumerable <IPayment> Payments { get; set; }

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

            if(ParentID==Guid.Empty && (this.Tenant ==null || this.Tenant.TenantID==Guid.Empty  ) )
            {
                throw new ValidationException("The Tenant is invalid");
            }

            if(Period<1)
            {
                throw new ValidationException("The week period is invalid");
            }

            if (ParentID == Guid.Empty)
            {
                ParentID = this.Tenant.TenantID;
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

        private bool CreateNextRent(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }
            var totalWeeks = this.Period;
           
            DateTime startDate = DateTime.Parse(this.StartDate.ToShortDateString());


            var lastRent = uow.RentRepository.GetAll(x => x.ScheduleID == this.ScheduleID && x.Paid).OrderByDescending(x => x.PaidDate).FirstOrDefault();
            if (lastRent != null)
            {               
                totalWeeks = lastRent.TotalPaidWeeks;
                startDate = lastRent.ExpiryDate.AddDays(7* totalWeeks);
                if (startDate >= this.EndDate)
                {
                    return true;
                }
            }

            DateTime rentToDate = startDate.AddDays(7 * this.Period);
            var tenant = uow.TenantRepository.Get(this.ParentID);

            if (tenant == null)
            {
                throw new ValidationException("Tenant does not exist");
            }

            var rent = new Rent();
            rent.Price = tenant.Rent*this.Period ;
            rent.Paid = false;
            rent.ScheduleID = this.ScheduleID;
            rent.TenantID = this.ParentID;
            rent.ExpiryDate = startDate;
            rent.RentToDate = rentToDate;

            if (DateTime.Compare(rentToDate, this.EndDate) > 0)
            {
                rent.RentToDate = this.EndDate;
                var days = rent.RentToDate - rent.ExpiryDate;
                rent.Price = Math.Round((tenant.Rent / 7) * days.Days, 0);

            }
            return rent.Save(uow);
        }

        private bool CreateNextBill(IUnitOfWork uow)
        {


            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }
            var totalWeeks = this.Period;

            DateTime startDate = DateTime.Parse(this.StartDate.ToShortDateString());


            var lastBill = uow.BillRepository.GetAll(x => x.ScheduleID == this.ScheduleID && x.Paid).OrderByDescending(x => x.PaidDate).FirstOrDefault();
            if (lastBill == null)
            {
                return false;
            }


            startDate = lastBill.ExpiryDate.AddDays(7 * totalWeeks);
            switch (this.SchedulePeriodType)
            {
                case SchedulePeriodType.Days:
                    startDate = lastBill.ExpiryDate.AddDays(totalWeeks);
                    break;
                case SchedulePeriodType.Weeks:
                    startDate = lastBill.ExpiryDate.AddDays(7 * totalWeeks);
                    break;
                case SchedulePeriodType.Months:
                    startDate = lastBill.ExpiryDate.AddMonths ( totalWeeks);
                    break;
                default:
                    break;
            }



            if (startDate >= this.EndDate)
            {
                return true;
            }


            DateTime paymentToDate = startDate.AddDays(7 * this.Period);
            var place = uow.PlaceRepository.Get(this.ParentID);

            if (place == null)
            {
                throw new ValidationException("Place does not exist");
            }

            var bill = new Bill();
            bill.Price = lastBill.Price;
            bill.Paid = false;
            bill.ScheduleID = this.ScheduleID;
            bill.PlaceID = this.ParentID;
            bill.ExpiryDate = startDate;
            bill.RentToDate = paymentToDate;
            bill.Biller = lastBill.Biller;
            bill.Description = lastBill.Description;
            bill.IsScheduled = lastBill.IsScheduled;
            bill.ScheduleLastDate = lastBill.ScheduleLastDate;
            bill.SchedulePeriod = lastBill.SchedulePeriod;
            bill.SchedulePeriodType = lastBill.SchedulePeriodType;


            if (DateTime.Compare(paymentToDate, this.EndDate) > 0)
            {
                bill.RentToDate = this.EndDate;
                var days = bill.RentToDate - bill.ExpiryDate;
            }
            return bill.Save(uow);
        }

        public bool CreateNextPayment(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if (this.ScheduleType == ScheduleType.Rent)
            {
                return CreateNextRent(uow);
            }
            else
            {
                return CreateNextBill(uow);
            }
        }

    }
}
