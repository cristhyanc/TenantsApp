﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Interfaces;

namespace TenantsApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        IPlaceRepository _placeRepository;
        ITenantRepository _tenantRepository;
        ISchedulePaymentRepositoy _scheduleRentRepositoy;
        IRentRepository _rentRepository;
        IDropboxService _dropbox;
        IBillRepository _billRepository;

        private DBContext _context;

        public UnitOfWork(IDropboxService dropbox)
        {
            _context = new DBContext();
           
        }

        public void CloseConnection()
        {
            _context.CloseConnection();
        }

        public void RestartConnection()
        {
            _context.RestartConnection();
        }

        public IBillRepository BillRepository
        {
            get
            {
                if (_billRepository == null)
                {
                    _billRepository = new BillRepository(_context);
                }
                return _billRepository;
            }
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

        public ISchedulePaymentRepositoy ScheduleRentRepositoy
        {
            get
            {
                if (_scheduleRentRepositoy == null)
                {
                    _scheduleRentRepositoy = new SchedulePaymentRepositoy(_context);
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
