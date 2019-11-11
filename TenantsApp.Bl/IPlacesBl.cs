using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Bl
{
  public  interface IPlacesBl
    {
        bool SavePlace(Place place);        
        bool DeletePlace(Guid  placeId);
        IList<Place> GetCurrentPlaces();
        Place GetPlace(Guid placeId);
        IList<Rent> GetRents(Guid placeID); 
    }
}
