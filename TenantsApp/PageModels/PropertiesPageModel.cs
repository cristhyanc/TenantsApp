using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TenantsApp.Entities;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PropertiesPageModel : FreshBasePageModel
    {

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
                this.placeSelected = value;
            }
        }


        public PropertiesPageModel()
        {
            AddCommand = new Command(AddPlace);

            Places = new ObservableCollection<Place>();
            var place = new Place { Description="71 Arthur", Address="U5 / 71 Arthur st, Fortitude Valley", Bathrooms=1, Rooms=2,TenantsCapacity=3, Carparks=1 };
           
            Places.Add(place);

            place = new Place { Description = "79 Berwick", Address = "U26 / 79 Berwick st, Fortitude Valley", Bathrooms = 2, Rooms = 2, TenantsCapacity = 4, Carparks = 1 };
            Places.Add(place);

            place = new Place { Description = "Emporium", Address = "U211 / 1000 Ann St, Fortitude Valley", Bathrooms = 2, Rooms = 2, TenantsCapacity = 4, Carparks = 1 };
            Places.Add(place);
           
        }

        private async void AddPlace()
        {
            try
            {
               
                await CoreMethods.PushPageModel<PropertyPageModel>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
