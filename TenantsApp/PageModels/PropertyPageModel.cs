using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Exceptions;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PropertyPageModel: BaseViewModel
    {

        IUserDialogs _userDialogs;

        public ICommand SaveCommand { get; set; }
        public ICommand AddTenatCommand { get; set; }
        public ICommand GoToRentsCommand { get; set; }
        public int TotalTenants { get; set; }

        public decimal?  TotalBond { get; set; }

        public Place Place { get; set; }

        IPlacesBl _placesBl;

        public PropertyPageModel(IUserDialogs userDialogs, IPlacesBl placesBl ): base(userDialogs)
        {
          
            _userDialogs = userDialogs;
            _placesBl = placesBl;
            SaveCommand = new Command(Save);
            AddTenatCommand = new Command(GoToTenants);
            GoToRentsCommand = new Command(GoToRents);
        }

        private async void GoToRents()
        {
            try
            {
                var para = new RentsPageModelParameters();
                para.Place = this.Place;
                await CoreMethods.PushPageModel<RentsPageModel>(para);
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }


        private void GoToTenants()
        {
            try
            {
                CoreMethods.PushPageModel<TenantsPageModel>(Place );
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            try
            {
                if (this.Place != null && this.Place.PlaceID !=Guid.Empty )
                {
                    this.Place = _placesBl.GetPlace(this.Place.PlaceID);
                    TotalTenants = this.Place.Tenants.Count;
                    TotalBond = this.Place.Tenants?.Sum(x => x.Bond);
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
                this.Place = new Place();

                if (initData != null)
                {
                    this.Place = (Place)initData;                  
                }
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
                if(_placesBl.SavePlace(this.Place))
                {
                    await CoreMethods.PopPageModel();
                }
            }           
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }
    }
}
