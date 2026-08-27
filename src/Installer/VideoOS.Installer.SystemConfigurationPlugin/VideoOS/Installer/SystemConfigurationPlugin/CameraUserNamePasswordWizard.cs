using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI.Framework;
using VideoOS.Management.Common;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CameraUserNamePasswordWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private CameraUserNamePasswordControl _wpfControl = new CameraUserNamePasswordControl();

	private CameraUserNamePasswordViewModel _viewModel;

	private static int userRowCount = 3;

	private IContainer components;

	public CameraUserNamePasswordWizard()
	{
		InitializeComponent();
		PageName = "CameraUserNamePasswordPage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new CameraUserNamePasswordControl();
		_viewModel = new CameraUserNamePasswordViewModel(CreateUserNamePasswordCollection());
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		ReloadResources();
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

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string>
		{
			{
				ServerParameters.ScanDataUserNamePassword.ToString(),
				GetUserInfoAsXml(_viewModel.UserInfo)
			},
			{
				ServerParameters.UseHttps.ToString(),
				_viewModel.UseHttps.ToString()
			}
		};
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.TryGetValue(ServerParameters.ScanDataUserNamePassword.ToString(), out var value))
		{
			try
			{
				_viewModel.SetUsers(GetUserInfoFromXml(value));
			}
			catch
			{
				LogManager.Write("CameraUserNamePassword: Failed to extract data", TraceEventType.Information);
			}
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		UpdateWizardButtons();
	}

	private void UpdateWizardButtons()
	{
		Wizard.NextButton.AutoSize = false;
		Wizard.HidePreviousButton();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		ReloadResources();
	}

	private void ReloadResources()
	{
		_wpfControl.labelHeadLine.Text = Resources.UserNamePasswordHeader;
		_wpfControl.labelInfo.Text = Resources.UsernamePasswordInfo;
		_wpfControl.labelTip.Text = Resources.UserNamePasswordTip;
		_wpfControl.UserNameHeader.Text = Resources.UserName;
		_wpfControl.PasswordHeader.Text = Resources.Password;
	}

	private static string GetUserInfoAsXml(ObservableCollection<IUserInformation> userInformation)
	{
		return EncryptHelper.EncryptPassword(ServerCameraCredentials.GetAsXml(userInformation.Select((IUserInformation userInfo) => new ServerCameraCredential(userInfo.UserName, userInfo.Password.ConvertToUnsecureString())).ToArray()), unicodePassword: true);
	}

	public static ObservableCollection<IUserInformation> GetUserInfoFromXml(string scanDataEncrypted)
	{
		return EnsureAtLeastThreeUsers(new ObservableCollection<IUserInformation>(from c in ServerCameraCredentials.GetFromXml(EncryptHelper.DecryptPassword(scanDataEncrypted, unicodePassword: true))
			select new UserInformation(c.UserName, GetSecureString(c.Password))));
	}

	private static ObservableCollection<IUserInformation> EnsureAtLeastThreeUsers(ObservableCollection<IUserInformation> userInformation)
	{
		int num = userRowCount - userInformation.Count();
		for (int i = 0; i < num; i++)
		{
			userInformation.Add(new UserInformation("", new SecureString()));
		}
		return userInformation;
	}

	private static SecureString GetSecureString(string password)
	{
		SecureString secureString = new SecureString();
		try
		{
			foreach (char c in password)
			{
				secureString.AppendChar(c);
			}
			return secureString;
		}
		catch
		{
			secureString.Dispose();
			throw;
		}
	}

	private static ObservableCollection<IUserInformation> CreateUserNamePasswordCollection()
	{
		return EnsureAtLeastThreeUsers(new ObservableCollection<IUserInformation>());
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
		base.Name = "CameraUserNamePasswordWizard";
		this.PageName = "RecorderConfigurationPage";
		base.ResumeLayout(false);
	}
}
