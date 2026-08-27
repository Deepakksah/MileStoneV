using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Mobile.CommonUtilities;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.InstallerWizard;

public class WizardPageServiceAccount : VideoOS.Installer.UI.Framework.WizardPageServiceAccount
{
	private bool _firstTimeLoad = true;

	private Dictionary<string, string> _parameters = new Dictionary<string, string>();

	private string _mosServiceUsername;

	private string _mosServicePassword = "";

	private string _mosIsUserServiceAccount;

	private IContainer components;

	public WizardPageServiceAccount()
		: base(disableAllAccounts: false, "LocalService;LocalSystem", null, null, null, disableAllPredefAccounts: false)
	{
		InitializeComponent();
		PageName = "AccountPage";
	}

	public override bool OnBeforeActivate()
	{
		if (!_firstTimeLoad)
		{
			return true;
		}
		_firstTimeLoad = false;
		ResetSubLine();
		PreselectPreviousServiceAccount();
		return true;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		_parameters["ENCRYPTEDPASSWORD"] = _mosServicePassword;
		_parameters["USERACCOUNT"] = _mosIsUserServiceAccount;
		_parameters["SERVICEACCOUNT"] = _mosServiceUsername;
		return _parameters;
	}

	protected override void LoadResources()
	{
		base.LoadResources();
		labelHeadline.Text = Strings.WizardPageServiceAccount_Headline;
	}

	public override bool OnBeforeContinue()
	{
		if (radioButtonAccounts.Checked)
		{
			if (!ValidateEntersValues())
			{
				return false;
			}
			string text = textBoxAccountName.Text;
			if (!text.Contains("\\"))
			{
				text = UserAccountHandling.CheckDomainOrLocalUser(textBoxAccountName.Text, Environment.MachineName, Environment.UserDomainName);
				if (text == textBoxAccountName.Text)
				{
					MessageHelper.ShowInstallerMessageBox(Strings.MessageBox_UnableToVerifyAccountType_Caption, Strings.MessageBox_UnableToVerifyAccountType, InstallerMessageBoxIcon.Warning);
					textBoxAccountName.Focus();
					textBoxAccountName.SelectAll();
					return false;
				}
			}
			ServiceAccountErrorString serviceAccountErrorString = Account.ServiceAccountPasswordValidation(text, textBoxPassword.Text);
			if (serviceAccountErrorString != ServiceAccountErrorString.Empty)
			{
				ServiceAccountErrorHelper.ThrowErrorMessage(serviceAccountErrorString);
				return false;
			}
			_mosIsUserServiceAccount = "true";
			_mosServicePassword = EncryptHelper.EncryptPassword(textBoxPassword.Text, unicodePassword: true);
			_mosServiceUsername = text;
		}
		else if (comboBoxPredefAccounts.SelectedItem is ServiceAccountItem serviceAccountItem)
		{
			_mosServiceUsername = serviceAccountItem.ServiceAccount.ToString();
			_mosIsUserServiceAccount = "false";
		}
		return true;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
	}

	private void PreselectPreviousServiceAccount()
	{
		try
		{
			SecurityIdentifier securityIdentifier = (SecurityIdentifier)new NTAccount(AccountHelper.GetAccountNameUnderWhichServiceRuns(OemResources.Instance.MobileServerFullProductName)).Translate(typeof(SecurityIdentifier));
			if (securityIdentifier.IsAccountSid())
			{
				NTAccount nTAccount = (NTAccount)securityIdentifier.Translate(typeof(NTAccount));
				SelectUserAccount(nTAccount.ToString());
			}
			else
			{
				SelectWellKnownAccount(ServiceAccount.NetworkService);
			}
		}
		catch
		{
			SelectWellKnownAccount(ServiceAccount.NetworkService);
		}
	}

	private void SelectWellKnownAccount(ServiceAccount defAccount)
	{
		_mosServiceUsername = defAccount.ToString();
		_mosIsUserServiceAccount = "false";
		radioButtonPredefinedAccounts.Enabled = true;
		radioButtonPredefinedAccounts.Checked = true;
		textBoxAccountName.Text = string.Empty;
		int num = 0;
		IEnumerator enumerator = comboBoxPredefAccounts.Items.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() && (!(enumerator.Current is ServiceAccountItem serviceAccountItem) || serviceAccountItem.ServiceAccount != defAccount))
			{
				num++;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		comboBoxPredefAccounts.SelectedIndex = num;
	}

	private void SelectUserAccount(string accountName)
	{
		_mosServiceUsername = accountName;
		_mosIsUserServiceAccount = "true";
		radioButtonPredefinedAccounts.Checked = false;
		radioButtonAccounts.Checked = true;
		textBoxAccountName.Text = accountName;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ServiceAccountControl";
		this.PageName = "ServiceAccountPage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
