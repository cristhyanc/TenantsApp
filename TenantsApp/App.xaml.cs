using FreshMvvm;
using System;
using TenantsApp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Repository;
using Acr.UserDialogs;
using TenantsApp.Services;
using TenantsApp.Shared.Interfaces;
using TenantsApp.Bl;

namespace TenantsApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            try
            {

                FreshIOC.Container.Register<ISchedulePaymentRepositoy, SchedulePaymentRepositoy>();
                FreshIOC.Container.Register<ITenantRepository, TenantRepository>();
                FreshIOC.Container.Register<IRentRepository, RentRepository>();
                FreshIOC.Container.Register<IPlaceRepository, PlaceRepository>();
                FreshIOC.Container.Register<IEmailService, EmailService>();
                FreshIOC.Container.Register<IDropboxService, DropBoxService>();
                FreshIOC.Container.Register<IBillsBl, BillsBl>();
                


                FreshIOC.Container.Register<IUnitOfWork, UnitOfWork>().AsSingleton();
                FreshIOC.Container.Register<IUserDialogs>(UserDialogs.Instance);
                FreshIOC.Container.Register<Bl.IPlacesBl, Bl.PlacesBl>();
                FreshIOC.Container.Register<Bl.ITenantsBl, Bl.TenantsBl>();
                FreshIOC.Container.Register<Bl.IScheduleBl, Bl.ScheduleBl>();
                
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU5MTk4QDMxMzcyZTMzMmUzMGFvd0UvK2ZtUm5LSXYxRE9Rc3NvZnBOUFZqTHJqWkF2WVFKa1JVRENETFk9");
                InitializeComponent();

                var mainPage = new FreshTabbedNavigationContainer();

               //  mainPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                //mainPage.UnselectedTabColor = Color.Black;
                //mainPage.SelectedTabColor = Color.DodgerBlue;

                mainPage.AddTab<UpcomingRentPageModel>("Rents", null);
                mainPage.AddTab<UpcomingBillsPageModel>("Bills", null);             
                mainPage.AddTab<PropertiesPageModel>("Places", null);
                mainPage.AddTab<SettingsPageModel>("Setting", null);


                MainPage = mainPage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
