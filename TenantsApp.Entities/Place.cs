using System;
using System.Collections.Generic;
using System.Text;

namespace TenantsApp.Entities
{
  public  class Place
    {
        public Guid PlaceID { get; set; }
        public string Description { get; set; }
        public string  Address { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public int Carparks { get; set; }
        public int TenantsCapacity { get; set; }

        public List<Tenant> Tenants  { get; set; }
    }
}
