﻿using System;
using System.Collections.Generic;
using IE.CommonSrc.Configuration;
using IE.CommonSrc.Controls;
using IE.Helpers;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class RegionSelectionPage : ContentPage
    {
        public RegionSelectionPage()
        {
            InitializeComponent();
			this.Title = "Change Regions";

			Regions regions = new Regions();

			Items = new SelectableObservableCollection<Region>(regions.AvailableRegions);
            List<Region> itemsToMove = new List<Region>();

            int[] selectedRegions = Settings.SelectedRegions;

			foreach (var item in Items)
			{
				foreach (var regId in selectedRegions)
				{
					if (regId == item.Data.Id)
					{
						item.IsSelected = true;
                        itemsToMove.Add(item.Data);
						break;
					}
				}
			}
            int newIndex = 0;
            // Move our selected items to the top of the list
            foreach( var item in itemsToMove ) {
                int origIdx = Items.IndexOf(item);
                Items.Move(origIdx, newIndex++);
            }
			BindingContext = this;

		}

		protected override void OnDisappearing()
		{
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
