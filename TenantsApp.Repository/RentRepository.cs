using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
    public class RentRepository : Repository<Rent>, IRentRepository
    {

        public RentRepository(DBContext context) : base(context)
        {

        }
    }
}
