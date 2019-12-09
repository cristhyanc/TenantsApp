using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
  public  interface IBillsBl
    {
        bool Save(Bill bill);

        IList  <Bill> GetUpcomingBills();
       
    }
}
