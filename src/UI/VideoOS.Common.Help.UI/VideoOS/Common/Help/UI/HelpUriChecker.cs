using System;
using System.IO;
using System.Net;

namespace VideoOS.Common.Help.UI;

public class HelpUriChecker : IHelpUriChecker
{
	public static readonly Lazy<IHelpUriChecker> Instance = new Lazy<IHelpUriChecker>(() => new HelpUriChecker());

	private HelpUriChecker()
	{
	}

	public bool FileExists(Uri strOfflineUrl)
	{
		return File.Exists(strOfflineUrl.LocalPath);
	}

	public void CheckUriAccess(string strUrl)
	{
		WebRequest webRequest = WebRequest.Create(strUrl);
		webRequest.Method = "HEAD";
		webRequest.Timeout = 3000;
		webRequest.GetResponse().Close();
	}
}
