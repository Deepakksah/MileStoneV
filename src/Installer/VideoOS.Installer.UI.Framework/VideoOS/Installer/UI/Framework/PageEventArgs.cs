using System;

namespace VideoOS.Installer.UI.Framework;

public class PageEventArgs : EventArgs
{
	private PageCollection _pages;

	public virtual WizardPage Page
	{
		get
		{
			if (PageIndex >= 0 && PageIndex < _pages.Count)
			{
				return _pages[PageIndex];
			}
			return null;
		}
		set
		{
			if (_pages.Contains(value))
			{
				PageIndex = _pages.IndexOf(value);
				return;
			}
			throw new ArgumentOutOfRangeException("NextPage", value, "The page you tried to set was not found in the wizard.");
		}
	}

	public int PageIndex { get; private set; }

	public PageEventArgs(int index, PageCollection pages)
	{
		PageIndex = index;
		_pages = pages;
	}
}
