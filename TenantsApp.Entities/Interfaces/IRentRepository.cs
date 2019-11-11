using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
    public interface IRentRepository
    {
        Rent Get(Guid id);
        Rent Get(Expression<Func<Rent, bool>> predicate);
        List<Rent> GetAll();
        List<Rent> GetAll(Expression<Func<Rent, bool>> predicate = null);
        bool Insert(Rent entity);
        bool Insert(IEnumerable<Rent> entities);
        bool Update(Rent entity);
        bool Update(IEnumerable<Rent> entities);
        bool Delete(Rent entity);
        bool Delete(IEnumerable<Rent> entities);
    }
}
