using System;

namespace VideoOS.Common.Help.UI;

public interface IHelpDisplayer
{
	void DisplayHtmlHelp(bool isOnline, Uri uri);
}
