using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Sequencer;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageInitialize : WizardPageBase
{
	private delegate void SetCurrentProgressDelegate(int value, int maximum);

	private delegate void SetActionDelegate(string action);

	private delegate void SetMessageDelegate(string message);

	private delegate void ShowErrorDelegate(Exception ex);

	protected delegate void SetContinueDelegate();

	protected delegate void SetErrorMessageDelegate(Exception ex);

	private bool _error;

	private IContainer components;

	private Label labelHeadline;

	private ProgressBar progressBarProgress;

	private Label labelMessage;

	private TextBox textBox6;

	private TextBox textBox5;

	private TextBox textBox4;

	private TextBox textBox3;

	private TextBox textBox2;

	private TextBox textBox1;

	public WizardPageInitialize()
	{
		InitializeComponent();
		LoadResources();
		InstallerManager.Instance.CreatePrerequisiteManager();
	}

	protected void LoadResources()
	{
		labelHeadline.Text = string.Format(Resources.Initialize_LabelHeadLine);
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	protected void OnMessageChanged(object sender, MessageEventArgs args)
	{
		SetMessage(args.Message);
	}

	protected void SetMessage(string message)
	{
		if (message != null)
		{
			if (progressBarProgress.InvokeRequired)
			{
				SetMessageDelegate method = SetMessage;
				progressBarProgress.Invoke(method, message);
			}
			else
			{
				message = message.Replace("'", "");
				labelMessage.Text = message;
			}
		}
	}

	protected void OnCurrentOverallProgress(object sender, ProgressEventArgs args)
	{
		SetCurrentProgress(args.Value, args.Maximum);
	}

	protected void SetCurrentProgress(int value, int maximum)
	{
		if (progressBarProgress.InvokeRequired)
		{
			SetCurrentProgressDelegate method = SetCurrentProgress;
			progressBarProgress.Invoke(method, value, maximum);
		}
		else
		{
			progressBarProgress.Maximum = maximum;
			progressBarProgress.Value = value;
		}
	}

	protected void PrerequisiteCheckEnded(IAsyncResult ar)
	{
		try
		{
			SetCurrentProgress(1, 1);
			InstallerManager.Instance.PrerequisiteManager.EndPrepareInstallation(ar);
		}
		catch (Exception ex)
		{
			LogManager.Write(ex);
			ShowErrorPage(ex);
			return;
		}
		FinalizePrerequisiteCheck();
	}

	private void FinalizePrerequisiteCheck()
	{
		_error = ErrorCheck();
		if (_error)
		{
			ShowErrorPage(null);
		}
		else
		{
			ShowNextPage();
		}
	}

	private void ShowGeneralErrorPage(Exception ex)
	{
		if (progressBarProgress.InvokeRequired)
		{
			ShowErrorDelegate method = ShowGeneralErrorPage;
			progressBarProgress.Invoke(method, ex);
		}
		else
		{
			Wizard.NavigationPanel.Hide();
			ErrorHandler.ShowError(ex);
		}
	}

	protected virtual void ShowNextPage()
	{
		if (progressBarProgress.InvokeRequired)
		{
			SetContinueDelegate method = ShowNextPage;
			progressBarProgress.Invoke(method);
		}
		else
		{
			Wizard.NavigationPanel.Show();
			Wizard.ShowNextButton();
			FlowManager.Next();
		}
	}

	private bool ErrorCheck()
	{
		Dictionary<string, string> packagesCannotInstall = InstallerManager.Instance.PrerequisiteManager.PackagesCannotInstall;
		if (packagesCannotInstall.Count != 0)
		{
			string arg = null;
			bool flag = true;
			foreach (KeyValuePair<string, string> item in packagesCannotInstall)
			{
				if (flag)
				{
					arg = "\n\n" + item.Key;
					flag = false;
				}
				else
				{
					arg = $"{arg} \n{item.Key} ";
				}
			}
			ErrorInfo.SetMissingPrereqErrorInfo(Resources.ErrorMessage_PackagesRequiredLabelHeadLine, "", null, packagesCannotInstall, enablePreviousButton: false);
			return true;
		}
		try
		{
			InstallerManager.Instance.GetFreeSpace(null);
		}
		catch (Win32Exception exception)
		{
			ErrorHandler.ShowError(exception);
		}
		if (InstallerManager.Instance.IsEnoughOfSpaceOnDrive() != 0.0)
		{
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_NotEnoughSpaceHeadLine, string.Format(Resources.ErrorMessage_NotEnoughSpaceSubLine, VersionFormat.GenerateProductNameWithVersion(), InstallerManager.Instance.GetRequiredSpace(), InstallerManager.Instance.SpaceAvailableOnDrive), null, null, enablePreviousButton: true);
			return true;
		}
		return false;
	}

	protected void ShowErrorPage(Exception ex)
	{
		if (progressBarProgress.InvokeRequired)
		{
			SetErrorMessageDelegate method = ShowErrorPage;
			progressBarProgress.Invoke(method, ex);
			return;
		}
		Environment.ExitCode = 1;
		if (ex == null)
		{
			ErrorHandler.ShowError();
		}
		else
		{
			ErrorHandler.ShowError(ex);
		}
	}

	protected virtual void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		if (!new ProcessWatcher(InstallerManager.Instance.InstallerConfig.InstallerPackages, InstallerManager.Instance.InstallerConfig.UninstallerInfo.UninstallerSequencer, InstallerManager.Instance.InstallerConfig.InstallExecuteSequence).CheckWatchedProcesses(this))
		{
			InstallerManager.Instance.FormShowConfirmClosing = false;
			Wizard.btnCancel_Click(this, new CancelEventArgs());
			return;
		}
		InstallerManager.Instance.PrerequisiteManager.OverallProgress += OnCurrentOverallProgress;
		InstallerManager.Instance.PrerequisiteManager.MessageChanged += OnMessageChanged;
		Wizard.NavigationPanel.Hide();
		InstallerManager.Instance.PrerequisiteManager.BeginPrepareInstallation(PrerequisiteCheckEnded, null);
	}

	protected virtual void WizardPage_Leave(object sender, EventArgs e)
	{
		InstallerManager.Instance.PrerequisiteManager.OverallProgress -= OnCurrentOverallProgress;
		InstallerManager.Instance.PrerequisiteManager.MessageChanged -= OnMessageChanged;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageInitialize));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.progressBarProgress = new System.Windows.Forms.ProgressBar();
		this.labelMessage = new System.Windows.Forms.Label();
		this.textBox6 = new System.Windows.Forms.TextBox();
		this.textBox5 = new System.Windows.Forms.TextBox();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.textBox1 = new System.Windows.Forms.TextBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.Controls.Add(this.labelMessage);
		base.WizardPage.Controls.Add(this.progressBarProgress);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Leave += new System.EventHandler(WizardPage_Leave);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("2BB3121E-77ED-439E-937A-B85006028D67");
		base.WizardPage.Name = "WizardPage";
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.progressBarProgress, "progressBarProgress");
		this.progressBarProgress.Name = "progressBarProgress";
		componentResourceManager.ApplyResources(this.labelMessage, "labelMessage");
		this.labelMessage.Name = "labelMessage";
		this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox6, "textBox6");
		this.textBox6.Name = "textBox6";
		this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox5, "textBox5");
		this.textBox5.Name = "textBox5";
		this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox4, "textBox4");
		this.textBox4.Name = "textBox4";
		this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox3, "textBox3");
		this.textBox3.Name = "textBox3";
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox2, "textBox2");
		this.textBox2.Name = "textBox2";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		componentResourceManager.ApplyResources(this.textBox1, "textBox1");
		this.textBox1.Name = "textBox1";
		base.Controls.Add(this.textBox6);
		base.Controls.Add(this.textBox5);
		base.Controls.Add(this.textBox4);
		base.Controls.Add(this.textBox3);
		base.Controls.Add(this.textBox2);
		base.Controls.Add(this.textBox1);
		base.Name = "WizardPageInitialize";
		this.PageName = "InitializePage";
		base.Controls.SetChildIndex(this.textBox1, 0);
		base.Controls.SetChildIndex(this.textBox2, 0);
		base.Controls.SetChildIndex(this.textBox3, 0);
		base.Controls.SetChildIndex(this.textBox4, 0);
		base.Controls.SetChildIndex(this.textBox5, 0);
		base.Controls.SetChildIndex(this.textBox6, 0);
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
