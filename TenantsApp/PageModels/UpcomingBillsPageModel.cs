using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UpcomingBillsPageModel : BaseViewModel
    {
        IUserDialogs _userDialogs;
        ObservableCollection<Bill> bills;
        public ObservableCollection<Bill> Bills
        {
            get { return this.bills; }
            set { this.bills = value; }
        }

        Bill billSelected;
        public Bill BillSelected
        {
            get
            {
                return this.billSelected;
            }
            set
            {
                this.billSelected = value;
            }
        }

        public bool DisplayPayConfirmation { get; set; }
      
        public decimal TotalPaid { get; set; }

        public ICommand PayBillCommand { get; set; }
        

        IBillsBl _billBl;

        public UpcomingBillsPageModel(IUserDialogs userDialogs, IBillsBl billBl) : base(userDialogs)
        {
            _billBl = billBl;
            _userDialogs = userDialogs;
            PayBillCommand = new Command<Guid>((x) => { PayBill(x); });
            
        }

        private void LoadBills()
        {
            try
            {
                this.Bills = new ObservableCollection<Bill>(_billBl.GetUpcomingBills());               
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(UpcomingBillsPageModel));
            }
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            LoadBills();
        }

        private async void PayBill(Guid paymentID)
        {
            try
            {
                this.IsBusy = true;
                if(await _userDialogs.ConfirmAsync("Do you want to mark this bill as paid?", "", "Yes", "No"))
                {
                    if (!_billBl.PaidBill(paymentID))
                    {
                        _userDialogs.Alert("The rent could not be proccessed");
                    }
                    else
                    {
                        _userDialogs.Alert("Done");
                       
                    }
                    LoadBills();
                }

              
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
            finally
            {
                this.IsBusy = false;
            }
        }
               
    }
}
