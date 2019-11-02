using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
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
    public class PropertyPageModel: FreshBasePageModel
    {

        IUserDialogs _userDialogs;

        public ICommand SaveCommand { get; set; }

        public Place Place { get; set; }

        IPlacesBl _placesBl;

        public PropertyPageModel(IUserDialogs userDialogs, IPlacesBl placesBl )
        {
          
            _userDialogs = userDialogs;
            _placesBl = placesBl;
            SaveCommand = new Command(Save);
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
                  await  CoreMethods.PopPageModel();
                }
            }           
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(PropertyPageModel));
            }
        }
    }
}
