using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Sequencer;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageUninstallation : WizardPageBase
{
	private delegate void SetContinueDelegate();

	private delegate void SetOverallProgressDelegate(int value, int maximum);

	private delegate void SetMessageDelegate(string message);

	private delegate void ShowErrorDelegate(Exception ex, bool deleteFiles);

	private IContainer components;

	private ProgressBar progressBarTotalProgress;

	private Label labelTotalProgress;

	private Label labelHeadline;

	private Button buttonDone;

	public WizardPageUninstallation(InstallInfo installInfo)
	{
		InitializeComponent();
		labelHeadline.UseMnemonic = false;
		labelTotalProgress.UseMnemonic = false;
		buttonDone.Hide();
	}

	private void SetUpSequenceManager()
	{
		List<string> list = new List<string>();
		if (InstallerManager.Instance.InstallInfo.InstalledPackages != null)
		{
			foreach (PackageRefType installedPackage in InstallerManager.Instance.InstallInfo.InstalledPackages)
			{
				list.Add(installedPackage.ID);
			}
		}
		else
		{
			LogManager.Write("InstallInfo.InstalledPackages is empty");
		}
		Dictionary<string, InstallerPackageType> dictionary = InstallerManager.Instance.InstallerConfig.InstallerPackages.ToDictionary((InstallerPackageType i) => i.ID);
		if (InstallerManager.Instance.InstallInfo.NotInstalledPackages != null)
		{
			foreach (PackageRefType notInstalledPackage in InstallerManager.Instance.InstallInfo.NotInstalledPackages)
			{
				InstallerPackageType installerPackageType = dictionary[notInstalledPackage.ID];
				string productName = null;
				ComponentInstalledHelper.InstallationState installationState = ComponentInstalledHelper.ComponentIsInstalled(installerPackageType.ProductManifest.PackageData.ProductInfo.ProductCode, Path.Combine(InstallerManager.Instance.CurrentDirectory, installerPackageType.ProductManifest.PackageData.PrimaryFile[0]), ref productName);
				if (installationState == ComponentInstalledHelper.InstallationState.InstalledByThisSource || installerPackageType.UninstallConditions == null || (installerPackageType.UninstallConditions.AllowOtherSource && installationState == ComponentInstalledHelper.InstallationState.InstalledByOtherSource))
				{
					if (productName != null && productName.Contains(InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher))
					{
						productName = productName.Replace($"{InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher} ", "");
						installerPackageType.ProductManifest.PackageData.DisplayName = productName;
					}
					list.Add(notInstalledPackage.ID);
				}
			}
		}
		InstallerManager.Instance.UpdateParametersFromInstallInfoAndServerConfigurator();
		InstallerManager.Instance.UpdatePackageParameters();
		InstallerManager.Instance.CreateSequenceManager();
		InstallerManager.Instance.SequenceManager.PackagesToUninstall = list;
		foreach (string item in list)
		{
			LogManager.Write($"Package to uninstall {item}.");
		}
		InstallerManager.Instance.SequenceManagerWrapper.TaskSequencer.OverallProgress += OnOverallProgress;
		InstallerManager.Instance.SequenceManagerWrapper.TaskSequencer.MessageChanged += OnMessageChanged;
	}

	private void LoadResources()
	{
		labelTotalProgress.Text = Resources.UninstallWelcome_PrepareUninstall;
		labelHeadline.Text = string.Format(Resources.Uninstalling_LabelHeadLine, VersionFormat.GenerateProductNameWithVersionUninstallMode());
	}

	public override void ReloadResources(CultureInfo culture)
	{
		Resources.Culture = culture;
		base.ReloadResources(culture);
		LoadResources();
	}

	private void WizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.HideNavigationButtons();
	}

	private void UninstallationEnded(IAsyncResult ar)
	{
		try
		{
			InstallerManager.Instance.RemoveInstallerShortcut();
			SetOverallProgress(0, 1);
			InstallerManager.Instance.SequenceManager.EndUninstall(ar);
			RenameCancelButton();
			InstallerManager.Instance.VerifyCurrentMachineState(doContinue: false);
			InstallerManager.Instance.UpdateInstallerInfo();
			ErrorInfo.SetErrorInfo(Resources.Uninstalling_CompletedHeadline, string.Format(Resources.Uninstalling_CompletedMessage, VersionFormat.GenerateProductNameWithVersionUninstallMode()), null, "", enablePreviousButton: false);
			ShowMessagePage(null, deleteFiles: true);
		}
		catch (Exception ex)
		{
			if (InstallerManager.Instance.SequenceManager.NeedReboot && InstallerManager.Instance.SequenceManager.NotExecutedUninstallPackages.Count != 0)
			{
				InstallerManager.Instance.SaveIntermediateInstallInfo(PageName, BreakAction.UninstallContinue);
				ErrorInfo.SetErrorInfo(Resources.ErrorMessage_RestartLabelHead, Resources.ErrorMessage_RestartLabelSubLine, new RestartAction(), null, enablePreviousButton: false);
				ShowMessagePage(null, deleteFiles: false);
			}
			else
			{
				LogManager.Write("Install failed." + ex, TraceEventType.Error);
				ShowMessagePage(ex, deleteFiles: false);
			}
		}
	}

	private void ShowMessagePage(Exception ex, bool deleteFiles)
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			ShowErrorDelegate method = ShowMessagePage;
			progressBarTotalProgress.Invoke(method, ex, deleteFiles);
			return;
		}
		Wizard.NavigationPanel.Hide();
		if (ex != null)
		{
			ErrorHandler.ShowUninstallationError(ex);
			return;
		}
		if (deleteFiles)
		{
			InstallerManager.Instance.DeleteFiles(InstallerManager.Instance.CurrentDirectory);
		}
		ErrorHandler.ShowError();
	}

	private void RenameCancelButton()
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			SetContinueDelegate method = RenameCancelButton;
			progressBarTotalProgress.Invoke(method);
		}
		else
		{
			buttonDone.Show();
		}
	}

	private void SetOverallProgress(int value, int maximum)
	{
		if (progressBarTotalProgress.InvokeRequired)
		{
			SetOverallProgressDelegate method = SetOverallProgress;
			progressBarTotalProgress.Invoke(method, value, maximum);
			return;
		}
		if (value > maximum)
		{
			value = maximum - 1;
		}
		progressBarTotalProgress.Maximum = maximum;
		progressBarTotalProgress.Value = value;
	}

	private void SetMessage(string message)
	{
		if (message != null)
		{
			if (progressBarTotalProgress.InvokeRequired)
			{
				SetMessageDelegate method = SetMessage;
				progressBarTotalProgress.Invoke(method, message);
			}
			else
			{
				message = message.Replace("'", "");
				labelTotalProgress.Text = message;
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

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		LoadResources();
		SetUpSequenceManager();
		if (!new ProcessWatcher(InstallerManager.Instance.InstallerConfig.InstallerPackages, InstallerManager.Instance.InstallerConfig.UninstallerInfo.UninstallerSequencer, InstallerManager.Instance.SequenceManager.PackagesToUninstall).CheckWatchedProcesses(this))
		{
			InstallerManager.Instance.FormShowConfirmClosing = false;
			Wizard.btnCancel_Click(this, new CancelEventArgs());
		}
		else
		{
			Wizard.NavigationPanel.Hide();
			LogManager.Write("Uninstallation started.");
			InstallerManager.Instance.SequenceManager.BeginUninstall(UninstallationEnded, null);
		}
	}

	private void buttonDone_Click(object sender, EventArgs e)
	{
		FlowManager.TriggerEvent(null);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageUninstallation));
		this.progressBarTotalProgress = new System.Windows.Forms.ProgressBar();
		this.labelTotalProgress = new System.Windows.Forms.Label();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.buttonDone = new System.Windows.Forms.Button();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.buttonDone);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.progressBarTotalProgress);
		base.WizardPage.Controls.Add(this.labelTotalProgress);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("05f5d82e-dcb4-4ebb-851e-f90ff6025529");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.ShowFromNext += new System.EventHandler(WizardPage_ShowFromNext);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this.progressBarTotalProgress, "progressBarTotalProgress");
		this.progressBarTotalProgress.Name = "progressBarTotalProgress";
		componentResourceManager.ApplyResources(this.labelTotalProgress, "labelTotalProgress");
		this.labelTotalProgress.Name = "labelTotalProgress";
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.buttonDone, "buttonDone");
		this.buttonDone.Name = "buttonDone";
		this.buttonDone.Click += new System.EventHandler(buttonDone_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageUninstallation";
		this.PageName = "UninstallationPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
