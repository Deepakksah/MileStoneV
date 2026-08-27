using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VideoOS.LPR.Common.UI.Resources;

namespace VideoOS.LPR.Common.UI.Wizard;

public abstract class WizardHandler
{
	private readonly string _title;

	private WizardWindow _wizardWindow;

	private Size _windowSize;

	public WizardResult WizardResult { get; public set; }

	public ReadOnlyCollection<WizardPage> WizardPages { get; private set; }

	protected WizardHandler(string title, Size windowSize)
		: this(title)
	{
		_windowSize = windowSize;
	}

	protected WizardHandler(string title)
	{
		_title = title;
		_windowSize = new Size(800.0, 600.0);
		WizardResult = WizardResult.InProgress;
	}

	public void ShowWizard()
	{
		_wizardWindow = new WizardWindow
		{
			Title = _title,
			Height = _windowSize.Height,
			Width = _windowSize.Width
		};
		_wizardWindow.ShowWizard(this);
	}

	protected void SetWizardPages(IList<WizardPage> wizardPages)
	{
		if (wizardPages == null || !wizardPages.Any())
		{
			throw new ArgumentException("The argument wizardPages must contain at least one page.");
		}
		WizardPages = new ReadOnlyCollection<WizardPage>(wizardPages);
	}

	public virtual bool SkipPageInBackwardNavigation(WizardPage wizardPage)
	{
		return false;
	}

	public abstract void PageChanged(WizardPage oldWizardPage, WizardPage newWizardPage);

	public virtual string ForwardNavigationName(WizardPage currentPage)
	{
		return Strings.WizardWindow_ButtonNext;
	}

	public virtual string FinishButtonName(WizardPage wizardPage)
	{
		return Strings.WizardWindow_ButtonFinish;
	}
}
