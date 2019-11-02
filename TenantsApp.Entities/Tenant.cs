using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
