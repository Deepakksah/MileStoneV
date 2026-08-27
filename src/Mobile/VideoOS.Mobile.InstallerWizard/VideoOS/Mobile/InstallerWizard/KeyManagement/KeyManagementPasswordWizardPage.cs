using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Mobile.KeyManagement;
using VideoOS.Mobile.UI.Mobile.Controls.Password;
using VideoOS.Utilities.Security;

namespace VideoOS.Mobile.InstallerWizard.KeyManagement;

public class KeyManagementPasswordWizardPage : WizardPageBase
{
	private ElementHost _elementHost;

	private KeyManagementPasswordControl _wpfControl;

	private KeyManagementPasswordViewModel _viewModel;

	private IContainer components;

	public KeyManagementPasswordWizardPage()
		: this(new KeyManagementPasswordViewModel())
	{
	}

	public KeyManagementPasswordWizardPage(KeyManagementPasswordViewModel viewModel)
	{
		InitializeComponent();
		PageName = "MobileServerKeyManagementPasswordPage";
		_elementHost = new ElementHost
		{
			Dock = DockStyle.Fill
		};
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new KeyManagementPasswordControl();
		_viewModel = viewModel;
		_viewModel.HeaderText = Resources.KeyManagementPasswordPageHeader;
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		_wpfControl.Margin = new Thickness(52.0, 45.0, 50.0, 45.0);
		_viewModel.ReloadResources();
		_viewModel.PropertyChanged += delegate(object s, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Password" || e.PropertyName == "DoNotUsePassword")
			{
				SetNextButtonEnabledState();
			}
		};
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		_viewModel.HeaderText = Resources.KeyManagementPasswordPageHeader;
		_viewModel.PageInfoText = Resources.KeyManagementPasswordPageInfo;
		_viewModel.PasswordLabelText = Resources.KeyManagementPassword;
		_viewModel.ConfirmPasswordLabelText = Resources.KeyManagementConfirmPassword;
		_viewModel.MessageDialogText = Resources.KeyManagementPasswordPageHeader;
		_viewModel.ReloadResources();
	}

	public override bool OnBeforeActivate(Dictionary<string, string> parameters)
	{
		if (PasswordManager.Instance.HasPassword())
		{
			return false;
		}
		return base.OnBeforeActivate(parameters);
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		SetNextButtonEnabledState();
	}

	public override bool OnBeforeContinue()
	{
		if (!_viewModel.Validate())
		{
			return false;
		}
		return base.OnBeforeContinue();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		string password = _viewModel.Password.DecryptSecureString();
		password = EncryptHelper.EncryptPassword(password, unicodePassword: true);
		return new Dictionary<string, string> { ["KEYMANAGEMENTPASSWORD"] = password };
	}

	private void SetNextButtonEnabledState()
	{
		Wizard.NextEnabled = _viewModel.DoNotUsePassword || _viewModel.Password.Length > 0;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_elementHost != null)
			{
				if (_elementHost.Child != null)
				{
					_elementHost.Child = null;
				}
				_elementHost.Dispose();
				_elementHost = null;
			}
			if (components != null)
			{
				components.Dispose();
				components = null;
			}
			if (_viewModel != null)
			{
				_viewModel.Dispose();
				_viewModel = null;
			}
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Mobile.InstallerWizard.KeyManagement.KeyManagementPasswordWizardPage));
		base.WizardPage = new VideoOS.Installer.UI.Framework.WizardPage();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("70D9BB5C-AB2F-4560-BCCC-37806EBADE0E");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "KeyManagementPasswordWizardPage";
		base.ResumeLayout(false);
	}
}
