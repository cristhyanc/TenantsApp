using System;
using System.Collections.Generic;
using System.Text;

namespace TenantsApp.Entities.Interfaces
{
   public interface IUnitOfWork
    {
        IPlaceRepository PlaceRepository { get; }
        ITenantRepository TenantRepository { get; }
        IScheduleRentRepositoy ScheduleRentRepositoy { get; }
        IRentRepository RentRepository { get; }

        void Begin();
        void Commit();
        void RollBack();
        void RollBack(string savePoint);
    }
}
