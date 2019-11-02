using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PropertiesPageModel : FreshBasePageModel
    {

        IUserDialogs _userDialogs;

        public ICommand AddCommand { get; set; }

        ObservableCollection<Place> places;
        public ObservableCollection<Place> Places
        {
            get { return this.places; }
            set { this.places = value; }
        }

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

        public PropertiesPageModel(IUserDialogs userDialogs, IPlacesBl placesBl)
        {
            AddCommand = new Command(AddPlace);
            _placesBl = placesBl;
            _userDialogs = userDialogs;
            //Places = new ObservableCollection<Place>();
            //var place = new Place { Description="71 Arthur", Address="U5 / 71 Arthur st, Fortitude Valley", Bathrooms=1, Rooms=2,TenantsCapacity=3, Carparks=1 };

            //Places.Add(place);

            //place = new Place { Description = "79 Berwick", Address = "U26 / 79 Berwick st, Fortitude Valley", Bathrooms = 2, Rooms = 2, TenantsCapacity = 4, Carparks = 1 };
            //Places.Add(place);

            //place = new Place { Description = "Emporium", Address = "U211 / 1000 Ann St, Fortitude Valley", Bathrooms = 2, Rooms = 2, TenantsCapacity = 4, Carparks = 1 };
            //Places.Add(place);

        }

        public override void Init(object initData)
        {
            GetCurrentPlaces();
        }

        private async Task GetCurrentPlaces()
        {
            try
            {
               // _userDialogs.Loading();
               await Task.Run(() => {
                   Places = new ObservableCollection<Place>(_placesBl.GetCurrentPlaces());
                //   _userDialogs.HideLoading();
                });
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
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
