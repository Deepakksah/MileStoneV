using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Sequencer;
using VideoOS.Installer.Sequencer.ErrorHandling;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageInstall : WizardPageBase
{
	private delegate void SetGraphicsCallback(GraphicsInfo graphicsInfo);

	private delegate void SetContinueDelegate();

	private delegate void ShowErrorDelegate(Exception ex);

	private delegate void SetCurrentProgressDelegate(int value, int maximum);

	private delegate void SetOverallProgressDelegate(int value, int maximum);

	private delegate void SetMessageDelegate(string message);

	private delegate void SetActionDelegate(string action);

	private IContainer components;

	private Label labelHeadline;

	private ProgressBar progressBarTotalProgress;

	private Label labelTotalProgress;

	private ProgressBar progressBarComponent;

	private Label labelComponentProgress;

	public WizardPageInstall()
	{
		InitializeComponent();
		labelHeadline.UseMnemonic = false;
		labelComponentProgress.UseMnemonic = false;
		InstallerManager.Instance.CreateSequenceManager();
		InstallerManager.Instance.SequenceManagerWrapper.TaskSequencer.CurrentProgress += OnCurrentProgress;
		InstallerManager.Instance.SequenceManagerWrapper.TaskSequencer.OverallProgress += OnOverallProgress;
		InstallerManager.Instance.SequenceManagerWrapper.TaskSequencer.MessageChanged += OnMessageChanged;
	}

	private void LoadResources()
	{
		labelComponentProgress.Text = string.Format(Resources.Install_LabelComponentProgress, string.Empty);
		labelTotalProgress.Text = string.Format(Resources.Install_LabelTotalProgress);
		labelHeadline.Text = (InstallerManager.Instance.IsModify ? string.Format(Resources.Modify_LabelHeadLine, VersionFormat.GenerateProductNameWithVersion()) : string.Format(Resources.Install_LabelHeadLine, VersionFormat.GenerateProductNameWithVersion()));
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void OnCurrentProgress(object sender, ProgressEventArgs args)
	{
		SetCurrentProgress(args.Value, args.Maximum);
	}

	private void SetCurrentProgress(int value, int maximum)
	{
		if (progressBarComponent.InvokeRequired)
		{
			SetCurrentProgressDelegate method = SetCurrentProgress;
			progressBarComponent.Invoke(method, value, maximum);
			return;
		}
		if (value > maximum)
		{
			value = maximum - 1;
		}
		progressBarComponent.Maximum = maximum;
		progressBarComponent.Value = value;
	}

	private void SetOverallProgress(int value, int maximum)
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			SetOverallProgressDelegate method = SetOverallProgress;
			progressBarComponent.Invoke(method, value, maximum);
			return;
		}
		if (value > maximum)
		{
			value = maximum - 1;
		}
		else if (value < 0)
		{
			value = 0;
		}
		progressBarTotalProgress.Maximum = maximum;
		progressBarTotalProgress.Value = value;
	}

	private void SetMessage(string message)
	{
		if (message != null)
		{
			if (progressBarComponent.InvokeRequired)
			{
				SetMessageDelegate method = SetMessage;
				progressBarComponent.Invoke(method, message);
			}
			else
			{
				message = message.Replace("'", "");
				labelComponentProgress.Text = message;
			}
		}
	}

	private void OnMessageChanged(object sender, MessageEventArgs args)
	{
		SetMessage(args.Message);
	}

	private void OnOverallProgress(object sender, ProgressEventArgs args)
	{
		progressBarTotalProgress.Invoke(new SetOverallProgressDelegate(SetOverallProgress), args.Value, args.Maximum);
	}

	private void wizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.HideNavigationButtons();
		Wizard.CancelButton.Hide();
	}

	private void InstallationEnded(IAsyncResult ar)
	{
		try
		{
			SetCurrentProgress(1, 1);
			SetOverallProgress(1, 1);
			InstallerManager.Instance.SequenceManager.EndInstall(ar);
		}
		catch (DowngradeDisallowedException ex)
		{
			LogManager.Write("Install failed." + ex, TraceEventType.Error);
			ShowErrorPage(ex);
			return;
		}
		catch (Exception ex2)
		{
			if (InstallerManager.Instance.SequenceManager.NeedReboot)
			{
				HandleReboot();
				return;
			}
			LogManager.Write("Install failed." + ex2, TraceEventType.Error);
			FinalizeInstallation();
			ShowErrorPage(ex2);
			return;
		}
		if (FinalizeInstallation() && CheckInstalledPackages())
		{
			if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
			{
				InstallerManager.Instance.RemoveInstallerShortcut();
			}
			NextPage();
		}
	}

	private void HandleReboot()
	{
		if (InstallerManager.Instance.SequenceManager.NotExecutedInstallPackages.Count != 0)
		{
			InstallerManager.Instance.SaveIntermediateInstallInfo(PageName, BreakAction.InstallContinue);
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_RestartLabelHead, Resources.ErrorMessage_RestartLabelSubLine, new RestartAction(), null, enablePreviousButton: false);
			ShowErrorPage(null);
		}
		else
		{
			FinalizeInstallation();
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_RestartLabelHead, Resources.ErrorMessage_RestartToCompleteLabelSubLine, new RestartAction(), null, enablePreviousButton: false);
			ShowErrorPage(null);
		}
	}

	private bool FinalizeInstallation()
	{
		try
		{
			InstallerManager.Instance.SaveInstallationInfo(doContinue: false);
			if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
			{
				InstallerManager.Instance.UpdateInstallInfoAfterFinalize();
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Error);
			ShowErrorPage(ex);
			return false;
		}
		return true;
	}

	private bool CheckInstalledPackages()
	{
		Dictionary<string, string> failedInstallPackages = InstallerManager.Instance.SequenceManager.FailedInstallPackages;
		if (failedInstallPackages.Count != 0)
		{
			string text = "";
			string subLine = string.Format(Resources.ErrorMessage_PackagesFailedLabelSubLine, VersionFormat.GenerateProductNameWithVersion());
			Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
			InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ProductManifest.Format == FormatType.Milestone)
				{
					_ = installerPackageType.ProductManifest.PackageData.ProductInfo.Manufacture;
					if (installerPackageType.ProductManifest.PackageData.ProductInfo.ManufactureLink != null)
					{
						_ = installerPackageType.ProductManifest.PackageData.ProductInfo.ManufactureLink;
					}
					dictionary[installerPackageType.ID] = installerPackageType;
				}
			}
			foreach (KeyValuePair<string, string> item in failedInstallPackages)
			{
				string text2 = item.Value;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = Resources.ErrorMessage_GeneralErrorLabelSubLine;
				}
				if (dictionary.ContainsKey(item.Key) && dictionary[item.Key].ProductManifest.MainPackage)
				{
					ErrorInfo.SetErrorInfo(string.Format(Resources.ErrorMessage_PackagesFailedLabelHeadLine, VersionFormat.GenerateProductNameWithVersion()), null, null, string.Format(Resources.ErrorMessage_GeneralErrorLabelMessage, text2), enablePreviousButton: false);
					ShowErrorPage(null);
					return false;
				}
				if (dictionary.ContainsKey(item.Key))
				{
					string arg = $"{dictionary[item.Key].ProductManifest.PackageData.DisplayName} \t {string.Format(Resources.ErrorMessage_GeneralErrorLabelMessage, text2)} \n";
					text += $"{arg}\n";
					continue;
				}
				text += $"{item.Value}\n";
				string prerequisiteCode = InstallerManager.Instance.SequenceManager.GetPrerequisiteCode(item.Key);
				if (prerequisiteCode != null && prerequisiteCode == "Microsoft.Net.Framework.3.5")
				{
					subLine = "";
				}
			}
			ErrorInfo.SetErrorInfo(string.Format(Resources.ErrorMessage_PackagesFailedLabelHeadLine, VersionFormat.GenerateProductNameWithVersion()), subLine, null, text, enablePreviousButton: false);
			ShowErrorPage(null);
			return false;
		}
		return true;
	}

	private bool SetPackageDetails(Dictionary<string, InstallerPackageType> installPackagesId, KeyValuePair<string, string> failedPackage, ref string packages)
	{
		string text = failedPackage.Value;
		if (string.IsNullOrEmpty(text))
		{
			text = Resources.ErrorMessage_GeneralErrorLabelSubLine;
		}
		if (installPackagesId[failedPackage.Key].ProductManifest.MainPackage)
		{
			ErrorInfo.SetErrorInfo(string.Format(Resources.ErrorMessage_PackagesFailedLabelHeadLine, VersionFormat.GenerateProductNameWithVersion()), null, null, string.Format(Resources.ErrorMessage_GeneralErrorLabelMessage, text), enablePreviousButton: false);
			ShowErrorPage(null);
			return false;
		}
		string arg = $"{installPackagesId[failedPackage.Key].ProductManifest.PackageData.DisplayName} \t {string.Format(Resources.ErrorMessage_GeneralErrorLabelMessage, text)} \n";
		if (installPackagesId.ContainsKey(failedPackage.Key))
		{
			packages += $"{arg}\n";
		}
		else
		{
			packages += $"{failedPackage}\n";
		}
		return true;
	}

	private void ModifyEnded(IAsyncResult ar)
	{
		bool flag = false;
		try
		{
			SetOverallProgress(0, 1);
			InstallerManager.Instance.SequenceManager.EndModify(ar);
			InstallerManager.Instance.UpdateInstallInfoAfterModify();
			flag = true;
			Dictionary<string, string> failedInstallPackages = InstallerManager.Instance.SequenceManager.FailedInstallPackages;
			Dictionary<string, string> failedUninstallationPackages = InstallerManager.Instance.SequenceManager.FailedUninstallationPackages;
			Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
			InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ProductManifest.Format == FormatType.Milestone)
				{
					dictionary.Add(installerPackageType.ID, installerPackageType);
				}
			}
			if ((failedInstallPackages != null && failedInstallPackages.Count != 0) || (failedUninstallationPackages != null && failedUninstallationPackages.Count != 0))
			{
				bool flag2 = true;
				string packages = "";
				foreach (KeyValuePair<string, string> item in failedInstallPackages)
				{
					flag2 = SetPackageDetails(dictionary, item, ref packages);
				}
				foreach (KeyValuePair<string, string> item2 in failedUninstallationPackages)
				{
					SetPackageDetails(dictionary, item2, ref packages);
				}
				if (flag2)
				{
					ErrorInfo.SetErrorInfo(string.Format(Resources.ErrorMessage_PackagesFailedModifyLabelHeadLine, VersionFormat.GenerateProductNameWithVersion()), null, null, packages, enablePreviousButton: false);
					ShowErrorPage(null);
				}
			}
			else
			{
				NextPage();
			}
		}
		catch (Exception ex)
		{
			if (!flag)
			{
				InstallerManager.Instance.UpdateInstallInfoAfterModify();
			}
			if (InstallerManager.Instance.SequenceManager.NeedReboot)
			{
				HandleReboot();
				return;
			}
			LogManager.Write("Install failed." + ex, TraceEventType.Error);
			ShowErrorPage(ex);
		}
	}

	private void ShowErrorPage(Exception ex)
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			ShowErrorDelegate method = ShowErrorPage;
			progressBarTotalProgress.Invoke(method, ex);
			return;
		}
		Wizard.NavigationPanel.Hide();
		if (ex != null)
		{
			ErrorHandler.ShowError(ex);
		}
		else
		{
			ErrorHandler.ShowError();
		}
	}

	private void NextPage()
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			SetContinueDelegate method = NextPage;
			progressBarTotalProgress.Invoke(method);
		}
		else
		{
			Wizard.NavigationPanel.Hide();
			FlowManager.TriggerEvent(null);
		}
	}

	private void WizardPageInstall_Leave(object sender, EventArgs e)
	{
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.HideNavigationButtons();
		LoadResources();
		if (InstallerManager.Instance.IsModify && !new ProcessWatcher(InstallerManager.Instance.InstallerConfig.InstallerPackages, InstallerManager.Instance.InstallerConfig.UninstallerInfo.UninstallerSequencer, InstallerManager.Instance.InstallerConfig.InstallExecuteSequence, InstallerManager.Instance.SequenceManager.PackagesToUninstall, InstallerManager.Instance.SequenceManager.PackagesToInstall).CheckWatchedProcesses(this))
		{
			InstallerManager.Instance.FormShowConfirmClosing = false;
			Wizard.btnCancel_Click(this, new CancelEventArgs());
			return;
		}
		int websitePort = 80;
		if (InstallerManager.Instance.Parameters.ContainsKey("WebSitePort"))
		{
			websitePort = Convert.ToInt32(InstallerManager.Instance.Parameters["WebSitePort"]);
		}
		InstallerManager.Instance.UpdatePackageParameters();
		InstallerManager.Instance.RemovePrerequisitesByCommandLineParameter();
		InstallerManager.Instance.SequenceManager.SetPrerequisitesToInstall(InstallerManager.Instance.PrerequisiteManager.PackagesToInstall, websitePort);
		if (InstallerManager.Instance.InstallInfo.InstalledPackages.Count != 0 || InstallerManager.Instance.InstallInfo.NotInstalledPackages.Count != 0)
		{
			InstallerManager.Instance.SkipInstalledPackages();
			InstallerManager.Instance.AddNotInstalledPackages();
		}
		if (!InstallerManager.Instance.IsModify)
		{
			InstallerManager.Instance.SequenceManager.BeginInstall(InstallationEnded, null);
		}
		else
		{
			InstallerManager.Instance.SequenceManager.BeginModify(ModifyEnded, null);
		}
	}

	public override void OnBeforeCancel()
	{
		InstallerManager.Instance.SequenceManager.Cancel();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageInstall));
		base.WizardPage = new VideoOS.Installer.UI.Framework.WizardPage();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelComponentProgress = new System.Windows.Forms.Label();
		this.progressBarComponent = new System.Windows.Forms.ProgressBar();
		this.progressBarTotalProgress = new System.Windows.Forms.ProgressBar();
		this.labelTotalProgress = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.progressBarTotalProgress);
		base.WizardPage.Controls.Add(this.labelTotalProgress);
		base.WizardPage.Controls.Add(this.progressBarComponent);
		base.WizardPage.Controls.Add(this.labelComponentProgress);
		base.WizardPage.Controls.Add(this.labelHeadline);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.ShowFromNext += new System.EventHandler(wizardPage_ShowFromNext);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Leave += new System.EventHandler(WizardPageInstall_Leave);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.labelComponentProgress, "labelComponentProgress");
		this.labelComponentProgress.BackColor = System.Drawing.Color.Transparent;
		this.labelComponentProgress.Name = "labelComponentProgress";
		componentResourceManager.ApplyResources(this.progressBarComponent, "progressBarComponent");
		this.progressBarComponent.Name = "progressBarComponent";
		componentResourceManager.ApplyResources(this.progressBarTotalProgress, "progressBarTotalProgress");
		this.progressBarTotalProgress.Name = "progressBarTotalProgress";
		componentResourceManager.ApplyResources(this.labelTotalProgress, "labelTotalProgress");
		this.labelTotalProgress.Name = "labelTotalProgress";
		base.Name = "WizardPageInstall";
		this.PageName = "InstallPage";
		base.Leave += new System.EventHandler(WizardPageInstall_Leave);
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
