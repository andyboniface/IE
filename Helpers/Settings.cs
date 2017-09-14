// Helpers/Settings.cs
using System;
using System.Collections.Generic;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace IE.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
        private static ISettings AppSettings => CrossSettings.Current;

		public static string UserName
		{
			get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
			set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
		}

		public static string Password
		{
			get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
			set => AppSettings.AddOrUpdateValue(nameof(Password), value);
		}

        public static int[] SelectedRegions
        {
            get 
            {
                List<int> selectedRegions = new List<int>();

                if (! String.IsNullOrEmpty(Regions))
                {
                    string[] ids = Regions.Split(',');
                    if (ids != null)
                    {
                        foreach (string id in ids)
                        {
                            selectedRegions.Add(Convert.ToInt32(id));
                        }
                    }
                }
                return selectedRegions.ToArray();
            }
        }

        public static void ClearRegions() {
            Regions = String.Empty;
        }

        public static void AddRegion(int id) {
            if ( String.IsNullOrEmpty(Regions)) {
                Regions = "" + id;
            }
            else
            {
                Regions = Regions + "," + id;
            }
        }

		public static int PollingRate
		{
			get => AppSettings.GetValueOrDefault(nameof(PollingRate), 10);
			set => AppSettings.AddOrUpdateValue(nameof(PollingRate), value);
		}

		public static string Regions
		{
			get => AppSettings.GetValueOrDefault(nameof(Regions), string.Empty);
			set => AppSettings.AddOrUpdateValue(nameof(Regions), value);
		}

		public static int MinAge
		{
			get => AppSettings.GetValueOrDefault(nameof(MinAge), 21);
			set => AppSettings.AddOrUpdateValue(nameof(MinAge), value);
		}

		public static int MaxAge
		{
			get => AppSettings.GetValueOrDefault(nameof(MaxAge), 99);
			set => AppSettings.AddOrUpdateValue(nameof(MaxAge), value);
		}

		public static bool IgnoreUnsolicitedMessages
		{
			get => AppSettings.GetValueOrDefault(nameof(IgnoreUnsolicitedMessages), false);
			set => AppSettings.AddOrUpdateValue(nameof(IgnoreUnsolicitedMessages), value);
		}

		public static bool SearchForFemales
		{
			get => AppSettings.GetValueOrDefault(nameof(SearchForFemales), false);
			set => AppSettings.AddOrUpdateValue(nameof(SearchForFemales), value);
		}

		public static bool IgnoreVKs
		{
			get => AppSettings.GetValueOrDefault(nameof(IgnoreVKs), false);
			set => AppSettings.AddOrUpdateValue(nameof(IgnoreVKs), value);
		}

        public static bool IgnoreGifts
		{
			get => AppSettings.GetValueOrDefault(nameof(IgnoreGifts), false);
			set => AppSettings.AddOrUpdateValue(nameof(IgnoreGifts), value);
		}

	}
}