using System;
namespace IE.Helpers
{
	public static class DesignTimeHelper
	{
		public static bool DesignModeOn { get; private set; }
#if !RELEASE
		  = true; //design mode is by default false in Release, just to be sure :-)
#endif

		public static void TurnOffDesignMode()
		{
			DesignModeOn = false;
		}
	}
}
