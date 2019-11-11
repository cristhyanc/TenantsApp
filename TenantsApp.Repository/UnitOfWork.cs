using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        IPlaceRepository _placeRepository;
        ITenantRepository _tenantRepository;
        IScheduleRentRepositoy _scheduleRentRepositoy;
        IRentRepository _rentRepository;
        private DBContext _context;

        public UnitOfWork()
        {
            _context = new DBContext();
        }

        public IRentRepository RentRepository
        {
            get
            {
                if (_rentRepository == null)
                {
                    _rentRepository = new RentRepository(_context);
                }
                return _rentRepository;
            }
        }

        public IScheduleRentRepositoy ScheduleRentRepositoy
        {
            get
            {
                if (_scheduleRentRepositoy == null)
                {
                    _scheduleRentRepositoy = new ScheduleRentRepositoy(_context);
                }
                return _scheduleRentRepositoy;
            }
        }

        public ITenantRepository TenantRepository
        {
            get
            {
                if (_tenantRepository == null)
                {
                    _tenantRepository = new TenantRepository(_context);
                }
                return _tenantRepository;
            }
        }

        public IPlaceRepository PlaceRepository
        {
            get
            {
                if (_placeRepository == null)
                {
                    _placeRepository = new PlaceRepository(_context );
                }
                return _placeRepository;
            }
        }

        public void Begin()
        {
            _context.BeginTransaction();
        }

        public void Commit()
        {
            _context.CommitTransaction();
        }

        public void RollBack()
        {
            _context.RollbackTransaction();
        }

        public void RollBack(string savePoint)
        {
            _context.RollbackTransaction(savePoint);
        }
    }
}
