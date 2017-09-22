using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IE.CommonSrc.IEIntegration;
using IE.Helpers;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class ActiveMembersListPage : ContentPage
    {
		private bool firstTimeIn = true;

		public ActiveMembersListPage()
        {
			InitializeComponent();
			this.Title = "Active Members";
			NavigationPage.SetHasNavigationBar(this, false);

			IEDataSource ds = IEDataSource.GetInstance();

            //List<IEMember> members = ds.Members.Select(member => member.Value).Where(member => (member.Status == IEMember.MEMBER_KNOWN || member.Status == IEMember.MEMBER_ACTIVE)).ToList();

            Items = ds.ActiveItems;
			BindingContext = this;

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
            this.SearchImage.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(GotoSearchPage),
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

		async void GotoRejctedMembersPage()
		{
			await Navigation.PushAsync(new RejectedMembersListPage());
		}

		async void GotoSearchPage()
		{
			await Navigation.PushAsync(new SearchPage());
		}

		async void GotoSettingsPage()
		{
			await Navigation.PushAsync(new SettingsPage());
		}

		async void GotoNewMembersPage()
		{
			await Navigation.PushAsync(new NewMemberListPage());
		}

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

		public async void OnReject(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);

			IEMember member = mi.CommandParameter as IEMember;
			if (member != null)
			{
				await RejectMember(member);
			}
		}

		private async Task<bool> RejectMember(IEMember member)
		{
			var ans = await DisplayAlert("Rejecting", "Are you sure you want to reject " + member.Username + "?", "Reject", "Cancel");
			if (ans == true)
			{
				IEDataSource ds = IEDataSource.GetInstance();
                ds.ChangeMemberStatus(member, IEMember.MEMBER_REJECTED);
				//member.Status = IEMember.MEMBER_REJECTED;
				//ds.SaveMember(member);
				//Items.Remove(member);
			}
			return true;
		}

		public ObservableCollection<IEMember> Items { get; private set; }

	}
}
