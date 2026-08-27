using System;
using System.Windows.Controls;

namespace VideoOS.LPR.Common.UI.Wizard;

public abstract class WizardPage : UserControl
{
	private bool _enableForwardNavigation;

	private bool _enableBackwardNavigation;

	private bool _enableWizardCancellation = true;

	private bool _enableWizardFinish;

	private bool _showForwardNavigation = true;

	private bool _showBackwardNavigation = true;

	private bool _showWizardCancellation = true;

	private bool _showWizardFinish;

	public bool EnableForwardNavigation
	{
		get
		{
			return _enableForwardNavigation;
		}
		set
		{
			if (_enableForwardNavigation != value)
			{
				_enableForwardNavigation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool EnableBackwardNavigation
	{
		get
		{
			return _enableBackwardNavigation;
		}
		set
		{
			if (_enableBackwardNavigation != value)
			{
				_enableBackwardNavigation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool EnableWizardCancellation
	{
		get
		{
			return _enableWizardCancellation;
		}
		set
		{
			if (_enableWizardCancellation != value)
			{
				_enableWizardCancellation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool EnableWizardFinish
	{
		get
		{
			return _enableWizardFinish;
		}
		set
		{
			if (_enableWizardFinish != value)
			{
				_enableWizardFinish = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool ShowForwardNavigation
	{
		get
		{
			return _showForwardNavigation;
		}
		set
		{
			if (_showForwardNavigation != value)
			{
				_showForwardNavigation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool ShowBackwardNavigation
	{
		get
		{
			return _showBackwardNavigation;
		}
		set
		{
			if (_showBackwardNavigation != value)
			{
				_showBackwardNavigation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool ShowWizardCancellation
	{
		get
		{
			return _showWizardCancellation;
		}
		set
		{
			if (_showWizardCancellation != value)
			{
				_showWizardCancellation = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public bool ShowWizardFinish
	{
		get
		{
			return _showWizardFinish;
		}
		set
		{
			if (_showWizardFinish != value)
			{
				_showWizardFinish = value;
				this.ShowOrEnableNavigationChanged(this, new EventArgs());
			}
		}
	}

	public event EventHandler ShowOrEnableNavigationChanged = delegate
	{
	};

	public event EventHandler<NavigationEventArgs> NavigationRequested = delegate
	{
	};

	public void RequestNavigation(NavigationType navigationType)
	{
		this.NavigationRequested(this, new NavigationEventArgs
		{
			NavigationType = navigationType
		});
	}

	public abstract bool ApproveNavigation(NavigationType navigationType);

	public abstract void PageShown(NavigationType navigationType);

	public abstract void PageHided(NavigationType navigationType);
}
