using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.ServiceProcess;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public class WizardPageServiceAccount : WizardPageBase
{
	private bool _disableAllAccount;

	private string _disablePreferedAccounts;

	private string _defaultPredefAccount;

	private string _onParameterValue;

	private string _onParameterName;

	private bool _disableAllPredefAccounts;

	private string _curOnParameterValue;

	private IContainer components;

	protected TextBox textBoxAccountName;

	protected Label labelHeadline;

	protected Label labelSubHeading;

	protected RadioButton radioButtonPredefinedAccounts;

	protected ComboBox comboBoxPredefAccounts;

	protected RadioButton radioButtonAccounts;

	protected TextBox textBoxPassword;

	private Label labelPassword;

	private Label labelConfirmPass;

	protected TextBox textBoxConfirmPass;

	protected Button buttonBrowse;

	public WizardPageServiceAccount()
	{
	}

	public WizardPageServiceAccount(bool considerConfigFile)
	{
		InitializeComponent();
		SetupUI();
		SetControlsState();
		LoadResources();
		if (!considerConfigFile)
		{
			_disableAllAccount = false;
			_disablePreferedAccounts = null;
			_defaultPredefAccount = null;
			_onParameterName = null;
			_onParameterValue = null;
			_disableAllPredefAccounts = false;
		}
	}

	public WizardPageServiceAccount(bool disableAllAccounts, string disablePreferedAccounts, string defaultPredefAccount, string onParameterName, string onParameterValue, bool disableAllPredefAccounts)
	{
		InitializeComponent();
		_disableAllAccount = disableAllAccounts;
		_disablePreferedAccounts = disablePreferedAccounts;
		_defaultPredefAccount = defaultPredefAccount;
		_onParameterName = onParameterName;
		_onParameterValue = onParameterValue;
		_disableAllPredefAccounts = disableAllPredefAccounts;
		SetupUI();
		SetControlsState();
		LoadResources();
	}

	protected void ResetSubLine()
	{
		labelSubHeading.Text = Resources.ServiceAccount_SubHeadLine;
	}

	protected void SetupUI()
	{
		if (_disableAllAccount)
		{
			radioButtonPredefinedAccounts.Checked = true;
			radioButtonAccounts.Enabled = false;
		}
		List<string> list = new List<string>();
		if (!radioButtonPredefinedAccounts.Enabled)
		{
			return;
		}
		if (_disablePreferedAccounts != null)
		{
			if (_disablePreferedAccounts.IndexOf(';') != -1)
			{
				list = new List<string>(_disablePreferedAccounts.Split(';'));
			}
			else
			{
				list.Add(_disablePreferedAccounts);
			}
			if (!list.Contains(ServiceAccount.NetworkService.ToString()))
			{
				comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_NetworkServiceName, ServiceAccount.NetworkService));
			}
			if (!list.Contains(ServiceAccount.LocalService.ToString()))
			{
				comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_LocalServiceName, ServiceAccount.LocalService));
			}
			if (!list.Contains(ServiceAccount.LocalSystem.ToString()))
			{
				comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_LocalSystemName, ServiceAccount.LocalSystem));
			}
		}
		else
		{
			comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_NetworkServiceName, ServiceAccount.NetworkService));
			comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_LocalServiceName, ServiceAccount.LocalService));
			comboBoxPredefAccounts.Items.Add(new ServiceAccountItem(Resources.ServiceAccount_LocalSystemName, ServiceAccount.LocalSystem));
		}
		if (_defaultPredefAccount != null)
		{
			if (_defaultPredefAccount == ServiceAccount.NetworkService.ToString())
			{
				comboBoxPredefAccounts.SelectedIndex = 0;
			}
			else if (_defaultPredefAccount == ServiceAccount.LocalService.ToString())
			{
				comboBoxPredefAccounts.SelectedIndex = 1;
			}
			else if (_defaultPredefAccount == ServiceAccount.LocalSystem.ToString())
			{
				comboBoxPredefAccounts.SelectedIndex = 2;
			}
			else
			{
				comboBoxPredefAccounts.SelectedIndex = 0;
			}
		}
		else
		{
			comboBoxPredefAccounts.SelectedIndex = 0;
		}
	}

	protected virtual void LoadResources()
	{
		labelSubHeading.Text = Resources.ServiceAccount_SubHeadLine;
		radioButtonAccounts.Text = Resources.ServiceAccount_ThisAccount;
		radioButtonPredefinedAccounts.Text = Resources.ServiceAccount_PrefAccount;
		buttonBrowse.Text = Resources.ServiceAccount_BrowsButton;
		labelConfirmPass.Text = Resources.ServiceAccount_ConfirmPass;
		labelPassword.Text = Resources.ServiceAccount_Password;
		labelHeadline.Text = Resources.ServiceAccount_Headline;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return null;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		comboBoxPredefAccounts.Items.Clear();
		SetupUI();
		base.ReloadResources(culture);
		LoadResources();
	}

	protected void radioButtonAccounts_CheckedChanged(object sender, EventArgs e)
	{
		SetControlsState();
	}

	protected virtual void SetControlsState()
	{
		textBoxAccountName.Enabled = radioButtonAccounts.Checked;
		buttonBrowse.Enabled = radioButtonAccounts.Checked;
		textBoxConfirmPass.Enabled = radioButtonAccounts.Checked;
		textBoxPassword.Enabled = radioButtonAccounts.Checked;
		labelConfirmPass.Enabled = radioButtonAccounts.Checked;
		labelPassword.Enabled = radioButtonAccounts.Checked;
		comboBoxPredefAccounts.Enabled = !radioButtonAccounts.Checked;
	}

	private void SetupUI(bool isStandard)
	{
		radioButtonAccounts.Enabled = isStandard;
		radioButtonPredefinedAccounts.Enabled = isStandard;
		radioButtonAccounts.Checked = !isStandard;
	}

	private void buttonBrowse_Click(object sender, EventArgs e)
	{
		AccountInfo[] array = ObjectPickerWrapper.ShowUserObjectPicker(base.Handle);
		if (array == null)
		{
			return;
		}
		try
		{
			using (new ShowWaitCursor())
			{
				string[] array2 = array[0].ADPath.Split('/');
				if (array2 != null && array2.Length > 1)
				{
					string text = array2[array2.Length - 1];
					string text2 = array2[array2.Length - 2];
					textBoxAccountName.Text = text2 + "\\" + text;
				}
			}
		}
		catch (Exception ex)
		{
			MessageHelper.ShowInstallerMessageBox("", ex.Message);
		}
	}

	protected void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		if (GlobalData.TryGetValue("SERVERTYPE", out var value))
		{
			SetupUI(value == "Standard");
		}
	}

	public override bool OnBeforeContinue()
	{
		using (new ShowWaitCursor())
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
						MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_UnableToVerifyAccountType_Caption, Resources.MessageBox_UnableToVerifyAccountType, InstallerMessageBoxIcon.Warning);
						textBoxAccountName.Focus();
						textBoxAccountName.SelectAll();
						return false;
					}
				}
				GlobalData.SetValue("SERVICEACCOUNT", text, "ServiceAccountPage.OnBeforeContinue()");
				GlobalData.SetValue("SERVICEACCOUNT_NONLOC", text, "ServiceAccountPage.OnBeforeContinue()");
				GlobalData.SetValue("USERACCOUNT", "true", "ServiceAccountPage.OnBeforeContinue()");
				string value = EncryptHelper.EncryptPassword(textBoxPassword.Text, unicodePassword: true);
				GlobalData.SetValue("ENCRYPTEDPASSWORD", value, "ServiceAccountPage.OnBeforeContinue()");
				GlobalData.SetValue("INTERNALPASSWORD", textBoxPassword.Text, "ServiceAccountPage.OnBeforeContinue()");
			}
			else if (radioButtonPredefinedAccounts.Checked)
			{
				if (comboBoxPredefAccounts.SelectedItem is ServiceAccountItem serviceAccountItem)
				{
					GlobalData.SetValue("SERVICEACCOUNT", serviceAccountItem.AccountName, "ServiceAccountPage.OnBeforeContinue()");
					GlobalData.SetValue("SERVICEACCOUNT_NONLOC", serviceAccountItem.NonLocalisedAccountName, "ServiceAccountPage.OnBeforeContinue()");
				}
				GlobalData.SetValue("USERACCOUNT", "false", "ServiceAccountPage.OnBeforeContinue()");
				GlobalData.RemoveValue("ENCRYPTEDPASSWORD");
				GlobalData.RemoveValue("INTERNALPASSWORD");
			}
		}
		return true;
	}

	public bool ValidateEntersValues()
	{
		if (string.IsNullOrEmpty(textBoxAccountName.Text))
		{
			MessageHelper.ShowInstallerMessageBox(Resources.ServiceAccount_MessageValueInvalid, Resources.ServiceAccount_MessageAcountNameMissing, InstallerMessageBoxIcon.Exclamation, InstallerMessageBoxButtons.OK);
			textBoxAccountName.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(textBoxPassword.Text))
		{
			MessageHelper.ShowInstallerMessageBox(Resources.ServiceAccount_MessageValueInvalid, Resources.ServiceAccount_MessagePasswordInvalid, InstallerMessageBoxIcon.Exclamation, InstallerMessageBoxButtons.OK);
			textBoxPassword.Focus();
			return false;
		}
		if (textBoxPassword.Text != textBoxConfirmPass.Text)
		{
			MessageHelper.ShowInstallerMessageBox(Resources.ServiceAccount_MessageValueInvalid, Resources.ServiceAccount_MessagePasswordMismatch, InstallerMessageBoxIcon.Exclamation, InstallerMessageBoxButtons.OK);
			textBoxConfirmPass.Focus();
			return false;
		}
		return true;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (!string.IsNullOrEmpty(_onParameterName) && parameters.ContainsKey(_onParameterName))
		{
			_curOnParameterValue = parameters[_onParameterName];
		}
		if (_disableAllPredefAccounts)
		{
			bool flag = true;
			if (!string.IsNullOrEmpty(_onParameterName) && !string.IsNullOrEmpty(_onParameterValue))
			{
				string onParameterValue = _onParameterValue;
				flag = string.Compare(_curOnParameterValue, onParameterValue, StringComparison.InvariantCultureIgnoreCase) == 0;
			}
			if (flag)
			{
				radioButtonAccounts.Checked = true;
				radioButtonPredefinedAccounts.Enabled = false;
				comboBoxPredefAccounts.Enabled = false;
			}
			else
			{
				radioButtonPredefinedAccounts.Checked = true;
				radioButtonPredefinedAccounts.Enabled = true;
				comboBoxPredefAccounts.Enabled = true;
			}
		}
		if (_defaultPredefAccount != null || !parameters.ContainsKey("USERACCOUNT") || !parameters.ContainsKey("SERVICEACCOUNT"))
		{
			return;
		}
		if (string.Compare(parameters["USERACCOUNT"], "true", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			radioButtonAccounts.Checked = true;
			textBoxAccountName.Text = parameters["SERVICEACCOUNT"];
			return;
		}
		radioButtonPredefinedAccounts.Checked = true;
		string text = parameters["SERVICEACCOUNT"];
		if (text == ServiceAccount.NetworkService.ToString())
		{
			comboBoxPredefAccounts.SelectedIndex = 0;
		}
		else if (text == ServiceAccount.LocalService.ToString())
		{
			comboBoxPredefAccounts.SelectedIndex = 1;
		}
		else if (text == ServiceAccount.LocalSystem.ToString())
		{
			comboBoxPredefAccounts.SelectedIndex = 2;
		}
		else
		{
			comboBoxPredefAccounts.SelectedIndex = 0;
		}
	}

	protected void WizardPage_Enter(object sender, EventArgs e)
	{
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = Resources.btnCancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.Framework.WizardPageServiceAccount));
		this.textBoxAccountName = new System.Windows.Forms.TextBox();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.radioButtonPredefinedAccounts = new System.Windows.Forms.RadioButton();
		this.comboBoxPredefAccounts = new System.Windows.Forms.ComboBox();
		this.radioButtonAccounts = new System.Windows.Forms.RadioButton();
		this.textBoxPassword = new System.Windows.Forms.TextBox();
		this.labelPassword = new System.Windows.Forms.Label();
		this.labelConfirmPass = new System.Windows.Forms.Label();
		this.textBoxConfirmPass = new System.Windows.Forms.TextBox();
		this.buttonBrowse = new System.Windows.Forms.Button();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.buttonBrowse);
		base.WizardPage.Controls.Add(this.labelConfirmPass);
		base.WizardPage.Controls.Add(this.textBoxConfirmPass);
		base.WizardPage.Controls.Add(this.labelPassword);
		base.WizardPage.Controls.Add(this.textBoxPassword);
		base.WizardPage.Controls.Add(this.radioButtonAccounts);
		base.WizardPage.Controls.Add(this.comboBoxPredefAccounts);
		base.WizardPage.Controls.Add(this.radioButtonPredefinedAccounts);
		base.WizardPage.Controls.Add(this.textBoxAccountName);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Enter += new System.EventHandler(WizardPage_Enter);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		this.textBoxAccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxAccountName.Location = new System.Drawing.Point(66, 264);
		this.textBoxAccountName.Name = "textBoxAccountName";
		this.textBoxAccountName.Size = new System.Drawing.Size(202, 22);
		this.textBoxAccountName.TabIndex = 5;
		componentResourceManager.ApplyResources(this.textBoxAccountName, "textBoxAccountName");
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "Select service account";
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 46);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "The Management Server service will be started using this account. If the computer is part of a domain it must either be \"Network Service\" or a domain user account.";
		componentResourceManager.ApplyResources(this.labelSubHeading, "labelSubHeading");
		this.radioButtonPredefinedAccounts.AutoSize = true;
		this.radioButtonPredefinedAccounts.Checked = true;
		this.radioButtonPredefinedAccounts.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonPredefinedAccounts.Location = new System.Drawing.Point(50, 150);
		this.radioButtonPredefinedAccounts.Name = "radioButtonPredefinedAccounts";
		this.radioButtonPredefinedAccounts.Size = new System.Drawing.Size(150, 18);
		this.radioButtonPredefinedAccounts.TabIndex = 2;
		this.radioButtonPredefinedAccounts.TabStop = true;
		this.radioButtonPredefinedAccounts.Text = "This &prefered account:";
		this.radioButtonPredefinedAccounts.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.radioButtonPredefinedAccounts, "radioButtonPredefinedAccounts");
		this.comboBoxPredefAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBoxPredefAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBoxPredefAccounts.FormattingEnabled = true;
		this.comboBoxPredefAccounts.Location = new System.Drawing.Point(66, 174);
		this.comboBoxPredefAccounts.Name = "comboBoxPredefAccounts";
		this.comboBoxPredefAccounts.Size = new System.Drawing.Size(202, 24);
		this.comboBoxPredefAccounts.TabIndex = 3;
		componentResourceManager.ApplyResources(this.comboBoxPredefAccounts, "comboBoxPredefAccounts");
		this.radioButtonAccounts.AutoSize = true;
		this.radioButtonAccounts.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonAccounts.Location = new System.Drawing.Point(49, 240);
		this.radioButtonAccounts.Name = "radioButtonAccounts";
		this.radioButtonAccounts.Size = new System.Drawing.Size(99, 18);
		this.radioButtonAccounts.TabIndex = 4;
		this.radioButtonAccounts.Text = "This &account:";
		this.radioButtonAccounts.UseVisualStyleBackColor = true;
		this.radioButtonAccounts.CheckedChanged += new System.EventHandler(radioButtonAccounts_CheckedChanged);
		componentResourceManager.ApplyResources(this.radioButtonAccounts, "radioButtonAccounts");
		this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxPassword.Location = new System.Drawing.Point(66, 329);
		this.textBoxPassword.Name = "textBoxPassword";
		this.textBoxPassword.PasswordChar = '*';
		this.textBoxPassword.Size = new System.Drawing.Size(202, 22);
		this.textBoxPassword.TabIndex = 8;
		componentResourceManager.ApplyResources(this.textBoxPassword, "textBoxPassword");
		this.labelPassword.AutoSize = true;
		this.labelPassword.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelPassword.Location = new System.Drawing.Point(66, 313);
		this.labelPassword.Name = "labelPassword";
		this.labelPassword.Size = new System.Drawing.Size(62, 14);
		this.labelPassword.TabIndex = 7;
		this.labelPassword.Text = "&Password:";
		componentResourceManager.ApplyResources(this.labelPassword, "labelPassword");
		this.labelConfirmPass.AutoSize = true;
		this.labelConfirmPass.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelConfirmPass.Location = new System.Drawing.Point(66, 354);
		this.labelConfirmPass.Name = "labelConfirmPass";
		this.labelConfirmPass.Size = new System.Drawing.Size(107, 14);
		this.labelConfirmPass.TabIndex = 9;
		this.labelConfirmPass.Text = "C&onfirm Password:";
		componentResourceManager.ApplyResources(this.labelConfirmPass, "labelConfirmPass");
		this.textBoxConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxConfirmPass.Location = new System.Drawing.Point(66, 370);
		this.textBoxConfirmPass.Name = "textBoxConfirmPass";
		this.textBoxConfirmPass.PasswordChar = '*';
		this.textBoxConfirmPass.Size = new System.Drawing.Size(202, 22);
		this.textBoxConfirmPass.TabIndex = 10;
		componentResourceManager.ApplyResources(this.textBoxConfirmPass, "textBoxConfirmPass");
		this.buttonBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.buttonBrowse.Font = new System.Drawing.Font("Tahoma", 9f);
		this.buttonBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.buttonBrowse.Location = new System.Drawing.Point(274, 264);
		this.buttonBrowse.Name = "buttonBrowse";
		this.buttonBrowse.Size = new System.Drawing.Size(100, 22);
		this.buttonBrowse.TabIndex = 6;
		this.buttonBrowse.Text = "&Browse...";
		this.buttonBrowse.Click += new System.EventHandler(buttonBrowse_Click);
		componentResourceManager.ApplyResources(this.buttonBrowse, "buttonBrowse");
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageServiceAccount";
		this.PageName = "ServiceAccountPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
