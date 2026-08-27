using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.RecorderPlugins.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.RecorderPlugins;

public class ServerServiceAccountControl : WizardPageServiceAccount
{
	private class SystemServiceAccount
	{
		public ServiceAccount ServiceAccount { get; }

		public string LocalizedName { get; }

		public string NonLocalizedName { get; }

		public SystemServiceAccount(ServiceAccount serviceAccount)
		{
			WellKnownSidType sidType;
			switch (serviceAccount)
			{
			case ServiceAccount.LocalService:
				sidType = WellKnownSidType.LocalServiceSid;
				NonLocalizedName = "NT AUTHORITY\\LocalService";
				break;
			case ServiceAccount.LocalSystem:
				sidType = WellKnownSidType.LocalSystemSid;
				NonLocalizedName = "LocalSystem";
				break;
			case ServiceAccount.NetworkService:
				sidType = WellKnownSidType.NetworkServiceSid;
				NonLocalizedName = "NT AUTHORITY\\NetworkService";
				break;
			default:
				throw new ArgumentException("", "serviceAccount");
			}
			ServiceAccount = serviceAccount;
			NTAccount nTAccount = (NTAccount)new SecurityIdentifier(sidType, null).Translate(typeof(NTAccount));
			LocalizedName = nTAccount.Value;
		}
	}

	private const string OldRecServerProgramDataFolder = "XProtect Corporate Recording Server";

	private const string OldFailoverProgramDataFolder = "XProtect Corporate Failover Server";

	private const string OnBeforeContinueDebugName = "ServerServiceAccount.OnBeforeContinue()";

	private const string OnBeforeActivateDebugName = "ServerServiceAccount.OnBeforeActivate()";

	private bool? showPage;

	private static IEnumerable<SystemServiceAccount> systemServiceAccounts = new SystemServiceAccount[2]
	{
		new SystemServiceAccount(ServiceAccount.LocalSystem),
		new SystemServiceAccount(ServiceAccount.NetworkService)
	};

	private IContainer components;

	public ServerServiceAccountControl()
		: base(disableAllAccounts: false, "LocalService", null, null, null, disableAllPredefAccounts: false)
	{
		InitializeComponent();
		PageName = "ServerServiceAccountPage";
	}

	public override bool OnBeforeActivate()
	{
		if (!showPage.HasValue)
		{
			showPage = OnBeforeActivateInternal();
		}
		return showPage.Value;
	}

	public bool OnBeforeActivateInternal()
	{
		ResetSubLine();
		(bool, string, ServiceAccount) tuple = ReadCurrentSelectedUser();
		(bool, string, ServiceAccount) tuple2 = ReadCurrentRecordingServerServiceUser();
		bool flag = tuple.Item1 && tuple2.Item1 && ((tuple.Item3 != ServiceAccount.User && tuple.Item3 != tuple2.Item3) || (tuple.Item3 == ServiceAccount.User && !string.Equals(tuple.Item2, tuple2.Item2, StringComparison.OrdinalIgnoreCase)));
		(bool, string, ServiceAccount) tuple3 = (flag ? tuple2 : tuple);
		if (PluginUtilities.Instance.IsTypical)
		{
			LogManager.Write("TYPICAL mode selected.", TraceEventType.Information);
			radioButtonPredefinedAccounts.Enabled = true;
			if (tuple3.Item1)
			{
				if (tuple3.Item3 == ServiceAccount.User)
				{
					SetSelectedUserAccount(tuple3.Item2);
					return true;
				}
				SetDefaultPredefinedAccount(tuple3.Item3);
				return flag;
			}
			if (RecorderConfigXmlFileExists())
			{
				SetDefaultPredefinedAccount(ServiceAccount.LocalSystem);
				return true;
			}
			SetDefaultPredefinedAccount(ServiceAccount.NetworkService);
			return false;
		}
		if (PluginUtilities.Instance.FailoverInstallation)
		{
			LogManager.Write("FAILOVER mode selected.", TraceEventType.Information);
			radioButtonPredefinedAccounts.Enabled = false;
			SetSelectedUserAccount(tuple3.Item2);
			labelSubHeading.Text = Resources.ServiceAccount_FailoverSubLine;
			return true;
		}
		if (PluginUtilities.Instance.IsCustom)
		{
			LogManager.Write("CUSTOM mode selected.", TraceEventType.Information);
			if (tuple3.Item1)
			{
				if (tuple3.Item3 == ServiceAccount.User)
				{
					radioButtonPredefinedAccounts.Checked = false;
					SetSelectedUserAccount(tuple3.Item2);
				}
				else
				{
					SetDefaultPredefinedAccount(tuple3.Item3);
				}
			}
			else
			{
				SetDefaultPredefinedAccount((!RecorderConfigXmlFileExists()) ? ServiceAccount.NetworkService : ServiceAccount.LocalSystem);
			}
			return true;
		}
		throw new InvalidOperationException("No service user selection has been made for the recording server install.");
	}

	private void SetSelectedUserAccount(string userName)
	{
		radioButtonAccounts.Checked = true;
		textBoxAccountName.Text = userName;
	}

	private static ServiceAccount ResolveServiceAccount(string userName)
	{
		ServiceAccount serviceAccount = ServiceAccount.User;
		SystemServiceAccount systemServiceAccount = systemServiceAccounts.FirstOrDefault((SystemServiceAccount a) => userName.EndsWith(a.LocalizedName, StringComparison.OrdinalIgnoreCase) || userName.EndsWith(a.NonLocalizedName, StringComparison.OrdinalIgnoreCase));
		if (systemServiceAccount != null)
		{
			serviceAccount = systemServiceAccount.ServiceAccount;
			LogManager.Write(FormattableString.Invariant($"Resolved user name '{userName}' to service account type '{systemServiceAccount.ServiceAccount}' with name '{systemServiceAccount.NonLocalizedName}' (localized '{systemServiceAccount.LocalizedName}')"));
		}
		else
		{
			LogManager.Write(FormattableString.Invariant($"Resolved user name '{userName}' to service account type '{serviceAccount}'"));
		}
		return serviceAccount;
	}

	private static (bool HasSelection, string UserName, ServiceAccount ServiceAccount) ReadCurrentSelectedUser()
	{
		string text = string.Empty;
		bool flag = false;
		try
		{
			text = PluginUtilities.Instance.GetValueFromRegistry(PluginUtilities.Instance.LookupRegistryKey, PluginUtilities.Instance.LookupUser);
			LogManager.Write(PluginUtilities.Instance.LookupRegistryKey);
			string valueFromRegistry = PluginUtilities.Instance.GetValueFromRegistry(PluginUtilities.Instance.LookupRegistryKey, "UserAccount");
			if (!string.IsNullOrEmpty(valueFromRegistry))
			{
				flag = string.Equals(valueFromRegistry, "true", StringComparison.OrdinalIgnoreCase);
			}
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("Retrieved from the registry: {0}='{1}' {2}='{3}'", PluginUtilities.Instance.LookupUser, text, "UserAccount", flag)), TraceEventType.Information);
		}
		catch (ArgumentNullException ex)
		{
			LogManager.Write("ArgumentNullException: " + ex.ParamName);
		}
		catch (ArgumentException ex2)
		{
			LogManager.Write("ArgumentException: " + ex2.ParamName + " " + ex2.Message);
		}
		catch (Exception ex3)
		{
			LogManager.Write(ex3.Message);
		}
		bool flag2 = !string.IsNullOrEmpty(text);
		ServiceAccount item = (flag ? ServiceAccount.User : (flag2 ? ResolveServiceAccount(text) : ServiceAccount.User));
		return (HasSelection: flag2, UserName: text, ServiceAccount: item);
	}

	private static (bool HasSelection, string UserName, ServiceAccount ServiceAccount) ReadCurrentRecordingServerServiceUser()
	{
		try
		{
			using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Name, StartName FROM Win32_Service");
			var anon = (from ManagementObject s in managementObjectSearcher.Get()
				select new
				{
					Name = s["Name"].ToString(),
					User = (s["StartName"]?.ToString() ?? "")
				}).FirstOrDefault(s => string.Equals(OemResources.Instance.Recorder.ServiceName, s.Name, StringComparison.OrdinalIgnoreCase));
			if (anon != null)
			{
				string text = anon.User;
				Match match = Regex.Match(text, "(?<user>[^@]+)@(?<domain>[^@]+)");
				if (match.Success)
				{
					string value = match.Groups["user"].Value;
					text = match.Groups["domain"].Value + "\\" + value;
				}
				(bool, string, ServiceAccount) result = (true, text, ResolveServiceAccount(anon.User));
				LogManager.Write(FormattableString.Invariant($"Recording server currently running as : User name: '{result.Item2}', Service account: '{result.Item3}'"), TraceEventType.Information);
				return result;
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.Message);
		}
		return (HasSelection: false, UserName: string.Empty, ServiceAccount: ServiceAccount.User);
	}

	private void SetDefaultPredefinedAccount(ServiceAccount defAccount)
	{
		ServiceAccountItem serviceAccountItem = new ServiceAccountItem(defAccount.ToString(), defAccount);
		GlobalData.SetValue("RECSERVICEACCOUNT", serviceAccountItem.AccountName, "ServerServiceAccount.OnBeforeActivate()");
		radioButtonPredefinedAccounts.Enabled = true;
		radioButtonPredefinedAccounts.Checked = true;
		textBoxAccountName.Text = string.Empty;
		int num = 0;
		IEnumerator enumerator = comboBoxPredefAccounts.Items.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() && (!(enumerator.Current is ServiceAccountItem serviceAccountItem2) || serviceAccountItem2.ServiceAccount != defAccount))
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

	private static bool RecorderConfigXmlFileExists()
	{
		if (File.Exists(Path.Combine(PluginUtilities.Instance.FailoverInstallation ? OemResources.Instance.FailoverServer.CommonApplicationDataFolder : OemResources.Instance.Recorder.CommonApplicationDataFolder, "RecorderConfig.xml")))
		{
			LogManager.Write("RecorderConfig.xml was found on the machine.", TraceEventType.Information);
			return true;
		}
		if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName, PluginUtilities.Instance.FailoverInstallation ? "XProtect Corporate Failover Server" : "XProtect Corporate Recording Server", "RecorderConfig.xml")))
		{
			LogManager.Write("RecorderConfig.xml of pre 13 version was found on the machine.", TraceEventType.Information);
			return true;
		}
		return false;
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
					MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_UnableToVerifyAccountType_Caption, Resources.MessageBox_UnableToVerifyAccountType, InstallerMessageBoxIcon.Warning);
					textBoxAccountName.Focus();
					textBoxAccountName.SelectAll();
					return false;
				}
			}
			if (IsUpgradeAndServiceUserChanged(text) && MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_MakeSureUserHasAccess_Caption, string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_MakeSureUserHasAccess, text), InstallerMessageBoxIcon.Warning, InstallerMessageBoxButtons.YesNo) == DialogResult.No)
			{
				textBoxAccountName.Focus();
				textBoxAccountName.SelectAll();
				return false;
			}
			ServiceAccountErrorString serviceAccountErrorString = Account.ServiceAccountPasswordValidation(text, textBoxPassword.Text);
			if (serviceAccountErrorString != ServiceAccountErrorString.Empty)
			{
				ServiceAccountErrorHelper.ThrowErrorMessage(serviceAccountErrorString);
				return false;
			}
			if (PluginUtilities.Instance.FailoverInstallation)
			{
				ServiceAccountErrorString serviceAccountErrorString2 = ServiceAccountErrorString.Empty;
				using (new ShowWaitCursor())
				{
					if (!Account.HasAccountAccountRight(string.Empty, text, "SeInteractiveLogonRight"))
					{
						serviceAccountErrorString2 = Account.SetServiceAccountRights("SeInteractiveLogonRight", text, textBoxPassword.Text);
					}
					if (serviceAccountErrorString2 != ServiceAccountErrorString.Empty)
					{
						return false;
					}
					if (!Account.HasAccountAccountRight(string.Empty, text, "SeServiceLogonRight"))
					{
						serviceAccountErrorString2 = Account.SetServiceAccountRights("SeServiceLogonRight", text, textBoxPassword.Text);
					}
					if (serviceAccountErrorString2 != ServiceAccountErrorString.Empty)
					{
						return false;
					}
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.Logging_ServiceAccountAddRightGranted, text), TraceEventType.Information);
				}
			}
			GlobalData.SetValue("RECSERVICEACCOUNT", text, "ServerServiceAccount.OnBeforeContinue()");
			GlobalData.SetValue("RECPASSWORD", EncryptHelper.EncryptPassword(textBoxPassword.Text, unicodePassword: true), "ServerServiceAccount.OnBeforeContinue()");
			GlobalData.SetValue("RECUSERACCOUNT", "true", "ServerServiceAccount.OnBeforeContinue()");
		}
		else
		{
			if (comboBoxPredefAccounts.SelectedItem is ServiceAccountItem serviceAccountItem)
			{
				if (IsUpgradeAndServiceUserChanged(serviceAccountItem.AccountName) && MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_MakeSureUserHasAccess_Caption, string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_MakeSureUserHasAccess, serviceAccountItem.AccountName), InstallerMessageBoxIcon.Warning, InstallerMessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				GlobalData.SetValue("RECSERVICEACCOUNT", serviceAccountItem.AccountName, "ServerServiceAccount.OnBeforeContinue()");
			}
			GlobalData.SetValue("RECUSERACCOUNT", "false", "ServerServiceAccount.OnBeforeContinue()");
			GlobalData.RemoveValue("RECPASSWORD");
		}
		return true;
	}

	private static bool IsUpgradeAndServiceUserChanged(string userAccount)
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_UPGRADE") && InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true")
		{
			if (InstallerManager.Instance.Parameters.ContainsKey("RECSERVICEACCOUNT"))
			{
				return !string.Equals(InstallerManager.Instance.Parameters["RECSERVICEACCOUNT"], userAccount, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
		return false;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		labelHeadline.Text = Resources.ServiceAccount_HeadLine;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
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
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ServerServiceAccountControl";
		this.PageName = "ServerServiceAccountPage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
