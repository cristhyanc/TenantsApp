using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;
using Xamarin.Forms;

namespace TenantsApp.Entities
{
    public class Tenant
    {
        [PrimaryKey]
        public Guid TenantID { get; set; }
        public Guid PlaceID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Rent { get; set; }
        public decimal Bond { get; set; }
        public int PayWeekPeriod { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }


        [Ignore]
        public Color ItemBackgroundColor
        {
            get
            {
                if(this.End.HasValue )
                {
                    DateTime dateNow = DateTime.Parse(DateTime.Now.ToShortDateString());
                    this.End = DateTime.Parse(this.End.Value .ToShortDateString());

                    if (this.End < dateNow)
                    {
                        return Color.WhiteSmoke; ;
                    }
                }
               
                return Color.White;
            }
        }

        [Ignore]
        public ScheduleRent ScheduleRent { get; set; }

        [Ignore]
        public Place Place { get; set; }

        public bool Save(IUnitOfWork uow)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    throw new ValidationException("The Name is required");
                }

                if (this.TenantID == Guid.Empty)
                {
                    this.TenantID = Guid.NewGuid();
                    return uow.TenantRepository.Insert(this);
                }

                return uow.TenantRepository.Update(this);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(IUnitOfWork uow)
        {

            if (this.TenantID == Guid.Empty)
            {
                throw new ValidationException("The Tenant ID is required");
            }

            var schedules = uow.ScheduleRentRepositoy.GetAll(x => x.TenantID == this.TenantID);
            if(schedules?.Count>0)
            {
                schedules.ForEach(x => x.Delete(uow));
            }
            return uow.TenantRepository.Delete(this);
        }

    }
}
