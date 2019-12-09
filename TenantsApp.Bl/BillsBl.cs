using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;

namespace TenantsApp.Bl
{
    public class BillsBl : IBillsBl
    {

        IUnitOfWork _uow;
       
        public BillsBl(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<Bill> GetUpcomingBills()
        {
          return  _uow.BillRepository.GetAll(x => !x.Paid);
        }

        public bool Save(Bill bill)
        {
            if(bill==null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            try
            {
                _uow.Begin();
                               
                if (bill.Save(_uow))
                {
                    _uow.Commit();
                    return true;
                }
                _uow.RollBack();
                return false;
            }
            catch (Exception)
            {
                _uow.RollBack();
                throw;
            }
           
        }

      
    }
}
