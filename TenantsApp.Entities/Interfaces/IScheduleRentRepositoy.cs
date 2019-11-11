using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Entities.Interfaces
{
    public interface IScheduleRentRepositoy
    {
        ScheduleRent Get(Guid id);
        ScheduleRent Get(Expression<Func<ScheduleRent, bool>> predicate);
        List<ScheduleRent> GetAll();
        List<ScheduleRent> GetAll(Expression<Func<ScheduleRent, bool>> predicate = null);
        bool Insert(ScheduleRent entity);
        bool Insert(IEnumerable<ScheduleRent> entities);
        bool Update(ScheduleRent entity);
        bool Update(IEnumerable<ScheduleRent> entities);
        bool Delete(ScheduleRent entity);
        bool Delete(IEnumerable<ScheduleRent> entities);
    }
}
