using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.XForms.iOS.EffectsView;
using UIKit;

namespace TenantsApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("Visual_Experimental");
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();

            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Cards.SfCardViewRenderer.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.SfNumericUpDown.XForms.iOS.SfNumericUpDownRenderer.Init();
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfCheckBoxRenderer.Init();
            new SfNumericTextBoxRenderer();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
