using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class LogoVerticalResizable : UserControl
{
	private IContainer components;

	private PictureBox PictureBoxTop;

	private PictureBox PictureBoxBottom;

	private PictureBox PictureBoxMiddle;

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Gets/Sets top image of the logo.")]
	public Image LogoTop
	{
		get
		{
			return PictureBoxTop.Image;
		}
		set
		{
			PictureBoxTop.Image = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Gets/Sets middle image of the logo. This image is stretched when the control is rezised.")]
	public Image LogoMiddle
	{
		get
		{
			return PictureBoxMiddle.BackgroundImage;
		}
		set
		{
			PictureBoxMiddle.BackgroundImage = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Gets/Sets bottom image of the logo.")]
	public Image LogoBottom
	{
		get
		{
			return PictureBoxBottom.Image;
		}
		set
		{
			PictureBoxBottom.Image = value;
		}
	}

	public LogoVerticalResizable()
	{
		InitializeComponent();
		if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
		{
			PictureBoxTop.Image = OemResources.Instance.Manager.WizardMainLogoTop;
			PictureBoxMiddle.BackgroundImage = OemResources.Instance.Manager.WizardMainLogoMiddle;
			PictureBoxBottom.Image = OemResources.Instance.Manager.WizardMainLogoBottom;
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

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		this.PictureBoxTop = new System.Windows.Forms.PictureBox();
		this.PictureBoxBottom = new System.Windows.Forms.PictureBox();
		this.PictureBoxMiddle = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.PictureBoxTop).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBoxBottom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBoxMiddle).BeginInit();
		base.SuspendLayout();
		this.PictureBoxTop.BackColor = System.Drawing.SystemColors.HotTrack;
		this.PictureBoxTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.PictureBoxTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.PictureBoxTop.Location = new System.Drawing.Point(0, 0);
		this.PictureBoxTop.Name = "PictureBoxTop";
		this.PictureBoxTop.Size = new System.Drawing.Size(160, 168);
		this.PictureBoxTop.TabIndex = 8;
		this.PictureBoxTop.TabStop = false;
		this.PictureBoxBottom.BackColor = System.Drawing.SystemColors.HotTrack;
		this.PictureBoxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PictureBoxBottom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.PictureBoxBottom.Location = new System.Drawing.Point(0, 212);
		this.PictureBoxBottom.Name = "PictureBoxBottom";
		this.PictureBoxBottom.Size = new System.Drawing.Size(160, 134);
		this.PictureBoxBottom.TabIndex = 22;
		this.PictureBoxBottom.TabStop = false;
		this.PictureBoxMiddle.BackColor = System.Drawing.SystemColors.HotTrack;
		this.PictureBoxMiddle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.PictureBoxMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PictureBoxMiddle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.PictureBoxMiddle.Location = new System.Drawing.Point(0, 168);
		this.PictureBoxMiddle.Name = "PictureBoxMiddle";
		this.PictureBoxMiddle.Size = new System.Drawing.Size(160, 44);
		this.PictureBoxMiddle.TabIndex = 23;
		this.PictureBoxMiddle.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.PictureBoxMiddle);
		base.Controls.Add(this.PictureBoxBottom);
		base.Controls.Add(this.PictureBoxTop);
		this.MinimumSize = new System.Drawing.Size(160, 303);
		base.Name = "LogoVerticalResizable";
		base.Size = new System.Drawing.Size(160, 346);
		((System.ComponentModel.ISupportInitialize)this.PictureBoxTop).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBoxBottom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PictureBoxMiddle).EndInit();
		base.ResumeLayout(false);
	}
}
