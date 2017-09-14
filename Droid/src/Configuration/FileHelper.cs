using System;
using System.IO;
using IE.CommonSrc.Configuration;
using Xamarin.Forms;

[assembly: Dependency(typeof(IFileHelper))]
namespace IE.Droid.src.Configuration
{
    public class FileHelper : IFileHelper
    {
		public string GetLocalFilePath(string filename)
		{
            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(path, filename);
		}
	}
}
