using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;
using Xamarin.Forms;

namespace TenantsApp.Entities
{
  public  class Rent
    {
        [PrimaryKey]
        public Guid RentID { get; set; }
        public Guid TenantID { get; set; }
        public Guid ScheduleID { get; set; }
        public decimal  Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public int TotalPaidWeeks { get; set; }

        [Ignore]
        public Tenant Tenant { get; set; }

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
                    if(this.ExpiryDate>= dateNow && this.ExpiryDate< dateNow.AddDays(3))
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

        public bool PayRent(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            this.Paid = true;
            this.PaidDate = DateTime.Parse(DateTime.Now.ToString());

            return uow.RentRepository.Update(this);
        }

        public bool Save(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException(nameof(uow));
            }

            if (TenantID == Guid.Empty && (this.Tenant == null || this.Tenant.TenantID == Guid.Empty))
            {
                throw new ValidationException("The Tenant is invalid");
            }

            if (this.ScheduleID  == Guid.Empty )
            {
                throw new ValidationException("The ScheduleID is invalid");
            }

            if (this.RentID == Guid.Empty)
            {
                this.RentID = Guid.NewGuid();
                return uow.RentRepository .Insert(this);
            }

            return uow.RentRepository.Update(this);
        }
               
    }
}
