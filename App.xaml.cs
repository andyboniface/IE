using IE.CommonSrc.IEIntegration;
using IE.CommonSrc.Pages;
using IE.Helpers;
using Xamarin.Forms;

namespace IE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Settings.UserName = "kiss me slowly";
            Settings.Password = "twenty1%fat";
            Settings.SearchForFemales = true;
            Settings.ClearRegions();
            Settings.AddRegion(33);
            Settings.AddRegion(50);

        }

        protected override void OnStart()
        {
			// Handle when your app starts
			DesignTimeHelper.TurnOffDesignMode();

			IEDataSource ds = IEDataSource.GetInstance();
			if (ds.Running == false)
			{
				ds.StartDataSource();
			}

			MainPage = new NavigationPage(new ActiveMembersListPage());
		}

        protected override void OnSleep()
        {
			// Handle when your app sleeps
			IEDataSource ds = IEDataSource.GetInstance();
            ds.StopDataSource();
		}

        protected override void OnResume()
        {
			// Handle when your app resumes
			IEDataSource ds = IEDataSource.GetInstance();
			ds.StartDataSource();
		}
    }
}
