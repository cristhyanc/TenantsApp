using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
  public  interface IPlacesBl
    {
        bool SavePlace(Place place);
        IList<Place> GetCurrentPlaces();
    }
}
