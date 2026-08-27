using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform.Admin;

namespace VideoOS.Platform.UI;

public class HelpUserControl : ItemNodeUserControl
{
	private IContainer components;

	private PictureBox pictureBoxIcon;

	private Label labelHelpName;

	private Panel panelContent;

	public Image Image
	{
		get
		{
			return pictureBoxIcon.Image;
		}
		set
		{
			if (value.Width == 32 && value.Height == 32)
			{
				pictureBoxIcon.Image = value;
			}
			else
			{
				pictureBoxIcon.Image = new Bitmap(value, new Size(32, 32));
			}
		}
	}

	public string HelpName
	{
		get
		{
			return labelHelpName.Text;
		}
		set
		{
			labelHelpName.Text = value;
		}
	}

	public UserControl UserControl
	{
		set
		{
			panelContent.Controls.Clear();
			panelContent.Controls.Add(value);
		}
	}

	public HelpUserControl()
	{
		InitializeComponent();
	}

	public HelpUserControl(Image image, string header, string description)
	{
		InitializeComponent();
		Image = image;
		labelHelpName.Text = header;
		RichTextBox value = new RichTextBox
		{
			Location = new Point(0, 0),
			Dock = DockStyle.Fill,
			Multiline = true,
			Text = description,
			Font = new Font("Verdana", 8f, FontStyle.Regular, GraphicsUnit.Point, 0),
			BorderStyle = BorderStyle.None,
			ReadOnly = true,
			BackColor = Color.White
		};
		panelContent.Controls.Add(value);
	}

	public HelpUserControl(Image image, string header, UserControl userControl)
	{
		InitializeComponent();
		Image = image;
		labelHelpName.Text = header;
		panelContent.Controls.Add(userControl);
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
		this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
		this.labelHelpName = new System.Windows.Forms.Label();
		this.panelContent = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxIcon).BeginInit();
		base.SuspendLayout();
		this.pictureBoxIcon.Location = new System.Drawing.Point(79, 6);
		this.pictureBoxIcon.Name = "pictureBoxIcon";
		this.pictureBoxIcon.Size = new System.Drawing.Size(32, 32);
		this.pictureBoxIcon.TabIndex = 0;
		this.pictureBoxIcon.TabStop = false;
		this.labelHelpName.AutoSize = true;
		this.labelHelpName.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelHelpName.Location = new System.Drawing.Point(113, 10);
		this.labelHelpName.Name = "labelHelpName";
		this.labelHelpName.Size = new System.Drawing.Size(61, 22);
		this.labelHelpName.TabIndex = 1;
		this.labelHelpName.Text = "label1";
		this.panelContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelContent.Location = new System.Drawing.Point(114, 54);
		this.panelContent.Name = "panelContent";
		this.panelContent.Size = new System.Drawing.Size(410, 337);
		this.panelContent.TabIndex = 2;
		this.panelContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.panelContent);
		base.Controls.Add(this.labelHelpName);
		base.Controls.Add(this.pictureBoxIcon);
		base.Name = "HelpUserControl";
		base.Size = new System.Drawing.Size(556, 425);
		((System.ComponentModel.ISupportInitialize)this.pictureBoxIcon).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
