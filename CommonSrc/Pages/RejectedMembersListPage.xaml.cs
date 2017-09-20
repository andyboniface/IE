using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IE.CommonSrc.IEIntegration;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class RejectedMembersListPage : ContentPage
    {
        public RejectedMembersListPage()
        {
			InitializeComponent();

			IEDataSource ds = IEDataSource.GetInstance();

            Items = ds.RejectedItems;
			this.Title = "Rejected Members (" + Items.Count() + ")";
			Items.CollectionChanged += (sender, e) => {
				this.Title = "Rejected Members (" + Items.Count() + ")";
			};
			BindingContext = this;
		}

		public async void OnSelected(object sender, EventArgs e)
		{
			if (MemberListView.SelectedItem != null)
			{
				IEMember member = MemberListView.SelectedItem as IEMember;
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
                ds.ChangeMemberStatus(member,IEMember.MEMBER_KNOWN);
			}
			return true;
		}

		public ObservableCollection<IEMember> Items { get; private set; }

	}
}
