using System;
using System.Collections.Generic;
using IE.Helpers;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class HomePage : ContentPage
    {
		private bool firstTimeIn = true;
		
        public HomePage()
        {
            InitializeComponent();
			this.Title = "IEPage";
			NavigationPage.SetHasNavigationBar(this, false);

			this.logo.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(() =>
				{
					//this.logo.IsVisible = false;
					GotoSettingsPage();
				}),
				NumberOfTapsRequired = 1
			});

            this.settings.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(() =>
				{
					//this.logo.IsVisible = false;
					GotoSettingsPage();
				}),
				NumberOfTapsRequired = 1
			});
		}

		protected override void OnAppearing()
		{

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
					DisplayAlert("Alert", "Please enter a username/password", "OK");
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
