using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Entities
{
   public class Tenant
    {
        [PrimaryKey]
        public Guid TenantID { get; set; }
        public Guid PlaceID { get; set; }
        public string Name { get; set; }
        public decimal  Rent { get; set; }
        public decimal Bond { get; set; }
        public int PayWeekPeriod { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

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
                    return uow.TenantRepository .Insert(this);
                }

                return uow.TenantRepository.Update(this);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
