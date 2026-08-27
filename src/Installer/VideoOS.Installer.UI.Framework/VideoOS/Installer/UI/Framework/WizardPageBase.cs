using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VideoOS.Installer.UI.Framework;

[CLSCompliant(true)]
public class WizardPageBase : WizardPageControl
{
	private delegate void InvokeDelegate(CultureInfo culture);

	private IContainer components;

	public WizardPage WizardPage;

	public virtual bool ExecSetParametersInQuiet { get; set; }

	public override WizardPage Page => WizardPage;

	public override Wizard Wizard
	{
		get
		{
			return WizardPage.Parent as Wizard;
		}
		set
		{
			base.Wizard = value;
		}
	}

	public WizardPageBase()
	{
		InitializeComponent();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		if (Page.InvokeRequired)
		{
			InvokeDelegate method = ReloadResources;
			Page.Invoke(method, culture);
		}
		else
		{
			ReloadResourcesInternal(Page, new ComponentResourceManager(GetType()));
			Page.PerformLayout();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.Framework.WizardPageBase));
		this.WizardPage = new VideoOS.Installer.UI.Framework.WizardPage();
		base.SuspendLayout();
		this.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		this.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(this.WizardPage, "WizardPage");
		this.WizardPage.FinishPage = false;
		this.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		this.WizardPage.Name = "WizardPage";
		base.Controls.Add(this.WizardPage);
		base.Name = "WizardPageBase";
		componentResourceManager.ApplyResources(this, "$this");
		base.ResumeLayout(false);
	}
}
