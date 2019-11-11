using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{
    public class TenantPageParameters
    {
        public Tenant Tenant { get; set; }
        public Place Place { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public  class TenantPageModel: BaseViewModel
    {

       

        IUserDialogs _userDialogs;
        ITenantsBl _tenantsBl;
        public ICommand SaveCommand { get; set; }

        public ICommand GoToRentsCommand { get; set; }

        public Tenant Tenant { get; set; }
        public Place Place { get; set; }

        public TenantPageModel(IUserDialogs userDialogs, ITenantsBl tenantsBl) : base(userDialogs)
        {

            _userDialogs = userDialogs;
            _tenantsBl = tenantsBl;
            SaveCommand = new Command(Save);
            GoToRentsCommand = new Command(GoToRents);


        }

        private async void GoToRents()
        {
            try
            {
                var para = new RentsPageModelParameters();
                para.Tenant  = this.Tenant ;
                await CoreMethods.PushPageModel<RentsPageModel>(para);
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }

        private async void Save()
        {
            try
            {
                if (_tenantsBl.SaveTenant (this.Place, this.Tenant ))
                {
                    await CoreMethods.PopPageModel();
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);               

                if (initData != null)
                {
                    var parameters = (TenantPageParameters)initData;
                    this.Place = parameters.Place;
                    this.Tenant = parameters.Tenant;
                }

                if (this.Tenant==null)
                {
                    this.Tenant = new Tenant();
                    this.Tenant.Start = DateTime.Now;
                    this.Tenant.End = DateTime.Now.AddYears(1);
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantPageModel));
            }
        }
    }
}
