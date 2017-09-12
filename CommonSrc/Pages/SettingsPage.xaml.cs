using System;
using System.Collections.Generic;
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

			this.Username.Text = Settings.UserName;
            this.Password.Text = Settings.Password;
            this.UnsolicitedMessages.IsToggled = Settings.IgnoreUnsolicitedMessages;
            this.IgnoreVKs.IsToggled = Settings.IgnoreVKs;
            this.IgnoreGifts.IsToggled = Settings.IgnoreGifts;

            Regions regions = new Regions();

            Items = new SelectableObservableCollection<Region>(regions.AvailableRegions);

            int[] selectedRegions = Settings.SelectedRegions;

            foreach( var item in Items ) {
                foreach( var regId in selectedRegions ) {
                    if ( regId == item.Data.Id ) {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
            BindingContext = this;
		}

        protected override void OnDisappearing()
        {
            Settings.UserName = this.Username.Text;
            Settings.Password = this.Password.Text;
            Settings.IgnoreUnsolicitedMessages = this.UnsolicitedMessages.IsToggled;
            Settings.IgnoreVKs = this.IgnoreVKs.IsToggled;
            Settings.IgnoreGifts = this.IgnoreGifts.IsToggled;

            Settings.ClearRegions();
            foreach (var item in Items)
            {
                if (item.IsSelected)
                {
                    Settings.AddRegion(item.Data.Id);
                }
            }
        }

		public SelectableObservableCollection<Region> Items { get; }
		public bool EnableMultiSelect { get { return true; } }

	}
}
