using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
   public interface IPlaceRepository
    {
        Place Get(Guid id);
        Place Get(Expression<Func<Place, bool>> predicate);
        List<Place> GetAll();
        List<Place> GetAll(Expression<Func<Place, bool>> predicate = null);
        bool  Insert(Place entity);
        bool Insert(IEnumerable<Place> entities);
        bool Update(Place entity);
        bool Update(IEnumerable<Place> entities);
        bool Delete(Place entity);
        bool Delete(IEnumerable<Place> entities);
    }
}
