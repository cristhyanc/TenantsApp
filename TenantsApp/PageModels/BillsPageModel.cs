using Acr.UserDialogs;
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
   public class BillsPageModel : BaseViewModel
    {

        public bool ShowAddButton { get; set; }

        public decimal Total { get; set; }

        Bill _billSelected;
        public Bill BillSelected
        {
            get { return _billSelected; }
            set
            {
                if(_billSelected!=value && value!= null)
                {
                     CoreMethods.PushPageModel<BillPageModel>(value);
                }
                    _billSelected = value;
            }
        }

        public ObservableCollection<Bill> Bills { get; set; }

        public ICommand AddCommand { get; set; }

        public Place Place { get; set; }
        IPlacesBl _placesBl;

        public BillsPageModel(IUserDialogs userDialogs, IPlacesBl placesBl) : base(userDialogs)
        {
            AddCommand = new Command(GoToBill);
            _placesBl = placesBl;
        }

        private async void GoToBill()
        {
            try
            {
                await CoreMethods.PushPageModel<BillPageModel>(this.Place);
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(BillsPageModel));
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
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(BillsPageModel));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            LoadBills();
        }

        private void LoadBills()
        {
            try
            {
                ShowAddButton = true;
                if (Place != null)
                {
                    this.Bills = new ObservableCollection<Bill>(_placesBl.GetBills(this.Place.PlaceID));
                    if(this.Bills?.Count>0)
                    {
                        ShowAddButton = false;
                        Total = this.Bills.Where(x=> x.Paid ).Sum(x => x.Price );
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(BillsPageModel));
            }
        }

    }
}
