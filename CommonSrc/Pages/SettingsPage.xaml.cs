using System;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.Controls;
using IE.Helpers;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            this.Title = "Settings";

            this.ChangeRegionsButton.Clicked += (sender, e) => {
                GotoRegionsPage();
            };
			this.PollingRate.ValueChanged += (sender, e) => 
            {
                this.PollingRateLabel.Text = String.Format("Polling Rate {0:F0} minutes", e.NewValue);    
            };
		}

		async void GotoRegionsPage()
		{
            await Navigation.PushAsync(new RegionSelectionPage());
		}

        protected override void OnAppearing() 
        {
			this.Username.Text = Settings.UserName;
			this.Password.Text = Settings.Password;
			this.UnsolicitedMessages.IsToggled = Settings.IgnoreUnsolicitedMessages;
			this.IgnoreVKs.IsToggled = Settings.IgnoreVKs;
			this.IgnoreGifts.IsToggled = Settings.IgnoreGifts;
			this.SearchFemales.IsToggled = Settings.SearchForFemales;
  			this.PollingRate.Value = Settings.PollingRate;
			this.PollingRateLabel.Text = String.Format("Polling Rate {0:F0} minutes", Settings.PollingRate);

			Regions regions = new Regions();

			if (Settings.SelectedRegions.Length > 0)
			{
				string regionList = "";
				foreach (int regionId in Settings.SelectedRegions)
				{
					if (regionList.Length > 0)
					{
						regionList = regionList + ", ";
					}
					regionList = regionList + regions.CountyById(regionId);
				}
				this.CurrentRegions.Text = "Regions: " + regionList;
			}
			else
			{
				this.CurrentRegions.Text = "Regions: None selected";
			}
		}

		protected override void OnDisappearing()
        {
            Settings.UserName = this.Username.Text;
            Settings.Password = this.Password.Text;
            Settings.IgnoreUnsolicitedMessages = this.UnsolicitedMessages.IsToggled;
            Settings.IgnoreVKs = this.IgnoreVKs.IsToggled;
            Settings.IgnoreGifts = this.IgnoreGifts.IsToggled;
            Settings.PollingRate = (int )this.PollingRate.Value;
			Settings.SearchForFemales = this.SearchFemales.IsToggled;
        }
	}
}
