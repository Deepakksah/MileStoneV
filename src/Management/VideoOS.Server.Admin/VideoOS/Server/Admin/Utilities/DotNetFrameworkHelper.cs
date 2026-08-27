using System;
using System.IO;

namespace VideoOS.Server.Admin.Utilities;

public sealed class DotNetFrameworkHelper
{
	public static string GetFramework40Path
	{
		get
		{
			string environmentVariable = Environment.GetEnvironmentVariable("windir");
			string path = "Microsoft.NET\\Framework\\v4.0.30319\\";
			string path2 = "Microsoft.NET\\Framework64\\v4.0.30319\\";
			string text = Path.Combine(environmentVariable, path2);
			string result = Path.Combine(environmentVariable, path);
			if (Environment.Is64BitOperatingSystem)
			{
				result = text;
			}
			return result;
		}
	}

	private DotNetFrameworkHelper()
	{
	}
}
