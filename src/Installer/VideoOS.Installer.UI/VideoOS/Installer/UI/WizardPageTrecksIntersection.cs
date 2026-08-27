using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageTrecksIntersection : WizardPageBase
{
	private UserControlTrecks _trecksUserControl;

	private string _treckKey;

	private IContainer components;

	private Label labelHeadline;

	private Panel panel1;

	public WizardPageTrecksIntersection()
	{
		InitializeComponent();
		_trecksUserControl = new UserControlTrecks();
		_trecksUserControl.ButtonClicked += _trecksUserControl_ButtonClicked;
		_trecksUserControl.Dock = DockStyle.Fill;
		panel1.Controls.Add(_trecksUserControl);
		LoadResources();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void _trecksUserControl_ButtonClicked(object sender, TreckChangedEventArgs e)
	{
		_treckKey = e.TreckKey;
		if (_treckKey == "Custom")
		{
			GlobalData.SetValue("INSTALLATION_MODE", "CustomMode", "_trecksUserControl_ButtonClicked()");
		}
		else
		{
			GlobalData.SetValue("INSTALLATION_MODE", _treckKey, "_trecksUserControl_ButtonClicked()");
		}
		if (Wizard.OnButtonNext != null)
		{
			Wizard.OnButtonNext(Wizard);
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (_treckKey != null)
		{
			dictionary.Add("TRECK_ID", _treckKey);
		}
		return dictionary;
	}

	private void LoadResources()
	{
		labelHeadline.Text = Resources.TrecksInterssection_HeadLine;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.NavigationPanel.Hide();
		if (!_trecksUserControl.IsPopulated)
		{
			_trecksUserControl.DrawLayout();
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
		this.labelHeadline = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.panel1);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelHeadline.AutoSize = true;
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(35, 47);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(279, 25);
		this.labelHeadline.TabIndex = 1;
		this.labelHeadline.Text = "Select the installation option";
		this.panel1.Location = new System.Drawing.Point(35, 103);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(540, 324);
		this.panel1.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageTrecksIntersection";
		this.PageName = "TreckIntersectionPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
