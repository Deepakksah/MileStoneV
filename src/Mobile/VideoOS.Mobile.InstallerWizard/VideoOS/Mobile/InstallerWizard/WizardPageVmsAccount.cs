using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Mobile.Users;

namespace VideoOS.Mobile.InstallerWizard;

public class WizardPageVmsAccount : VideoOS.Installer.UI.Framework.WizardPageServiceAccount
{
	private bool _useBasicVmsUser;

	private string _vmsUsername = string.Empty;

	private string _vmsPassword = string.Empty;

	private IContainer components;

	public WizardPageVmsAccount()
		: base(disableAllAccounts: false, "LocalService;LocalSystem", null, null, null, disableAllPredefAccounts: false)
	{
		InitializeComponent();
		PageName = "VmsAccountPage";
		PopulateBasicUser();
	}

	public override bool OnBeforeActivate()
	{
		PopulateServiceUser();
		return true;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string>
		{
			["MOSVMSUSEBASICUSER"] = _useBasicVmsUser.ToString().ToLower(),
			["MOSVMSUSER"] = _vmsUsername,
			["MOSVMSPASSWORD"] = _vmsPassword
		};
	}

	protected override void LoadResources()
	{
		base.LoadResources();
		buttonBrowse.Visible = false;
		labelHeadline.Text = Strings.WizardPageVmsAccount_Headline;
		labelSubHeading.Text = Strings.WizardPageVmsAccount_Description;
		radioButtonPredefinedAccounts.Text = Strings.WizardPageVmsAccount_RadioButton_ServiceUser;
		radioButtonAccounts.Text = Strings.WizardPageVmsAccount_RadioButton_BasicUser;
	}

	protected override void SetControlsState()
	{
		base.SetControlsState();
		comboBoxPredefAccounts.Enabled = false;
	}

	public override bool OnBeforeContinue()
	{
		_useBasicVmsUser = radioButtonAccounts.Checked;
		if (_useBasicVmsUser)
		{
			if (!ValidateEntersValues())
			{
				return false;
			}
			_vmsUsername = textBoxAccountName.Text;
			_vmsPassword = EncryptHelper.EncryptPassword(textBoxPassword.Text, unicodePassword: true);
		}
		return true;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
	}

	private void PopulateServiceUser()
	{
		GlobalData.TryGetValue("SERVICEACCOUNT", out var value);
		int num = 0;
		IEnumerator enumerator = comboBoxPredefAccounts.Items.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() && (!(enumerator.Current is ServiceAccountItem { ServiceAccount: var serviceAccount }) || !(serviceAccount.ToString() == value)))
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
		if (num == comboBoxPredefAccounts.Items.Count)
		{
			comboBoxPredefAccounts.Items.Add(value);
		}
		comboBoxPredefAccounts.SelectedIndex = num;
		comboBoxPredefAccounts.Enabled = false;
	}

	private void PopulateBasicUser()
	{
		try
		{
			ICredentials credentials = BasicUserCredentialsStore.Instance.Load(reencrypt: false);
			if (credentials != null)
			{
				textBoxAccountName.Text = credentials.User;
				radioButtonAccounts.Checked = true;
			}
			else
			{
				radioButtonPredefinedAccounts.Checked = true;
			}
		}
		catch (Exception)
		{
		}
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
		base.Name = "VmsAccountControl";
		this.PageName = "VmsAccountPage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
