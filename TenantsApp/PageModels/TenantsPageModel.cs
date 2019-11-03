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
    [PropertyChanged.AddINotifyPropertyChangedInterface]
  public  class TenantsPageModel: BaseViewModel
    {
        IUserDialogs _userDialogs;

        ITenantsBl _tenantsBl;

        public ICommand AddTenantCommand { get; set; }
        
        public Place Place { get; set; }

        ObservableCollection<Tenant > tenants;
        public ObservableCollection<Tenant> Tenants
        {
            get { return this.tenants; }
            set { this.tenants = value; }
        }

        Tenant tenantSelected;
        public Tenant TenantSelected
        {
            get
            {
                return this.tenantSelected;
            }
            set
            {
                if (this.tenantSelected != value)
                {
                    this.tenantSelected = value;
                    if (value != null)
                    {
                        var paramenters = new TenantPageModel.TenantPageParameters();
                        paramenters.Place = this.Place;
                        paramenters.Tenant = value;

                        CoreMethods.PushPageModel<TenantPageModel>(paramenters);
                    }
                }

            }
        }

        public TenantsPageModel(IUserDialogs userDialogs, ITenantsBl tenantsBl) : base(userDialogs)
        {

            _userDialogs = userDialogs;
            _tenantsBl = tenantsBl;
            //SaveCommand = new Command(Save);
            AddTenantCommand = new Command(GoToTenant);

            
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            GetCurrentTenants();
        }

        private void GetCurrentTenants()
        {
            try
            {
               if(this.Place!=null)
                {
                    this.Tenants = new ObservableCollection<Tenant>(_tenantsBl.GetTenantsByPlace(this.Place.PlaceID));
                }               
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
           
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);               

                if (initData != null)
                {
                    this.Place = (Place)initData;                   
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        private async void GoToTenant()
        {
            try
            {
                var paramenters = new TenantPageModel.TenantPageParameters();
                paramenters.Place = this.Place;

                await CoreMethods.PushPageModel<TenantPageModel>(paramenters);
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }
    }
}
