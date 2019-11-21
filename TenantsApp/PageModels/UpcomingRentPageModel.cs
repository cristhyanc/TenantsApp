﻿using Acr.UserDialogs;
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

        public bool DisplayPayConfirmation { get; set; }
        public int Weeks { get; set; }
        public decimal TotalPaid { get; set; }

        public ICommand PayRentCommand { get; set; }
        public ICommand ConfirmPaymentCommand { get; set; }
        
        IScheduleBl _scheduleBl;

        public UpcomingRentPageModel(IUserDialogs userDialogs, IScheduleBl scheduleBl) : base(userDialogs)
        {
            _scheduleBl = scheduleBl;
              _userDialogs = userDialogs;         
            PayRentCommand = new Command<Guid>((x) => { PrepartePayRent(x); });
            ConfirmPaymentCommand = new Command(PayRent);
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

        private async void PayRent()
        {
            try
            {
                this.IsBusy = true;
                bool email = await _userDialogs.ConfirmAsync("Do you want to send an email?", "", "Yes", "No");

                if (!await _scheduleBl.PayRent(this.RentSelected.RentID , email, "Cristhyan",this.Weeks,this.TotalPaid ))
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
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void PrepartePayRent(Guid rentId)
        {
            try
            {
                this.RentSelected = this.Rents.Where(x => x.RentID == rentId).FirstOrDefault();
                var sched = _scheduleBl.GetScheduleByRent(rentId);
                this.Weeks = sched.Period;
                this.TotalPaid = this.RentSelected.Price;
                DisplayPayConfirmation = true;
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
