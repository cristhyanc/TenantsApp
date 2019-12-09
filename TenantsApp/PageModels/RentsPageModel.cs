﻿using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{

    public class RentsPageModelParameters
    {
        public Place Place { get; set; }
        public Tenant Tenant { get; set; }
    }


    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RentsPageModel: BaseViewModel
    {

        IUserDialogs _userDialogs;

        ObservableCollection<Rent> rents;
        public ObservableCollection<Rent> Rents
        {
            get { return this.rents; }
            set { this.rents = value; }
        }

        public ICommand OpenAddCommand { get; set; }
        public ICommand SaveScheduleCommand { get; set; }
        public ICommand DeleteScheduleCommand { get; set; }
        public bool DisplayPopup { get; set; }
        public bool ShowAddButton { get; set; }
        public bool IsEditButtonVisible { get; set; }
        public bool IsDeleteButtonVisible { get; set; }
        public decimal Total { get; set; }

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

        public SchedulePayment ScheduleRent { get; set; }
        
        RentsPageModelParameters parameters { get;  set; }
        IScheduleBl _scheduleBl;
        IPlacesBl _placesBl;

        public RentsPageModel(IUserDialogs userDialogs, IScheduleBl scheduleBl, IPlacesBl placesBl) : base(userDialogs)
        {
            _scheduleBl = scheduleBl;
            _userDialogs = userDialogs;
            _placesBl = placesBl;
            OpenAddCommand = new Command(OpenPopup);
            SaveScheduleCommand = new Command(SaveSchedule);
            DeleteScheduleCommand = new Command(DeleteSchedule);
        }

        private async void DeleteSchedule()
        {
            try
            {
                if (await _userDialogs.ConfirmAsync("Do you want to delete this rent schedule?"))
                {
                    if(_scheduleBl.DeleteSchedule(this.ScheduleRent.ScheduleID))
                    {
                        await CoreMethods.PopPageModel();
                    }
                    else
                    {
                        _userDialogs.Alert("It Could not be deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }

        private async void SaveSchedule()
        {
            try
            {
                this.ScheduleRent.ScheduleType = Shared.ScheduleType.Rent;
               if (_scheduleBl.SaveSchedule(this.ScheduleRent ))
                {
                    await CoreMethods.PopPageModel();
                }
                else
                {
                    _userDialogs.Alert("The Schedule could not be created");
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }


        private void OpenPopup()
        {
            this.DisplayPopup = true;
        }


        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);
                this.ShowAddButton = false;
                if (initData != null)
                {
                    this.parameters = (RentsPageModelParameters)initData;                 
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }

        private void LoadRents()
        {
            try
            {
                if (parameters.Tenant != null)
                {
                    this.ScheduleRent = _scheduleBl.GetTenantSchedule(this.parameters.Tenant.TenantID);
                    if (this.ScheduleRent == null)
                    {
                        this.ScheduleRent = new SchedulePayment();
                        this.ScheduleRent.StartDate = DateTime.Now;
                        this.ScheduleRent.EndDate = DateTime.Now.AddYears(1);
                        this.ScheduleRent.Period = this.parameters.Tenant.PayWeekPeriod;
                        this.ScheduleRent.ParentID = this.parameters.Tenant.TenantID;
                    }
                    LoadSchedulaRents();
                }
                else if (parameters.Place != null)
                {
                    LoadPropertyRents();
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }

        private void LoadPropertyRents()
        {
            try
            {

                if (this.parameters.Place  != null && this.parameters.Place.PlaceID  != Guid.Empty)
                {
                    this.Rents = new ObservableCollection<Rent>(_placesBl.GetRents(this.parameters.Place.PlaceID));
                    this.Total = this.Rents.Where(x => x.Paid).Sum(x => x.Price);
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(RentsPageModel));
            }
        }

        private void LoadSchedulaRents()
        {
            try
            {
             
                if (this.ScheduleRent != null && this.ScheduleRent.ScheduleID != Guid.Empty)
                {
                    this.IsDeleteButtonVisible = true;
                    this.IsEditButtonVisible = true;
                    var rent = _scheduleBl.GetRents(this.ScheduleRent.ScheduleID);
                    rent.ForEach(x => x.Tenant = this.parameters.Tenant);
                    this.Rents = new ObservableCollection<Rent>(rent);
                    this.Total = this.Rents.Where(x=> x.Paid ).Sum(x => x.Price);                   
                }
                else
                {
                    this.ShowAddButton = true;
                }
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

    }
}
