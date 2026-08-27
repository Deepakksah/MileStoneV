using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPagePrerequisiteInstall : WizardPageInitialize
{
	private IContainer components;

	public WizardPagePrerequisiteInstall()
	{
		InitializeComponent();
		PageName = "PrerequisiteInstall";
	}

	public override bool OnBeforeActivate()
	{
		return InstallerManager.Instance.PrerequisiteManager.PackagesToInstall.Count != 0;
	}

	protected override void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		InstallerManager.Instance.PrerequisiteManager.OverallProgress += base.OnCurrentOverallProgress;
		InstallerManager.Instance.PrerequisiteManager.MessageChanged += base.OnMessageChanged;
		if (InstallerManager.Instance.PrerequisiteManager.PackagesToInstall.Count != 0 || InstallerManager.Instance.PrerequisiteManager.PrepareInstallationPrerequisites())
		{
			Wizard.NavigationPanel.Hide();
			int websitePort = 80;
			if (InstallerManager.Instance.Parameters.ContainsKey("WebSitePort"))
			{
				websitePort = Convert.ToInt32(InstallerManager.Instance.Parameters["WebSitePort"]);
			}
			InstallerManager.Instance.PrerequisiteManager.BeginPrerequisiteInstallation(PrerequisiteInstallEnded, null, websitePort);
		}
	}

	private void PrerequisiteInstallEnded(IAsyncResult ar)
	{
		try
		{
			SetCurrentProgress(1, 1);
			InstallerManager.Instance.PrerequisiteManager.EndPrerequisiteInstallation(ar);
		}
		catch (Exception ex)
		{
			if (InstallerManager.Instance.PrerequisiteManager.NeedReboot)
			{
				InstallerManager.Instance.SaveIntermediateInstallInfo(PageName, BreakAction.PrereqInstallContinue);
				ErrorInfo.SetErrorInfo(Resources.ErrorMessage_RestartLabelHead, ex.Message, new RestartAction(), null, enablePreviousButton: false);
				ShowErrorPage(null);
			}
			else
			{
				LogManager.Write(ex);
				ShowErrorPage(ex);
			}
			return;
		}
		ShowNextPage();
	}

	protected override void WizardPage_Leave(object sender, EventArgs e)
	{
		InstallerManager.Instance.PrerequisiteManager.OverallProgress -= base.OnCurrentOverallProgress;
		InstallerManager.Instance.PrerequisiteManager.MessageChanged -= base.OnMessageChanged;
	}

	protected override void ShowNextPage()
	{
		if (WizardPage.InvokeRequired)
		{
			SetContinueDelegate method = ShowNextPage;
			WizardPage.Invoke(method);
		}
		else
		{
			FlowManager.TriggerEvent(null);
		}
	}

	public override List<string> GetEventParameter()
	{
		return new List<string> { string.Format("{0},{1}", "InstallationTypePage", "InstallationTypePage") };
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPagePrerequisiteInstall";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
