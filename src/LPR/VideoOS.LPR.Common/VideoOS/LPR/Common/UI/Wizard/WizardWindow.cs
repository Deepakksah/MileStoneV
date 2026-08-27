using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.UI.Wizard;

public partial class WizardWindow : Window, IComponentConnector
{
	private WizardHandler _wizardHandler;

	public WizardWindow()
	{
		ClientControl.Instance.RegisterUIControlForAutoTheming(this);
		InitializeComponent();
	}

	public bool ShowWizard(WizardHandler wizardHandler)
	{
		_wizardHandler = wizardHandler;
		_wizardHandler.WizardResult = WizardResult.InProgress;
		AddOwnerToModalWindow(this);
		ShowDialog();
		return true;
	}

	private void WizardWindow_OnLoaded(object sender, RoutedEventArgs e)
	{
		NavigateForward();
	}

	private void WizardWindow_OnClosing(object sender, CancelEventArgs e)
	{
		if (_wizardHandler.WizardResult != WizardResult.Completed && _wizardPageGrid.Children.Count > 0)
		{
			WizardPage wizardPage = (WizardPage)_wizardPageGrid.Children[0];
			if (!wizardPage.EnableWizardCancellation || !wizardPage.ApproveNavigation(NavigationType.WizardCancellation))
			{
				e.Cancel = true;
			}
		}
	}

	private void WizardWindow_OnClosed(object sender, EventArgs e)
	{
		if (_wizardHandler.WizardResult == WizardResult.InProgress)
		{
			_wizardHandler.WizardResult = WizardResult.Canceled;
			ShowPage(null, NavigationType.WizardCancellation);
		}
		else
		{
			ShowPage(null, NavigationType.ForwardNavigation);
		}
	}

	private void _finishButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (_wizardPageGrid.Children.Count <= 0 || ((WizardPage)_wizardPageGrid.Children[0]).ApproveNavigation(NavigationType.ForwardNavigation))
		{
			_wizardHandler.WizardResult = WizardResult.Completed;
			Close();
		}
	}

	private void _cancelButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (_wizardPageGrid.Children.Count <= 0 || ((WizardPage)_wizardPageGrid.Children[0]).ApproveNavigation(NavigationType.WizardCancellation))
		{
			Close();
		}
	}

	private void _nextButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (_wizardPageGrid.Children.Count <= 0 || ((WizardPage)_wizardPageGrid.Children[0]).ApproveNavigation(NavigationType.ForwardNavigation))
		{
			NavigateForward();
		}
	}

	private void _previousButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (_wizardPageGrid.Children.Count <= 0 || ((WizardPage)_wizardPageGrid.Children[0]).ApproveNavigation(NavigationType.BackwardNavigation))
		{
			NavigateBackward();
		}
	}

	private void wizardPage_ShowOrEnableNavigationChanged(object sender, EventArgs e)
	{
		UpdateButtons();
	}

	private void wizardPage_NavigationRequested(object sender, NavigationEventArgs e)
	{
		switch (e.NavigationType)
		{
		case NavigationType.ForwardNavigation:
			_nextButton_OnClick(this, null);
			break;
		case NavigationType.BackwardNavigation:
			_previousButton_OnClick(this, null);
			break;
		case NavigationType.WizardCancellation:
			_cancelButton_OnClick(this, null);
			break;
		}
	}

	private void NavigateBackward()
	{
		if (_wizardPageGrid.Children.Count > 0)
		{
			WizardPage value = (WizardPage)_wizardPageGrid.Children[0];
			if (_wizardHandler.WizardPages.IndexOf(value) > 0)
			{
				WizardPage wizardPage = _wizardHandler.WizardPages[_wizardHandler.WizardPages.IndexOf(value) - 1];
				while (_wizardHandler.SkipPageInBackwardNavigation(wizardPage) && _wizardHandler.WizardPages.IndexOf(value) > 0)
				{
					wizardPage = _wizardHandler.WizardPages[_wizardHandler.WizardPages.IndexOf(wizardPage) - 1];
				}
				ShowPage(wizardPage, NavigationType.BackwardNavigation);
			}
		}
		else
		{
			ShowPage(_wizardHandler.WizardPages[0], NavigationType.BackwardNavigation);
		}
	}

	private void NavigateForward()
	{
		if (_wizardPageGrid.Children.Count > 0)
		{
			WizardPage value = (WizardPage)_wizardPageGrid.Children[0];
			if (_wizardHandler.WizardPages.IndexOf(value) < _wizardHandler.WizardPages.Count - 1)
			{
				ShowPage(_wizardHandler.WizardPages[_wizardHandler.WizardPages.IndexOf(value) + 1], NavigationType.ForwardNavigation);
			}
		}
		else
		{
			ShowPage(_wizardHandler.WizardPages[0], NavigationType.ForwardNavigation);
		}
	}

	private void ShowPage(WizardPage wizardPage, NavigationType navigationType)
	{
		WizardPage wizardPage2 = null;
		if (_wizardPageGrid.Children.Count > 0)
		{
			wizardPage2 = (WizardPage)_wizardPageGrid.Children[0];
			wizardPage2.ShowOrEnableNavigationChanged -= wizardPage_ShowOrEnableNavigationChanged;
			wizardPage2.NavigationRequested -= wizardPage_NavigationRequested;
			wizardPage2.PageHided(navigationType);
			_wizardPageGrid.Children.Clear();
		}
		_wizardHandler.PageChanged(wizardPage2, wizardPage);
		if (wizardPage != null)
		{
			_wizardPageGrid.Children.Add(wizardPage);
			wizardPage.PageShown(navigationType);
			wizardPage.ShowOrEnableNavigationChanged += wizardPage_ShowOrEnableNavigationChanged;
			wizardPage.NavigationRequested += wizardPage_NavigationRequested;
		}
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		if (_wizardPageGrid.Children.Count == 0)
		{
			UpdateButtonState(_nextButton, visible: true, enabled: false);
			UpdateButtonState(_previousButton, visible: true, enabled: false);
			UpdateButtonState(_finishButton, visible: true, enabled: true);
			UpdateButtonState(_cancelButton, visible: true, enabled: true);
			return;
		}
		WizardPage wizardPage = (WizardPage)_wizardPageGrid.Children[0];
		_nextButton.Content = _wizardHandler.ForwardNavigationName(wizardPage);
		_finishButton.Content = _wizardHandler.FinishButtonName(wizardPage);
		if (_wizardHandler.WizardPages.IndexOf(wizardPage) > 0)
		{
			UpdateButtonState(_previousButton, wizardPage.ShowBackwardNavigation, wizardPage.EnableBackwardNavigation);
		}
		else
		{
			UpdateButtonState(_previousButton, visible: false, enabled: false);
		}
		if (_wizardHandler.WizardPages.IndexOf(wizardPage) < _wizardHandler.WizardPages.Count - 1)
		{
			UpdateButtonState(_nextButton, wizardPage.ShowForwardNavigation, wizardPage.EnableForwardNavigation);
		}
		else
		{
			UpdateButtonState(_nextButton, visible: false, enabled: false);
		}
		if (wizardPage.ShowWizardFinish)
		{
			UpdateButtonState(_finishButton, wizardPage.ShowWizardFinish, wizardPage.EnableWizardFinish);
		}
		else if (_wizardHandler.WizardPages.IndexOf(wizardPage) == _wizardHandler.WizardPages.Count - 1)
		{
			UpdateButtonState(_finishButton, wizardPage.ShowForwardNavigation, wizardPage.EnableForwardNavigation);
		}
		else
		{
			UpdateButtonState(_finishButton, visible: false, enabled: false);
		}
		UpdateButtonState(_cancelButton, wizardPage.ShowWizardCancellation, wizardPage.EnableWizardCancellation);
	}

	private static void UpdateButtonState(System.Windows.Controls.Button button, bool visible, bool enabled)
	{
		button.Visibility = ((!visible) ? Visibility.Collapsed : Visibility.Visible);
		button.IsEnabled = enabled;
	}

	private static void AddOwnerToModalWindow(Window window)
	{
		if (System.Windows.Forms.Application.OpenForms != null && System.Windows.Forms.Application.OpenForms.Count > 0)
		{
			new WindowInteropHelper(window).Owner = System.Windows.Forms.Application.OpenForms[0].Handle;
		}
	}
}
