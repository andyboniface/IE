using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IE.CommonSrc.IEIntegration;
using System.Linq;
using Xamarin.Forms;
using System.ComponentModel;

namespace IE.CommonSrc.Pages
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();

			IEDataSource ds = IEDataSource.GetInstance();

			//List<IEMember> members = ds.Members.Select(member => member.Value).Where(member => member.Status == IEMember.MEMBER_NEW).ToList();

            Items = ds.SearchResults;
			this.Title = "Found (" + Items.Count() + ")";
			Items.CollectionChanged += (sender, e) => 
            {
                Device.BeginInvokeOnMainThread(() => 
                {
                    this.Title = "Found (" + Items.Count() + ")";
                    Items = ds.SearchResults;
                    this.OnPropertyChanging();
				});
			};
			BindingContext = this;

            this.SearchButton.Clicked += (sender, e) => {
                ds.DoNameSearch(this.SearchUsername.Text);
            };


		}

		//public event PropertyChangedEventHandler PropertyChanged;


		//public void FirePropertyChangeEvent()
		//{
        //    if (!= null)
	//		{
	//			BindableObject.BindingContextProperty(this, new PropertyChangedEventArgs(""));            // Empty string means everything has changed
	//		}
	//	}

		public async void OnSelected(object sender, EventArgs e)
		{
			if (MemberListView.SelectedItem != null)
			{
				IEMember member = MemberListView.SelectedItem as IEMember;
				//DisplayAlert("SelectedItem", MemberListView.SelectedItem.ToString(), "OK");
				await Navigation.PushAsync(ProfilePage.GetInstance(member));
				MemberListView.SelectedItem = null;
			}
		}

		public async void OnAccept(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			IEMember member = mi.CommandParameter as IEMember;
			if (member != null)
			{
				await AcceptMember(member);
			}
		}

		private async Task<bool> AcceptMember(IEMember member)
		{
			var ans = await DisplayAlert("Accepting", "Are you sure you want to accept " + member.Username + "?", "Accept", "Cancel");
			if (ans == true)
			{
				IEDataSource ds = IEDataSource.GetInstance();
				ds.ChangeMemberStatus(member, IEMember.MEMBER_KNOWN);
			}
			return true;
		}

		public ObservableCollection<IEMember> Items { get; private set; }

	}
}
