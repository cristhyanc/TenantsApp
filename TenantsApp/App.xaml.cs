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
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace TenantsApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                MainPage = new ContentPage();
                InitApp();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       public static  void InitApp()
        {
            try
            {

                IFreshIOC container = new FreshMvvm.FreshTinyIOCBuiltIn();

                container.Register<ISchedulePaymentRepositoy, SchedulePaymentRepositoy>();
                container.Register<ITenantRepository, TenantRepository>();
                container.Register<IRentRepository, RentRepository>();
                container.Register<IPlaceRepository, PlaceRepository>();
                container.Register<IEmailService, EmailService>();
                container.Register<IDropboxService, DropBoxService>();
                container.Register<IBillsBl, BillsBl>();
                container.Register<IUnitOfWork, UnitOfWork>().AsSingleton();
                container.Register<IUserDialogs>(UserDialogs.Instance);
                container.Register<Bl.IPlacesBl, Bl.PlacesBl>();
                container.Register<Bl.ITenantsBl, Bl.TenantsBl>();
                container.Register<Bl.IScheduleBl, Bl.ScheduleBl>();

                FreshIOC.OverrideContainer(container);

                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU5MTk4QDMxMzcyZTMzMmUzMGFvd0UvK2ZtUm5LSXYxRE9Rc3NvZnBOUFZqTHJqWkF2WVFKa1JVRENETFk9");

                var mainPage = new FreshTabbedNavigationContainer();

                //  mainPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                //mainPage.UnselectedTabColor = Color.Black;
                //mainPage.SelectedTabColor = Color.DodgerBlue;

                mainPage.AddTab<UpcomingRentPageModel>("Rents", null);
                mainPage.AddTab<UpcomingBillsPageModel>("Bills", null);
                mainPage.AddTab<PropertiesPageModel>("Places", null);
                mainPage.AddTab<SettingsPageModel>("Setting", null);

                Xamarin.Forms.Application.Current.MainPage = mainPage;
                GC.Collect();
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected override void OnStart()
        {
            //AppCenter.Start("android=5060fb64-b8e8-468f-8b30-5e0153e35200;" +
            //       "ios=015a7051-88a2-4ea9-8ef5-3224acfdfd2b",
            //       typeof(Analytics), typeof(Crashes));
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
