using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.UI.Framework;

[CLSCompliant(true)]
public abstract class PageSetupActionsBase : IPageSetupActions
{
	public abstract string PageName { get; }

	public virtual bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		return true;
	}

	public virtual bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		return true;
	}

	public virtual Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string>();
	}
}
