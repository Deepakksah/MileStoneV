using System;

namespace VideoOS.Common.Help.UI;

public interface IHelpUriChecker
{
	bool FileExists(Uri strOfflineUrl);

	void CheckUriAccess(string strUrl);
}
