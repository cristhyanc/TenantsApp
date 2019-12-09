using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp.Entities.Interfaces
{
    public interface ISchedulePaymentRepositoy
    {
        SchedulePayment Get(Guid id);
        SchedulePayment Get(Expression<Func<SchedulePayment, bool>> predicate);
        List<SchedulePayment> GetAll();
        List<SchedulePayment> GetAll(Expression<Func<SchedulePayment, bool>> predicate = null);
        bool Insert(SchedulePayment entity);
        bool Insert(IEnumerable<SchedulePayment> entities);
        bool Update(SchedulePayment entity);
        bool Update(IEnumerable<SchedulePayment> entities);
        bool Delete(SchedulePayment entity);
        bool Delete(IEnumerable<SchedulePayment> entities);
    }
}
