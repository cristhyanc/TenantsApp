using Acr.UserDialogs;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TenantsApp
{
  public  class BaseViewModel: FreshBasePageModel
    {
            
        bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if(_isBusy!=value )
                {
                    _isBusy = value;

                    if(value )
                    {
                        _userDialogs.ShowLoading();
                    }
                    else
                    {
                        _userDialogs.HideLoading();
                    }
                }
               
            }
        }
        IUserDialogs _userDialogs;

        public BaseViewModel(IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
           
        }

       
    }
}
