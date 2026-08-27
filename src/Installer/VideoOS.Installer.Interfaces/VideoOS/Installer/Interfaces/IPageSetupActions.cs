using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces;

public interface IPageSetupActions
{
	string PageName { get; }

	bool BeforeActivatePage(Dictionary<string, string> parameters);

	bool BeforeLeavePage(Dictionary<string, string> parameters);

	Dictionary<string, string> CollectParameters();
}
