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
        public ICommand ConfirmPaymentCommand { get; set; }

        IBillsBl _billBl;

        public UpcomingBillsPageModel(IUserDialogs userDialogs, IBillsBl billBl) : base(userDialogs)
        {
            _billBl = billBl;
            _userDialogs = userDialogs;
            PayBillCommand = new Command<Guid>((x) => { PrepartePayRent(x); });
            ConfirmPaymentCommand = new Command(PayBill);
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

        private async void PayBill()
        {
            //try
            //{
            //    this.IsBusy = true;
            //    bool email = await _userDialogs.ConfirmAsync("Do you want to send an email?", "", "Yes", "No");

            //    if (!await _billBl.PayRent(this.BillSelected.PaymentID, email, "Cristhyan", this.Weeks, this.TotalPaid))
            //    {
            //        _userDialogs.Alert("The rent could not be proccessed");
            //    }
            //    else
            //    {
            //        _userDialogs.Alert("Done");
            //        LoadBills();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            //}
            //finally
            //{
            //    this.IsBusy = false;
            //}
        }

        private async void PrepartePayRent(Guid paymentID)
        {
            //try
            //{
            //    this.BillSelected = this.Bills.Where(x => x.PaymentID == paymentID).FirstOrDefault();
            //    var sched = _billBl.GetScheduleByRent(paymentID);
            //    this.Weeks = sched.Period;
            //    this.TotalPaid = this.BillSelected.Price;
            //    DisplayPayConfirmation = true;
            //}
            //catch (Exception ex)
            //{
            //    Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            //}
            //finally
            //{
            //    this.IsBusy = false;
            //}
        }
    }
}
