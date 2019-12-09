using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
    public interface IBillRepository
    {
        Bill Get(Guid id);
        Bill Get(Expression<Func<Bill, bool>> predicate);
        List<Bill> GetAll();
        List<Bill> GetAll(Expression<Func<Bill, bool>> predicate = null);
        bool Insert(Bill entity);
        bool Insert(IEnumerable<Bill> entities);
        bool Update(Bill entity);
        bool Update(IEnumerable<Bill> entities);
        bool Delete(Bill entity);
        bool Delete(IEnumerable<Bill> entities);
    }
}
