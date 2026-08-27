namespace VideoOS.Installer.UI.Framework;

public class AutoBackPathPageEventArgs : PageEventArgs
{
	private WizardPage _activePage;

	public override WizardPage Page
	{
		get
		{
			return base.Page;
		}
		set
		{
			if (value == _activePage)
			{
				base.Page = value;
				return;
			}
			throw new PageEventArgsException("Page property can only be set to active page");
		}
	}

	public AutoBackPathPageEventArgs(int index, WizardPage activePage, PageCollection pages)
		: base(index, pages)
	{
		_activePage = activePage;
	}
}
