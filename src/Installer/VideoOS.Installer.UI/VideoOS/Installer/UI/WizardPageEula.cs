using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageEula : WizardPageBase
{
	private delegate void ShowEulaNoteDelegate();

	private IContainer components;

	private CheckBox checkBoxAccept;

	private Label labelHeadline;

	private RichTextBox textBoxEula;

	[DllImport("user32.dll")]
	public static extern long HideCaret(IntPtr hwnd);

	public WizardPageEula(int ceip)
	{
		InitializeComponent();
		LoadResources();
	}

	private void LoadResources()
	{
		labelHeadline.Text = string.Format(Resources.Eula_LabelHeadLine, InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher);
		checkBoxAccept.Text = Resources.Eula_CheckBoxAccepted;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		LoadResources();
		RefreshEula();
	}

	private void checkBoxAccept_CheckedChanged(object sender, EventArgs e)
	{
		if (sender is CheckBox checkBox)
		{
			Wizard.NextEnabled = checkBox.Checked;
		}
	}

	private void wizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.NextButton.Visible = true;
		Wizard.NextEnabled = checkBoxAccept.Checked;
		Wizard.NavigationPanel.Show();
	}

	private void textBoxEula_Click(object sender, EventArgs e)
	{
		HideCaret(textBoxEula.Handle);
	}

	private void TextBoxEula_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		if (Uri.TryCreate(e.LinkText, UriKind.Absolute, out var result))
		{
			Process.Start(result.ToString());
		}
	}

	private void WizardPage_Leave(object sender, EventArgs e)
	{
	}

	public override List<string> GetEventParameter()
	{
		return new List<string> { string.Format("{0},{1}", "PrerequisiteInstall", "PrerequisiteInstall") };
	}

	public override string OnTriggerEvent()
	{
		if (InstallerManager.Instance.InstallerConfig.InstallerPrerequisites != null)
		{
			try
			{
				if (InstallerManager.Instance.PrerequisiteManager.PackagesToInstall != null && InstallerManager.Instance.PrerequisiteManager.PackagesToInstall.Count != 0)
				{
					return "PrerequisiteInstall";
				}
				return null;
			}
			catch (Exception exception)
			{
				ErrorHandler.ShowError(exception);
			}
		}
		return null;
	}

	private void WizardPage_Enter(object sender, EventArgs e)
	{
		Focus();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		if (InstallerManager.Instance.InstallerConfig.EulaNoteProvider != null)
		{
			try
			{
				(Page.Parent as Wizard).NextEnabled = checkBoxAccept.Checked;
				RefreshEula();
			}
			catch (FileNotFoundException exception)
			{
				ErrorHandler.ShowError(exception);
				LogManager.Write(exception);
			}
		}
	}

	private void RefreshEula()
	{
		if (textBoxEula.InvokeRequired)
		{
			ShowEulaNoteDelegate method = RefreshEula;
			textBoxEula.Invoke(method);
			return;
		}
		Directory.SetCurrentDirectory(InstallerManager.Instance.CurrentDirectory);
		string text = (InstallerManager.Instance.Parameters.ContainsKey("LANGUAGE") ? InstallerManager.Instance.Parameters["LANGUAGE"] : "en-US");
		string text2;
		if (InstallerManager.Instance.InstallerConfig.EulaNoteProvider != null && !string.IsNullOrEmpty(InstallerManager.Instance.InstallerConfig.EulaNoteProvider.Filename))
		{
			text2 = InstallerManager.Instance.InstallerConfig.EulaNoteProvider.Filename;
			if (text2.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) != -1)
			{
				text2 = string.Format(text2, text);
				if (!File.Exists(text2))
				{
					text2 = string.Format(InstallerManager.Instance.InstallerConfig.EulaNoteProvider.Filename, "en-US");
				}
			}
		}
		else
		{
			List<string> list = new List<string>(Directory.GetFiles(InstallerManager.Instance.CurrentDirectory, "*.rtf"));
			list.AddRange(Directory.GetFiles(InstallerManager.Instance.CurrentDirectory, "*.RTF"));
			if (list.Count == 0)
			{
				LogManager.Write("No Eulas found in '.rtf' format. Please include it in the build.");
				throw new FileLoadException("Eula file is missing", "*.rtf");
			}
			text2 = FindRtfByLanguage(list, text);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = FindRtfByLanguage(list, "en-US");
				if (string.IsNullOrEmpty(text2))
				{
					text2 = FindRtfByLanguage(list, "");
				}
			}
		}
		textBoxEula.LoadFile(text2, RichTextBoxStreamType.RichText);
	}

	private string FindRtfByLanguage(List<string> rtfs, string langCodeToUse)
	{
		foreach (string rtf in rtfs)
		{
			string fileName = Path.GetFileName(rtf);
			if (!string.IsNullOrEmpty(fileName) && fileName.Contains(langCodeToUse))
			{
				return fileName;
			}
		}
		return null;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageEula));
		this.checkBoxAccept = new System.Windows.Forms.CheckBox();
		this.textBoxEula = new System.Windows.Forms.RichTextBox();
		this.labelHeadline = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.checkBoxAccept);
		base.WizardPage.Controls.Add(this.textBoxEula);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("ffe83b23-6abd-4504-b175-c6389bead565");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.ShowFromNext += new System.EventHandler(wizardPage_ShowFromNext);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Enter += new System.EventHandler(WizardPage_Enter);
		base.WizardPage.Leave += new System.EventHandler(WizardPage_Leave);
		componentResourceManager.ApplyResources(this.checkBoxAccept, "checkBoxAccept");
		this.checkBoxAccept.Name = "checkBoxAccept";
		this.checkBoxAccept.UseVisualStyleBackColor = true;
		this.checkBoxAccept.CheckedChanged += new System.EventHandler(checkBoxAccept_CheckedChanged);
		componentResourceManager.ApplyResources(this.textBoxEula, "textBoxEula");
		this.textBoxEula.BackColor = System.Drawing.SystemColors.Window;
		this.textBoxEula.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.textBoxEula.DetectUrls = false;
		this.textBoxEula.Name = "textBoxEula";
		this.textBoxEula.ReadOnly = true;
		this.textBoxEula.Click += new System.EventHandler(textBoxEula_Click);
		this.textBoxEula.Enter += new System.EventHandler(textBoxEula_Click);
		this.textBoxEula.MouseDown += new System.Windows.Forms.MouseEventHandler(textBoxEula_Click);
		this.textBoxEula.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(TextBoxEula_LinkClicked);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
		componentResourceManager.ApplyResources(this, "$this");
		base.Name = "WizardPageEula";
		this.PageName = "EulaPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
