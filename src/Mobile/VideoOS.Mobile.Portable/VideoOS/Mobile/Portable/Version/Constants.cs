using System;

namespace VideoOS.Mobile.Portable.Version;

public class Constants
{
	public const string ServerMajor = "25";

	public const string ServerMinor = "3";

	public const string ServerBuild = "17737";

	public const string ServerRevision = "1";

	public static readonly string ServerVersion = GetVersionString("25", "3", "17737", "1");

	public static string GetVersionString(string major, string minor, string build, string revision, int revisionStartNumber = 1)
	{
		char c = 'a';
		try
		{
			c = (char)(97 - revisionStartNumber + Convert.ToInt32(revision));
		}
		catch (Exception)
		{
		}
		return string.Format("{0}.{1}{3} ({2})", major, minor, build, c);
	}
}
