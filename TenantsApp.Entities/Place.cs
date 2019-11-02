using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Entities
{
   
  public  class Place
    {
        [PrimaryKey]
        public Guid PlaceID { get; set; }
        public string Description { get; set; }
        public string  Address { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public int Carparks { get; set; }
        public int TenantsCapacity { get; set; }

        [Ignore ]
        public List<Tenant> Tenants  { get; set; }

        

        public Place( )
        {
            
        }

        public bool Save(IUnitOfWork uow)
        {
            try
            {
                if(string.IsNullOrEmpty(this.Description ))
                {
                    throw new ValidationException("The Description is required");
                }

                if(string.IsNullOrEmpty(this.Address ))
                {
                    throw new ValidationException("The Address is required");
                }

                if(this.PlaceID ==Guid.Empty )
                {
                    this.PlaceID = Guid.NewGuid();
                    return uow.PlaceRepository.Insert (this);
                }

                return uow.PlaceRepository.Update (this);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
