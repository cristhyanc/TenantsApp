using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TenantsApp
{
    public class ExtendedContentPage : ContentPage
    {

        public bool IsTextBarWhite { get; set; }

        public static readonly BindableProperty PageAppearingCommandProperty =
        BindableProperty.Create(nameof(PageAppearingCommand), typeof(ICommand), typeof(ExtendedContentPage), null);

        public ICommand PageAppearingCommand
        {
            get { return (ICommand)GetValue(PageAppearingCommandProperty); }
            set { SetValue(PageAppearingCommandProperty, value); }
        }


        public static readonly BindableProperty PageDisappearingCommandProperty =
        BindableProperty.Create(nameof(PageDisappearingCommand), typeof(ICommand), typeof(ExtendedContentPage), null);

        public ICommand PageDisappearingCommand
        {
            get { return (ICommand)GetValue(PageDisappearingCommandProperty); }
            set { SetValue(PageDisappearingCommandProperty, value); }
        }


        public ExtendedContentPage()
        {
            this.Appearing += ExtendedContentPage_Appearing;
            this.Disappearing += ExtendedContentPage_Disappearing;

            this.SetBinding(ExtendedContentPage.PageAppearingCommandProperty, "PageAppearingCommand");
            this.SetBinding(ExtendedContentPage.PageDisappearingCommandProperty, "PageDisappearingCommand");
        }

        private void ExtendedContentPage_Disappearing(object sender, EventArgs e)
        {
            ExtendedContentPage page = (ExtendedContentPage)sender;
            Execute(page.PageDisappearingCommand);
        }

        private void ExtendedContentPage_Appearing(object sender, EventArgs e)
        {
            ExtendedContentPage page = (ExtendedContentPage)sender;
            Execute(page.PageAppearingCommand);
        }

        public static void Execute(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

    }
}
