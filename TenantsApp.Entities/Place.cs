using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public decimal Rent { get; set; }
        public decimal TotalSaved { get; set; }

        [Ignore ]
        public List<Tenant> Tenants  { get; set; }

        [Ignore]
        public int TotalTenants
        {
            get
            {
                if (this.Tenants?.Count > 0)
                {
                    return this.Tenants.Count;

                }
                return 0;
            }
        }

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

        public bool AddTenant(Tenant tenant, IUnitOfWork uow)
        {
            try
            {
                tenant.PlaceID = this.PlaceID;
                if (tenant.Save(uow))
                {
                    if(this.Tenants==null)
                    {
                        this.Tenants = new List<Tenant>();
                    }
                    this.Tenants.Add(tenant);
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(IUnitOfWork uow)
        {           
                if (this.PlaceID == Guid.Empty)
                {
                    throw new ValidationException("The place ID is required");
                }
                var tenants = uow.TenantRepository.GetAll(x => x.PlaceID == this.PlaceID);
              
                if(tenants?.Count>0)
                {
                    foreach (var item in tenants)
                    {
                        item.Delete(uow);
                    }
                }
                return uow.PlaceRepository.Delete(this);           
        }

        public void LoadTenants(IUnitOfWork uow)
        {
            this.Tenants = uow.TenantRepository.GetAll(x => x.PlaceID == this.PlaceID);
           
        }
    }
}
