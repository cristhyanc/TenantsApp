using System;
using System.Collections.Generic;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
   public interface IUnitOfWork
    {
        IPlaceRepository PlaceRepository { get; }
        ITenantRepository TenantRepository { get; }
        ISchedulePaymentRepositoy ScheduleRentRepositoy { get; }
        IRentRepository RentRepository { get; }
        IBillRepository BillRepository { get; }
        void RestartConnection();
        void Begin();
        void Commit();
        void RollBack();
        void RollBack(string savePoint);
    }
}
