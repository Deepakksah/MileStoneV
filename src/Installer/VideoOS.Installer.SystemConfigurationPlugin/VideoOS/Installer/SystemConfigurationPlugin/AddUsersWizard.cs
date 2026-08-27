using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddUsersWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private UserPageControl _wpfControl = new UserPageControl();

	private UserPageViewModel _viewModel;

	private int _port = 80;

	private List<string> _addedUsers = new List<string>();

	private List<string> _failedUsers = new List<string>();

	private IContainer components;

	public AddUsersWizard()
	{
		InitializeComponent();
		PageName = "AddUsersPage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_elementHost.Child = _wpfControl;
		ReloadResources();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(ServerParameters.AddedUsers.ToString(), SystemConfigurationHelper.ConvertCollectionToCsvString(_addedUsers));
		if (_failedUsers.Count != 0)
		{
			dictionary.Add(ServerParameters.FailedUsers.ToString(), SystemConfigurationHelper.ConvertCollectionToCsvString(_failedUsers));
		}
		return dictionary;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		ICollection<string> roles = new List<string>();
		string value = string.Empty;
		if (parameters.TryGetValue(ServerParameters.Roles.ToString(), out value))
		{
			roles = SystemConfigurationHelper.ConvertCsvStringToCollection(value);
		}
		_viewModel.LoadRoles(roles);
	}

	public override bool OnBeforeActivate()
	{
		string value = string.Empty;
		if (!GlobalData.TryGetValue(ServerParameters.Roles.ToString(), out value))
		{
			GlobalData.SetValue(ServerParameters.FailedUsers.ToString(), Resources.UserPageCouldNotLoadRoles, "AddUsersWizard.OnBeforeActivate()");
			return false;
		}
		return true;
	}

	public override bool OnBeforeContinue()
	{
		string empty = string.Empty;
		RolePlugin rolePlugin = new RolePlugin(_port);
		using (ConfigurationApiServiceManager serviceManager = new ConfigurationApiServiceManager(_port))
		{
			foreach (IAddedUsersHelper addedUser in _viewModel.AddedUsers)
			{
				if (addedUser.UserType == UserPageViewModel.UserType.BasicUser)
				{
					if (!rolePlugin.CreateBasicUser(serviceManager, addedUser.UserName, addedUser.Password.ConvertToUnsecureString()))
					{
						_failedUsers.Add(addedUser.UserName);
						continue;
					}
					empty = rolePlugin.GetBasicUserSid(serviceManager, addedUser.UserName);
					if (string.IsNullOrEmpty(empty))
					{
						_failedUsers.Add(addedUser.UserName);
						continue;
					}
				}
				else
				{
					empty = addedUser.Identity.Sid;
				}
				if (rolePlugin.AddUserToRole(serviceManager, empty, addedUser.RoleName))
				{
					_addedUsers.Add(addedUser.UserName);
				}
				else
				{
					_failedUsers.Add(addedUser.UserName);
				}
			}
		}
		return true;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		ReloadResources();
	}

	private void ReloadResources()
	{
		_viewModel = new UserPageViewModel();
		_viewModel.LoadResources();
		_wpfControl.DataContext = _viewModel;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = true;
		Wizard.NextButton.Visible = true;
		Wizard.NextButton.Enabled = true;
		Wizard.NavigationPanel.Visible = true;
		Wizard.HidePreviousButton();
		if (GlobalData.TryGetValue(ServerParameters.ServerPort.ToString(), out var value) && !int.TryParse(value, out _port))
		{
			_port = 80;
		}
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
		base.Name = "AddUsersWizard";
		this.PageName = "AddUsersWizardPage";
		base.ResumeLayout(false);
	}
}
