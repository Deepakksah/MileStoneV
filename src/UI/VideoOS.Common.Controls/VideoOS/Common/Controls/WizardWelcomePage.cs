using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(false)]
[Designer(typeof(WizardWelcomePageDesigner))]
public class WizardWelcomePage : WizardPageBase
{
	private CheckBox chkDontShow;

	private Label lblDescription2;

	private Label lblDescription;

	private Label lblHint;

	private Label lblWizardName;

	private PictureBox imgWatermark;

	private PictureBox imgWelcome;

	private IContainer components;

	protected override Size DefaultSize => new Size(498, 328);

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Is checkbox \"Don't show...\" checked.")]
	[DefaultValue(false)]
	public bool DontShow
	{
		get
		{
			return chkDontShow.Checked;
		}
		set
		{
			if (value != chkDontShow.Checked)
			{
				chkDontShow.Checked = value;
				OnDontShowChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page second description Text. This description used only by welocme and final pages")]
	public string Description2
	{
		get
		{
			if (lblDescription2 == null)
			{
				return string.Empty;
			}
			return lblDescription2.Text;
		}
		set
		{
			if (lblDescription2 != null && value != lblDescription2.Text)
			{
				lblDescription2.Text = value;
				OnDescription2Changed();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page title Font")]
	public Font TitleFont
	{
		get
		{
			if (lblWizardName == null)
			{
				return new Font(FontFamily.GenericSansSerif, 8f);
			}
			return lblWizardName.Font;
		}
		set
		{
			if (lblWizardName != null && value != lblWizardName.Font)
			{
				lblWizardName.Font = value;
				OnDescription2Changed();
			}
		}
	}

	public WizardWelcomePage()
	{
		InitializeComponent();
		base.Size = new Size(498, 328);
		base.Name = "wizWelcomePage";
		base.WelcomePage = true;
		base.Title = lblWizardName.Text;
		base.Description = lblDescription.Text;
		base.HeaderImage = imgWelcome.Image;
		lblDescription2.SizeChanged += OnLabelSizeChanged;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.WizardWelcomePage));
		this.chkDontShow = new System.Windows.Forms.CheckBox();
		this.lblDescription2 = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblHint = new System.Windows.Forms.Label();
		this.lblWizardName = new System.Windows.Forms.Label();
		this.imgWatermark = new System.Windows.Forms.PictureBox();
		this.imgWelcome = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.imgWatermark).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.imgWelcome).BeginInit();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.chkDontShow, "chkDontShow");
		this.chkDontShow.BackColor = System.Drawing.Color.Transparent;
		this.chkDontShow.Name = "chkDontShow";
		this.chkDontShow.UseVisualStyleBackColor = false;
		componentResourceManager.ApplyResources(this.lblDescription2, "lblDescription2");
		this.lblDescription2.BackColor = System.Drawing.Color.Transparent;
		this.lblDescription2.Name = "lblDescription2";
		componentResourceManager.ApplyResources(this.lblDescription, "lblDescription");
		this.lblDescription.BackColor = System.Drawing.Color.Transparent;
		this.lblDescription.Name = "lblDescription";
		componentResourceManager.ApplyResources(this.lblHint, "lblHint");
		this.lblHint.BackColor = System.Drawing.Color.Transparent;
		this.lblHint.Name = "lblHint";
		componentResourceManager.ApplyResources(this.lblWizardName, "lblWizardName");
		this.lblWizardName.BackColor = System.Drawing.Color.Transparent;
		this.lblWizardName.Name = "lblWizardName";
		this.imgWatermark.BackColor = System.Drawing.SystemColors.Control;
		componentResourceManager.ApplyResources(this.imgWatermark, "imgWatermark");
		this.imgWatermark.Name = "imgWatermark";
		this.imgWatermark.TabStop = false;
		this.imgWelcome.BackColor = System.Drawing.Color.White;
		componentResourceManager.ApplyResources(this.imgWelcome, "imgWelcome");
		this.imgWelcome.Name = "imgWelcome";
		this.imgWelcome.TabStop = false;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.imgWatermark);
		base.Controls.Add(this.imgWelcome);
		base.Controls.Add(this.chkDontShow);
		base.Controls.Add(this.lblDescription2);
		base.Controls.Add(this.lblDescription);
		base.Controls.Add(this.lblHint);
		base.Controls.Add(this.lblWizardName);
		base.Name = "WizardWelcomePage";
		componentResourceManager.ApplyResources(this, "$this");
		((System.ComponentModel.ISupportInitialize)this.imgWatermark).EndInit();
		((System.ComponentModel.ISupportInitialize)this.imgWelcome).EndInit();
		base.ResumeLayout(false);
	}

	private void OnLabelSizeChanged(object sender, EventArgs e)
	{
		OnResize(e);
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (Description2 != null && Description2.Length > 0)
		{
			Bitmap bitmap = new Bitmap(lblDescription2.Width, lblDescription2.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			SizeF sizeF = graphics.MeasureString(format: new StringFormat
			{
				Trimming = StringTrimming.Word
			}, text: Description2, font: lblDescription2.Font, width: lblDescription2.Width);
			lblDescription2.Height = (int)sizeF.Height + 16;
			lblHint.Top = lblDescription2.Bottom + 8;
			graphics.Dispose();
			bitmap.Dispose();
		}
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

	protected override void OnLoad(EventArgs e)
	{
		OnImageListChanged();
		OnHeaderImageChanged();
		base.OnLoad(e);
	}

	protected virtual void OnDescription2Changed()
	{
		OnResize(EventArgs.Empty);
	}

	protected virtual void OnDontShowChanged()
	{
	}
}
