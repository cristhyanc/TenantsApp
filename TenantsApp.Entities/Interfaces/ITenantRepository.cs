using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
   public interface ITenantRepository
    {
        Tenant Get(Guid id);
        Tenant Get(Expression<Func<Tenant, bool>> predicate);
        List<Tenant> GetAll();
        List<Tenant> GetAll(Expression<Func<Tenant, bool>> predicate = null);
        bool Insert(Tenant entity);
        bool Insert(IEnumerable<Tenant> entities);
        bool Update(Tenant entity);
        bool Update(IEnumerable<Tenant> entities);
        bool Delete(Tenant entity);
        bool Delete(IEnumerable<Tenant> entities);
    }
}
