using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IE.CommonSrc.IEIntegration;
using Xamarin.Forms;

namespace IE.CommonSrc.Pages
{
    public partial class NewMemberListPage : ContentPage
    {
        public NewMemberListPage()
        {
            InitializeComponent();

			IEDataSource ds = IEDataSource.GetInstance();

            //List<IEMember> members = ds.Members.Select(member => member.Value).Where(member => member.Status == IEMember.MEMBER_NEW).ToList();

            Items = ds.NewItems;
			this.Title = "New Members (" + Items.Count() + ")";
            Items.CollectionChanged += (sender, e) => {
                this.Title = "New Members (" + Items.Count() + ")";
            };
			BindingContext = this;
		}

	    public async void OnSelected(object sender, EventArgs e) 
        {
            if ( MemberListView.SelectedItem != null) 
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

		public async void OnReject(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);

            IEMember member = mi.CommandParameter as IEMember;
            if ( member != null ) 
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
			}
			return true;
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
