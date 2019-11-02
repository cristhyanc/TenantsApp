using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {

        public PlaceRepository(DBContext context) : base(context)
        {

        }       
    }
}
