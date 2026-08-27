using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.Misc;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class LogoHorizontalResizable : UserControl
{
	private IContainer components;

	private Panel pnlHeader;

	private PictureBox logoPictureBox;

	private UltraLabel lblHeaderText;

	[Browsable(true)]
	[Category("Misc")]
	[DefaultValue("")]
	[Localizable(true)]
	[Editor("System.ComponentModel.Design.MultilineStringEditor", typeof(UITypeEditor))]
	[Description("Text displayed as header text in the dialog")]
	public string HeaderText
	{
		[DebuggerStepThrough]
		get
		{
			return ((Control)(object)lblHeaderText).Text;
		}
		[DebuggerStepThrough]
		set
		{
			((Control)(object)lblHeaderText).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Misc")]
	[DefaultValue(null)]
	[Description("Logo image displayed in the dialog (recommended size 104x59)")]
	public Image Logo
	{
		[DebuggerStepThrough]
		get
		{
			return logoPictureBox.Image;
		}
		[DebuggerStepThrough]
		set
		{
			logoPictureBox.Image = value;
		}
	}

	[Browsable(true)]
	[Category("Misc")]
	[DefaultValue(null)]
	[Description("Imaged displayed in the header background (recommended size 1x59 - will be stretched)")]
	public override Image BackgroundImage
	{
		[DebuggerStepThrough]
		get
		{
			return pnlHeader.BackgroundImage;
		}
		[DebuggerStepThrough]
		set
		{
			pnlHeader.BackgroundImage = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(typeof(Color), "Black")]
	[Description("Gets or sets the text color used in the top bar.")]
	public override Color ForeColor
	{
		[DebuggerStepThrough]
		get
		{
			return ((ControlBase)lblHeaderText).Appearance.ForeColor;
		}
		[DebuggerStepThrough]
		set
		{
			((ControlBase)lblHeaderText).Appearance.ForeColor = value;
		}
	}

	public LogoHorizontalResizable()
	{
		InitializeComponent();
		if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
		{
			logoPictureBox.Image = OemResources.Instance.Manager.WizardSmallLogo;
			pnlHeader.BackgroundImage = OemResources.Instance.Manager.WizardSmallTopBar;
			((ControlBase)lblHeaderText).Appearance.ForeColor = OemResources.Instance.Manager.WizardSmallTopBarTextColor;
		}
		else
		{
			((ControlBase)lblHeaderText).Appearance.ForeColor = Color.Black;
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.LogoHorizontalResizable));
		Appearance val = new Appearance();
		this.pnlHeader = new System.Windows.Forms.Panel();
		this.logoPictureBox = new System.Windows.Forms.PictureBox();
		this.lblHeaderText = new UltraLabel();
		this.pnlHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).BeginInit();
		base.SuspendLayout();
		this.pnlHeader.BackColor = System.Drawing.Color.White;
		componentResourceManager.ApplyResources(this.pnlHeader, "pnlHeader");
		this.pnlHeader.Controls.Add(this.logoPictureBox);
		this.pnlHeader.Controls.Add((System.Windows.Forms.Control)(object)this.lblHeaderText);
		this.pnlHeader.Name = "pnlHeader";
		this.logoPictureBox.BackColor = System.Drawing.SystemColors.HotTrack;
		componentResourceManager.ApplyResources(this.logoPictureBox, "logoPictureBox");
		this.logoPictureBox.Name = "logoPictureBox";
		this.logoPictureBox.TabStop = false;
		componentResourceManager.ApplyResources(this.lblHeaderText, "lblHeaderText");
		componentResourceManager.ApplyResources(val, "appearance1");
		componentResourceManager.ApplyResources(((AppearanceBase)val).FontData, "appearance1.FontData");
		((SubObjectBase)val).ForceApplyResources = "FontData|";
		((ControlBase)this.lblHeaderText).Appearance = (AppearanceBase)(object)val;
		((ControlBase)this.lblHeaderText).BackColorInternal = System.Drawing.Color.Transparent;
		((System.Windows.Forms.Control)(object)this.lblHeaderText).Name = "lblHeaderText";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.pnlHeader);
		base.Name = "LogoHorizontalResizable";
		this.pnlHeader.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).EndInit();
		base.ResumeLayout(false);
	}
}
