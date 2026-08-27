using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Security;

namespace VideoOS.Installer.DatabasePlugin;

public class KeyManagementPasswordWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private KeyManagementPasswordControl _wpfControl = new KeyManagementPasswordControl();

	private KeyManagementPasswordViewModel _viewModel;

	private IEncryptionKeyReader _encryptionKeyReader;

	private IGlobalData _globalData;

	private IPasswordValidator _passwordValidator;

	private IContainer components;

	public IEncryptionKeyReader EncryptionKeyReader
	{
		get
		{
			return _encryptionKeyReader ?? (_encryptionKeyReader = KeyManagementPasswordHelper.CreateKeyManager(LogManager.InnerLog));
		}
		public set
		{
			_encryptionKeyReader = value;
		}
	}

	public IGlobalData GlobalData
	{
		get
		{
			return _globalData ?? (_globalData = new GlobalDataWrapper());
		}
		public set
		{
			_globalData = value;
		}
	}

	public IPasswordValidator PasswordValidator
	{
		get
		{
			if (_passwordValidator == null)
			{
				_passwordValidator = new PasswordValidator();
			}
			return _passwordValidator;
		}
		set
		{
			_passwordValidator = value;
		}
	}

	public KeyManagementPasswordWizard()
		: this(new KeyManagementPasswordViewModel())
	{
	}

	public KeyManagementPasswordWizard(KeyManagementPasswordViewModel viewModel)
	{
		InitializeComponent();
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new KeyManagementPasswordControl();
		_viewModel = viewModel;
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		_viewModel.ReloadResources();
		_viewModel.PropertyChanged += delegate(object s, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Password" || e.PropertyName == "DoNotUsePassword")
			{
				SetNextButtonEnabledState();
			}
		};
	}

	public override bool OnBeforeActivate()
	{
		try
		{
			_viewModel.SystemPasswordMode = KeyManagementPasswordHelper.GetSystemPasswordMode(EncryptionKeyReader, GlobalData, LogManager.InnerLog);
		}
		catch (KeyStorageException ex) when (ex.InnerException is UnauthorizedAccessException)
		{
			throw new UIFrameworkErrorException(VideoOS.Installer.DatabasePlugin.Properties.Resources.AccessDenied_ErrorOccurred);
		}
		catch (KeyStorageException ex2)
		{
			throw new UIFrameworkErrorException(VideoOS.Installer.DatabasePlugin.Properties.Resources.KeyStorageExceptionGenericMessage + " " + ex2.Message, enablePreviousButtong: true);
		}
		return base.OnBeforeActivate();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		_viewModel.ReloadResources();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		SetNextButtonEnabledState();
	}

	public override bool OnBeforeContinue()
	{
		if (_viewModel.ShowEnterExistingPassword)
		{
			string text;
			try
			{
				text = KeyManagementPasswordHelper.VerifyExistingPassword(EncryptionKeyReader, _viewModel.Password, _viewModel.DoNotUsePassword, LogManager.InnerLog);
			}
			catch (KeyStorageException)
			{
				text = VideoOS.Installer.UI.Properties.Resources.ConfigurationPlugin_ErrorOccured;
			}
			if (!string.IsNullOrEmpty(text))
			{
				_viewModel.ShowErrorMessage(text);
				return false;
			}
		}
		else if (!_viewModel.DoNotUsePassword)
		{
			PasswordValidationResult passwordValidationResult = PasswordValidator.ValidateNewPassword(_viewModel.Password, _viewModel.RepeatedPassword);
			if (passwordValidationResult != PasswordValidationResult.Ok)
			{
				_viewModel.ShowErrorMessage(ToErrorMessage(passwordValidationResult));
				return false;
			}
		}
		return base.OnBeforeContinue();
	}

	private void SetNextButtonEnabledState()
	{
		Wizard.NextEnabled = _viewModel.DoNotUsePassword || _viewModel.Password.Length > 0;
	}

	private static string ToErrorMessage(PasswordValidationResult result)
	{
		switch (result)
		{
		case PasswordValidationResult.Ok:
			return string.Empty;
		case PasswordValidationResult.PasswordsDiffers:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordPasswordsDoNotMatch;
		case PasswordValidationResult.PasswordIsEmpty:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordEmpty;
		case PasswordValidationResult.TooShort:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordTooShort;
		case PasswordValidationResult.TooLong:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordTooLong;
		case PasswordValidationResult.ComplexityPolicyNotMet:
		case PasswordValidationResult.LocalPasswordFilterNotMet:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordDoesNotMeetComplexityPolicy;
		case PasswordValidationResult.OtherError:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordInvalid;
		default:
			return VideoOS.Installer.DatabasePlugin.Properties.Resources.NewPasswordInvalid;
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string> { 
		{
			"SYSTEMPASSWORD",
			EncryptHelper.EncryptPassword(_viewModel.Password.DecryptSecureString(), unicodePassword: true)
		} };
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.DatabasePlugin.KeyManagementPasswordWizard));
		base.WizardPage = new VideoOS.Installer.UI.Framework.WizardPage();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("0094219c-668a-4eec-83b6-77108baa051f");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "KeyManagementPasswordWizard";
		this.PageName = "KeyManagementPasswordWizardPage";
		base.ResumeLayout(false);
	}
}
