using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

			this.LogoImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(GotoActiveMembersPage),
				NumberOfTapsRequired = 1
			});


			/*
			this.FavsImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(GotoNewMembersPage),
				NumberOfTapsRequired = 1
			});
			*/

			this.SettingsImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(GotoSettingsPage),
				NumberOfTapsRequired = 1
			});
			this.UsersImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
                Command = new Command(GotoNewMembersPage),
				NumberOfTapsRequired = 1
			});
			this.RejectedImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
                Command = new Command(GotoRejctedMembersPage),
				NumberOfTapsRequired = 1
			});
		}

		protected override void OnAppearing()
		{

			if (firstTimeIn)
			{
				firstTimeIn = false;
				bool needSettings = (false || string.IsNullOrEmpty(Settings.UserName)) || string.IsNullOrEmpty(Settings.Password);
				if (needSettings)
				{
					DisplayAlert("Alert", "Please enter a username/password", "OK");
					GotoSettingsPage();
				}
			}
		}

		async void GotoActiveMembersPage()
		{
            await Navigation.PushAsync(new ActiveMembersListPage());
		}

		async void GotoRejctedMembersPage()
		{
            await Navigation.PushAsync(new RejectedMembersListPage());
		}

		async void GotoSettingsPage()
		{
			await Navigation.PushAsync(new SettingsPage());
		}

		async void GotoNewMembersPage()
		{
            await Navigation.PushAsync(new NewMemberListPage());
		}
	}
}
