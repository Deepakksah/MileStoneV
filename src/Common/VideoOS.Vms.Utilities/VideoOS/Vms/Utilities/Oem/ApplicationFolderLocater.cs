using System;
using System.IO;
using System.Reflection;

namespace VideoOS.Vms.Utilities.Oem;

public class ApplicationFolderLocater : IApplicationFolderLocater
{
	public string GetApplicationFolder(string resourceFileName)
	{
		string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		if (!File.Exists(Path.Combine(text, resourceFileName)))
		{
			text = AppDomain.CurrentDomain.BaseDirectory;
			if (AppDomain.CurrentDomain.RelativeSearchPath != null && AppDomain.CurrentDomain.RelativeSearchPath.Length > 0)
			{
				text = ((!Path.IsPathRooted(AppDomain.CurrentDomain.RelativeSearchPath)) ? new DirectoryInfo(Path.Combine(text, AppDomain.CurrentDomain.RelativeSearchPath)).FullName : AppDomain.CurrentDomain.RelativeSearchPath);
			}
		}
		return text;
	}
}
