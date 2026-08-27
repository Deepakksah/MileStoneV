using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicenseWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private LicensePageControl _wpfControl = new LicensePageControl();

	private LicensePageViewModel _viewModel;

	private string _licenseFile = "";

	private IContainer components;

	public LicenseWizard()
	{
		InitializeComponent();
		PageName = "LicensePage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_viewModel = new LicensePageViewModel();
		_viewModel.PropertyChanged += _viewModel_PropertyChanged;
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
	}

	private void _viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName.Equals("LicensePath") || e.PropertyName.Equals("NotDownloadingFreeLicense"))
		{
			SetWizardButtonEnabled();
		}
	}

	private void SetWizardButtonEnabled()
	{
		Wizard.NextEnabled = !string.IsNullOrEmpty(_viewModel.LicensePath);
		Wizard.BackEnabled = true;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("LICENSE", _licenseFile);
		GlobalData.SetValue("LICENSE", dictionary["LICENSE"], "WizardPageWelcom.CollectParameters()");
		return dictionary;
	}

	public override void OnBeforeCancel()
	{
		_viewModel.Cancel();
		base.OnBeforeCancel();
	}

	public override bool OnBeforeContinue()
	{
		try
		{
			if (!ExistingSystemHelper.CheckExistingSystems(allowTrialCheck: false))
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_ExistingSystemsExistCaption, message, null, null, enablePreviousButton: false);
			ErrorHandler.ShowError();
			LogManager.Write(message, TraceEventType.Error);
			return false;
		}
		try
		{
			InstallerManager.Instance.RunCheckLicense(_viewModel.LicensePath, InstallerManager.Instance.InstallerConfig.LicenseProvider.Type.ToString(), Wizard);
		}
		catch (LicenseCheckException ex2)
		{
			MessageHelper.ShowInstallerMessageBox(ex2.LicenseType, ex2.LicenseMessage, InstallerMessageBoxIcon.Exclamation);
			return false;
		}
		_licenseFile = _viewModel.LicensePath;
		return true;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		_viewModel.ReloadResources();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		SetWizardButtonEnabled();
		_viewModel.LookupLicenseAndSet();
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
				_viewModel.PropertyChanged -= _viewModel_PropertyChanged;
				_viewModel.Dispose();
				_viewModel = null;
			}
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("45984772-2c05-466c-adb8-fbca0067e62e");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "LicenseWizard";
		this.PageName = "LicenseWizardPage";
		base.ResumeLayout(false);
	}
}
