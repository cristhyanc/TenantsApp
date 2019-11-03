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
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public  class TenantPageModel: BaseViewModel
    {

        public class TenantPageParameters
        {
            public Tenant Tenant { get; set; }
            public Place Place { get; set; }
        }

        IUserDialogs _userDialogs;
        ITenantsBl _tenantsBl;
        public ICommand SaveCommand { get; set; }

        public Tenant Tenant { get; set; }
        public Place Place { get; set; }

        public TenantPageModel(IUserDialogs userDialogs, ITenantsBl tenantsBl) : base(userDialogs)
        {

            _userDialogs = userDialogs;
            _tenantsBl = tenantsBl;
            SaveCommand = new Command(Save);
           
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
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantPageModel));
            }
        }
    }
}
