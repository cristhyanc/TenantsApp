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
    public class UpcomingRentPageModel: BaseViewModel
    {
        IUserDialogs _userDialogs;
        ObservableCollection<Rent> rents;
        public ObservableCollection<Rent> Rents
        {
            get { return this.rents; }
            set { this.rents = value; }
        }

        Rent rentSelected;
        public Rent RentSelected
        {
            get
            {
                return this.rentSelected;
            }
            set
            {
                this.rentSelected = value;
            }
        }
        
        public ICommand PayRentCommand { get; set; }
        IScheduleBl _scheduleBl;

        public UpcomingRentPageModel(IUserDialogs userDialogs, IScheduleBl scheduleBl) : base(userDialogs)
        {
            _scheduleBl = scheduleBl;
              _userDialogs = userDialogs;         
            PayRentCommand = new Command<Guid>((x) => { PayRent(x); });

        }

        private void LoadRents()
        {
            try
            {
                this.Rents = new ObservableCollection<Rent>(_scheduleBl.GetUpcomingRents());
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            LoadRents();
        }

        private void PayRent(Guid rentId)
        {
            try
            {
              
                if(!_scheduleBl.PayRent(rentId))
                {
                    _userDialogs.Alert("The rent could not be proccessed");
                }
                else
                {
                    _userDialogs.Alert("Done");
                    LoadRents();
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }
    }
}
