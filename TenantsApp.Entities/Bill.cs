using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared;
using TenantsApp.Shared.Exceptions;
using Xamarin.Forms;

namespace TenantsApp.Entities
{
   public class Bill: IPayment
    {
        [PrimaryKey]
        public Guid PaymentID { get; set; }
        public Guid ScheduleID { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public Guid PlaceID { get; set; }
        public string Description { get; set; }
        public string Biller { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime ScheduleLastDate { get; set; }
        public int SchedulePeriod { get; set; }
        public SchedulePeriodType SchedulePeriodType { get; set; }      

        [Ignore ]
        public Place Place { get; set; }
        public DateTime RentToDate { get; set; }

        [Ignore]
        public Color ItemBackgroundColor
        {
            get
            {
                DateTime dateNow = DateTime.Parse(DateTime.Now.ToShortDateString());
                this.ExpiryDate = DateTime.Parse(this.ExpiryDate.ToShortDateString());

                if (this.Paid)
                {
                    return Color.LightGreen;
                }

                if (this.ExpiryDate < dateNow)
                {
                    return Color.LightPink;
                }
                else
                {
                    if (this.ExpiryDate >= dateNow && this.ExpiryDate < dateNow.AddDays(3))
                    {
                        return Color.LightYellow;
                    }
                    else
                    {
                        return Color.White;
                    }
                }
            }
        }


        public bool Save(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if (PlaceID == Guid.Empty && (this.Place == null || this.Place.PlaceID == Guid.Empty))
            {
                throw new ValidationException("The Tenant is invalid");
            }

            if (this.PlaceID == Guid.Empty)
            {
                this.PlaceID = this.Place.PlaceID;
            }

            if (this.PaymentID == Guid.Empty)
            {
                this.PaymentID = Guid.NewGuid();

                var result = uow.BillRepository.Insert(this);

                if (this.Paid)
                {
                    return this.PaidBill(uow);
                }

                return result;
            }
            else
            {

                var old = uow.BillRepository.Get(this.PaymentID);
                var result = uow.BillRepository.Update(this);               

                if (result && this.Paid && !old.Paid)
                {
                    return PaidBill(uow);
                }
                return result;
            }

        }


        public bool PaidBill(IUnitOfWork uow)
        {

            var place = uow.PlaceRepository.Get(this.PlaceID);
            place.TotalSaved -= this.Price;
            
            if(this.IsScheduled)
            {
                SchedulePayment schedule;
                if(this.ScheduleID==Guid.Empty )
                {
                    schedule = new SchedulePayment();
                    schedule.EndDate = this.ScheduleLastDate;
                    schedule.ParentID = this.PlaceID;
                    schedule.Period = this.SchedulePeriod;
                    schedule.ScheduleID = Guid.NewGuid();
                    this.ScheduleID = schedule.ScheduleID;
                    schedule.ScheduleType = ScheduleType.Bill;
                    schedule.StartDate = DateTime.Now;
                    schedule.SchedulePeriodType = this.SchedulePeriodType;
                    uow.ScheduleRentRepositoy.Insert(schedule);
                    uow.BillRepository.Update(this);

                }
                else
                {
                    schedule = uow.ScheduleRentRepositoy.Get(this.ScheduleID);
                }

                schedule.CreateNextPayment(uow);
            }

            return uow.PlaceRepository.Update(place);

        }


    }
}
