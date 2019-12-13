using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TenantsApp.Bl;
using TenantsApp.Entities;
using TenantsApp.Shared;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class BillPageModel : BaseViewModel
    {
                      

        public string SelectedSchedulePeriodType
        {
            get
            {
                return ConvertEnumToString(this.Bill.SchedulePeriodType);
            }
            set
            {
                this.Bill.SchedulePeriodType = ConvertStringToEnum(value);
            }
        }

        public static string ConvertEnumToString(Enum eEnum)
        {
            return Enum.GetName(eEnum.GetType(), eEnum);
        }

        public static SchedulePeriodType ConvertStringToEnum(string value)
        {
            return (SchedulePeriodType)Enum.Parse(typeof(SchedulePeriodType), value);
        }

        public List<string> SchedulePeriodTypeList
        {
            get
            {
                var list= Enum.GetValues(typeof(SchedulePeriodType)).Cast<SchedulePeriodType>().Select(x=> x.ToString()).ToList();
                return list;
            }
        }

        public Place Place { get; set; }
        public Bill Bill { get; set; }
        
        public ICommand SaveCommand { get; set; }

        public IBillsBl _billsBl { get; set; }

        public BillPageModel(IUserDialogs userDialogs, IBillsBl billsBl) : base(userDialogs)
        {
            SaveCommand = new Command(Save);
            _billsBl = billsBl;
        }

        private async void Save()
        {
            try
            {
                if (!_billsBl.Save(this.Bill))
                {
                    _userDialogs.Alert("The Bill could not be saved, try again");
                }
                await CoreMethods.PopPageModel();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(BillPageModel));
            }
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);

                if (initData != null)
                {
                   if(initData is Place  place)
                    {
                        this.Place = place;
                        this.Bill = new Bill();
                        this.Bill.PlaceID = this.Place.PlaceID;
                        this.Bill.ExpiryDate = DateTime.Now;
                        this.Bill.ScheduleLastDate = DateTime.Now.AddMonths(12);
                    }
                    else if (initData is Bill bill)
                    {
                        this.Bill = bill;
                        this.Place = bill.Place;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(BillPageModel));
            }
        }
    }
}
