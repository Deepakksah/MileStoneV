using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(false)]
[Designer(typeof(WizardPageDesigner))]
public class WizardFinalPage : WizardPageBase
{
	private Label lblFinishHint;

	private PictureBox imgWatermark;

	private Label lblDescription;

	private Label lblDescription2;

	private PictureBox imgWelcome;

	private Label lblWizardName;

	private IContainer components;

	protected override Size DefaultSize => new Size(498, 328);

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page second description Text. This description used only by welocme and final pages")]
	public string Description2
	{
		get
		{
			return lblDescription2.Text;
		}
		set
		{
			if (value != lblDescription2.Text)
			{
				lblDescription2.Text = value;
				OnDescription2Changed();
			}
		}
	}

	public WizardFinalPage()
	{
		InitializeComponent();
		base.Size = new Size(498, 328);
		base.Name = "wizFinalPage";
		base.WelcomePage = true;
		base.Title = lblWizardName.Text;
		base.Description = lblDescription.Text;
		base.HeaderImage = imgWelcome.Image;
		base.FinishPage = true;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.WizardFinalPage));
		this.imgWatermark = new System.Windows.Forms.PictureBox();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblWizardName = new System.Windows.Forms.Label();
		this.imgWelcome = new System.Windows.Forms.PictureBox();
		this.lblFinishHint = new System.Windows.Forms.Label();
		this.lblDescription2 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.imgWatermark).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.imgWelcome).BeginInit();
		base.SuspendLayout();
		this.imgWatermark.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(this.imgWatermark, "imgWatermark");
		this.imgWatermark.Name = "imgWatermark";
		this.imgWatermark.TabStop = false;
		componentResourceManager.ApplyResources(this.lblDescription, "lblDescription");
		this.lblDescription.BackColor = System.Drawing.Color.Transparent;
		this.lblDescription.Name = "lblDescription";
		componentResourceManager.ApplyResources(this.lblWizardName, "lblWizardName");
		this.lblWizardName.BackColor = System.Drawing.Color.Transparent;
		this.lblWizardName.Name = "lblWizardName";
		componentResourceManager.ApplyResources(this.imgWelcome, "imgWelcome");
		this.imgWelcome.Name = "imgWelcome";
		this.imgWelcome.TabStop = false;
		componentResourceManager.ApplyResources(this.lblFinishHint, "lblFinishHint");
		this.lblFinishHint.BackColor = System.Drawing.Color.Transparent;
		this.lblFinishHint.Name = "lblFinishHint";
		componentResourceManager.ApplyResources(this.lblDescription2, "lblDescription2");
		this.lblDescription2.Name = "lblDescription2";
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.lblDescription2);
		base.Controls.Add(this.imgWatermark);
		base.Controls.Add(this.lblDescription);
		base.Controls.Add(this.lblWizardName);
		base.Controls.Add(this.imgWelcome);
		base.Controls.Add(this.lblFinishHint);
		base.Name = "WizardFinalPage";
		componentResourceManager.ApplyResources(this, "$this");
		((System.ComponentModel.ISupportInitialize)this.imgWatermark).EndInit();
		((System.ComponentModel.ISupportInitialize)this.imgWelcome).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	protected override void OnImageListChanged()
	{
		if (base.ImageList != null && base.ImageIndex >= 0 && base.ImageIndex < base.ImageList.Images.Count)
		{
			imgWatermark.Image = base.ImageList.Images[base.ImageIndex];
		}
	}

	protected override void OnImageIndexChanged()
	{
		if (base.ImageList != null && base.ImageIndex >= 0 && base.ImageIndex < base.ImageList.Images.Count)
		{
			imgWatermark.Image = base.ImageList.Images[base.ImageIndex];
		}
	}

	protected override void OnHeaderImageChanged()
	{
		imgWelcome.Image = base.HeaderImage;
	}

	protected override void OnTitleChanged()
	{
		lblWizardName.Text = base.Title;
	}

	protected override void OnDescriptionChanged()
	{
		lblDescription.Text = base.Description;
	}

	protected virtual void OnDescription2Changed()
	{
	}

	protected override void OnLoad(EventArgs e)
	{
		OnImageListChanged();
		OnHeaderImageChanged();
		base.OnLoad(e);
	}
}
