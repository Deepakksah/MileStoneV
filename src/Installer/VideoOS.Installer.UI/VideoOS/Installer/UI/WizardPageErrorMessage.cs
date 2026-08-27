using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageErrorMessage : WizardPageBase
{
	private IContainer components;

	private Label labelHeadline;

	private LinkLabel labelSubHeading;

	private Button buttonDone;

	private Button buttonAction;

	private Panel errorMessagePanel;

	public WizardPageErrorMessage()
	{
		InitializeComponent();
		labelHeadline.UseMnemonic = false;
		labelSubHeading.UseMnemonic = false;
		LoadResources();
	}

	private void LoadResources()
	{
		buttonDone.Text = Resources.Finish_ButtonClose;
	}

	private void buttonDone_Click(object sender, EventArgs e)
	{
		if (ErrorInfo.Action == null)
		{
			InstallerManager.Instance.SetupWhyUninstallParams();
			Dictionary<string, string> dictionary = GlobalData.ToDict();
			if (dictionary["Installation Context"] == "uninstall" && InstallerManager.Instance.Parameters.ContainsKey("Why Uninstalling") && (!dictionary.ContainsKey("USER_ACTION") || dictionary["USER_ACTION"] != "cancelled"))
			{
				Process.Start(string.Format("{0}?epslanguage={1}", InstallerManager.Instance.Parameters["Why Uninstalling"], InstallerManager.Instance.Parameters["LANGUAGE"]));
			}
		}
		FlowManager.TriggerEvent(null);
	}

	private void SetErrorMessageUserControl(UserControl errorMessageControl)
	{
		if (errorMessageControl == null)
		{
			throw new ArgumentNullException("errorMessageControl");
		}
		SuspendLayout();
		errorMessagePanel.Controls.Clear();
		errorMessagePanel.Visible = true;
		errorMessagePanel.Controls.Add(errorMessageControl);
		errorMessageControl.Dock = DockStyle.Fill;
		errorMessageControl.Parent = errorMessagePanel;
		ResumeLayout();
	}

	private void ClearErrorMessageUserControl()
	{
		SuspendLayout();
		errorMessagePanel.Controls.Clear();
		errorMessagePanel.Visible = false;
		ResumeLayout();
	}

	private void FillTextParameters()
	{
		labelHeadline.Text = ErrorInfo.HeadLine;
		labelSubHeading.Text = ErrorInfo.SubLine;
		new LabelTextAdaptor(labelHeadline).AdaptingFontSize();
		if (ErrorInfo.SubLineLinkText != null)
		{
			labelSubHeading.Text = ErrorInfo.SubLineLinkText.Text;
			foreach (ContentLink contentLink2 in ErrorInfo.SubLineLinkText.ContentLinks)
			{
				labelSubHeading.Links.Add(new LinkLabel.Link(contentLink2.StartIndex, contentLink2.Length, contentLink2));
			}
			labelSubHeading.LinkClicked += delegate(object s, LinkLabelLinkClickedEventArgs e)
			{
				if (e.Link.LinkData is ContentLink contentLink)
				{
					if (contentLink.Uri != null)
					{
						Process.Start(contentLink.Uri.AbsoluteUri);
					}
					contentLink.ClickAction?.Invoke();
				}
			};
		}
		if (ErrorInfo.Type == ErrorType.General && ErrorInfo.ErrorText != null)
		{
			UserControlGeneralError userControlGeneralError = new UserControlGeneralError();
			userControlGeneralError.SetErrorText(ErrorInfo.ErrorText);
			SetErrorMessageUserControl(userControlGeneralError);
		}
		else if (ErrorInfo.Type == ErrorType.MissingPrerequisites)
		{
			UserControlMissingPrerqError userControlMissingPrerqError = new UserControlMissingPrerqError();
			userControlMissingPrerqError.SetMissingPrerequisites(ErrorInfo.PrerequisitesList);
			SetErrorMessageUserControl(userControlMissingPrerqError);
		}
		else
		{
			ClearErrorMessageUserControl();
		}
		if (ErrorInfo.EnablePreviousButton)
		{
			buttonAction.Hide();
			buttonDone.Hide();
			Wizard.NavigationPanel.Show();
			Wizard.NextButton.Hide();
			Wizard.CancelButton.Text = Resources.Finish_ButtonClose;
			Wizard.BackButton.Location = Wizard.NextButton.Location;
			Wizard.ShowPreviousButton();
		}
		else if (ErrorInfo.Action != null && !string.IsNullOrEmpty(ErrorInfo.Action.ActionText))
		{
			Wizard.NavigationPanel.Hide();
			buttonAction.Show();
			buttonDone.Show();
			buttonAction.Text = ErrorInfo.Action.ActionText;
			buttonAction.Focus();
		}
		else
		{
			Wizard.NavigationPanel.Hide();
			buttonAction.Hide();
			buttonDone.Show();
			buttonDone.Focus();
		}
	}

	private void WizardPage_CloseFromBack(object sender, PageEventArgs e)
	{
		Wizard.NavigationPanel.Show();
	}

	private void buttonAction_Click(object sender, EventArgs e)
	{
		if (ErrorInfo.Action != null)
		{
			ErrorInfo.Action.ExecuteAction();
		}
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		FillTextParameters();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageErrorMessage));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.LinkLabel();
		this.buttonDone = new System.Windows.Forms.Button();
		this.buttonAction = new System.Windows.Forms.Button();
		this.errorMessagePanel = new System.Windows.Forms.Panel();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.Controls.Add(this.errorMessagePanel);
		base.WizardPage.Controls.Add(this.buttonAction);
		base.WizardPage.Controls.Add(this.buttonDone);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.FinishPage = true;
		base.WizardPage.FlowId = new System.Guid("0751a8c0-2056-4c7b-abe3-9e5e71af20cf");
		base.WizardPage.CloseFromBack += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_CloseFromBack);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		this.labelSubHeading.LinkColor = System.Drawing.Color.FromArgb(64, 141, 217);
		this.labelSubHeading.VisitedLinkColor = System.Drawing.Color.FromArgb(64, 141, 217);
		this.labelSubHeading.ActiveLinkColor = System.Drawing.Color.FromArgb(255, 64, 64);
		this.labelSubHeading.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(this.labelSubHeading, "labelSubHeading");
		this.labelSubHeading.Name = "labelSubHeading";
		componentResourceManager.ApplyResources(this.buttonDone, "buttonDone");
		this.buttonDone.Name = "buttonDone";
		this.buttonDone.UseVisualStyleBackColor = true;
		this.buttonDone.Click += new System.EventHandler(buttonDone_Click);
		componentResourceManager.ApplyResources(this.buttonAction, "buttonAction");
		this.buttonAction.Name = "buttonAction";
		this.buttonAction.UseVisualStyleBackColor = true;
		this.buttonAction.Click += new System.EventHandler(buttonAction_Click);
		componentResourceManager.ApplyResources(this.errorMessagePanel, "errorMessagePanel");
		this.errorMessagePanel.Name = "errorMessagePanel";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageErrorMessage";
		this.PageName = "ErrorMessagePage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
