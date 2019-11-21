using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PropertiesPageModel : BaseViewModel
    {

        IUserDialogs _userDialogs;

        
        public ICommand DeletePlaceCommand { get; set; }

        public ICommand AddCommand { get; set; }

        ObservableCollection<Place> places;
        public ObservableCollection<Place> Places
        {
            get { return this.places; }
            set { this.places = value; }
        }

        public decimal  TotalBond { get; set; }
        public decimal TotalSaved { get; set; }

        Place placeSelected;
        public Place PlaceSelected
        {
            get
            {
                return this.placeSelected;
            }
            set
            {
                if(this.placeSelected != value)
                {
                    this.placeSelected = value;
                    if(value!=null)
                    {
                         CoreMethods.PushPageModel<PropertyPageModel>(value);
                    }
                }
                
            }
        }

        IPlacesBl _placesBl;

        public PropertiesPageModel(IUserDialogs userDialogs, IPlacesBl placesBl) : base(userDialogs)
        {
            AddCommand = new Command(AddPlace);
            DeletePlaceCommand = new Command<Guid>((x) => { AttempToDeleteProperty(x); });
            _placesBl = placesBl;
            _userDialogs = userDialogs;           
        }

        private async Task AttempToDeleteProperty(Guid placeId)
        {
            try
            {
                this.placeSelected = this.Places.Where(x => x.PlaceID == placeId).FirstOrDefault();
                if (await this._userDialogs.ConfirmAsync("Do you want to delete this place"))
                {
                    DeletePlace();
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }

        private void DeletePlace()
        {
            try
            {
                if(_placesBl.DeletePlace(this.PlaceSelected.PlaceID  ))
                {
                    this.PlaceSelected = null;
                    this.GetCurrentPlaces();
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            GetCurrentPlaces();
        }

        private void GetCurrentPlaces()
        {
            try
            {
                this.TotalBond = 0;
                this.TotalSaved = 0;

                this.IsBusy = true;                
                Places = new ObservableCollection<Place>(_placesBl.GetCurrentPlaces());
               if(this.Places?.Count>0)
                {
                    foreach (var item in this.Places )
                    {
                        this.TotalBond += item.Tenants.Sum(x => x.Bond);
                        this.TotalSaved += item.TotalSaved;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void AddPlace()
        {
            try
            {               
                await CoreMethods.PushPageModel<PropertyPageModel>();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }
    }
}
