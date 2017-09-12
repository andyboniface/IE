using IE.CommonSrc.Pages;
using IE.Helpers;
using Xamarin.Forms;

namespace IE
{
    public partial class IEPage : ContentPage
    {
        private bool firstTimeIn = true;

        public IEPage()
        {
            InitializeComponent();
            this.Title = "IEPage";
            NavigationPage.SetHasNavigationBar(this, false );

            this.logo.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    //this.logo.IsVisible = false;
                    GotoSettingsPage();
                }),
                NumberOfTapsRequired = 1
            });
        }

        protected override void OnAppearing() {

            if (firstTimeIn)
            {
                firstTimeIn = false;
                bool needSettings = false;

                if (string.IsNullOrEmpty(Settings.UserName))
                {
                    needSettings = true;
                }
                if (string.IsNullOrEmpty(Settings.Password))
                {
                    needSettings = true;
                }
                if (needSettings)
                {
                    DisplayAlert( "Alert", "Please enter a username/password", "OK");
                    GotoSettingsPage();
                }
            }
		}

        async void GotoSettingsPage()
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
