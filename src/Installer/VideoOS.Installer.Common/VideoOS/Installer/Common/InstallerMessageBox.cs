using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Installer.Common;

public class InstallerMessageBox : Form
{
	private IContainer components;

	private Panel buttonsPanel;

	private Label messageLabel;

	public InstallerMessageBoxButtons Buttons{ get; set; }

	public InstallerMessageBoxIcon MessageBoxIcon{ get; set; }

	public string Message { get; set; }

	public string Title { get; set; }

	public InstallerMessageBox()
	{
		InitializeComponent();
		messageLabel.UseMnemonic = false;
	}

	private void SetButtons()
	{
		UserControl userControl = null;
		SuspendLayout();
		buttonsPanel.Controls.Clear();
		ButtonsUserControl buttonsUserControl = new ButtonsUserControl();
		if (Buttons == InstallerMessageBoxButtons.YesNo)
		{
			buttonsUserControl = new YesNoUserControl();
		}
		else if (Buttons == InstallerMessageBoxButtons.OK)
		{
			buttonsUserControl = new OKUserControl();
		}
		else if (Buttons == InstallerMessageBoxButtons.OKCancel)
		{
			buttonsUserControl = new OKCancelUserControl();
		}
		buttonsUserControl.OnButtonClicked += UserControl_ButtonClicked;
		userControl = buttonsUserControl;
		buttonsPanel.Controls.Add(userControl);
		userControl.Dock = DockStyle.Fill;
		userControl.Parent = buttonsPanel;
		ResumeLayout();
	}

	private void SetIcon()
	{
		Image image = null;
		switch (MessageBoxIcon)
		{
		case InstallerMessageBoxIcon.Warning:
			image = IconsManager.WarningIcon;
			break;
		case InstallerMessageBoxIcon.Question:
			image = IconsManager.QuestionIcon;
			break;
		case InstallerMessageBoxIcon.Exclamation:
			image = IconsManager.ExclamationIcon;
			break;
		case InstallerMessageBoxIcon.Error:
			image = IconsManager.ErrorIcon;
			break;
		}
		if (image != null)
		{
			messageLabel.Location = new Point(60, 30);
			messageLabel.MaximumSize = new Size(335, 500);
			AddPictureBox().Image = image;
		}
	}

	private void UserControl_ButtonClicked(DialogResult result)
	{
		base.DialogResult = result;
		Close();
	}

	private void InstallerMessageBox_Load(object sender, EventArgs e)
	{
		SuspendLayout();
		Text = Title ?? "";
		messageLabel.Text = Message ?? "";
		SetIcon();
		SetButtons();
		base.FormBorderStyle = FormBorderStyle.FixedDialog;
		ResumeLayout();
	}

	private PictureBox AddPictureBox()
	{
		PictureBox pictureBox = new PictureBox();
		pictureBox.Location = new Point(20, 30);
		pictureBox.Size = new Size(35, 35);
		pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
		base.Controls.Add(pictureBox);
		return pictureBox;
	}

	private void messageLabel_Resize(object sender, EventArgs e)
	{
		int num = buttonsPanel.Location.Y - messageLabel.Location.Y - 10;
		int num2 = messageLabel.Size.Height - num;
		if (num2 > 0)
		{
			base.Size = new Size(base.Size.Width, base.Size.Height + num2);
			buttonsPanel.Location = new Point(buttonsPanel.Location.X, buttonsPanel.Location.Y + num2);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Common.InstallerMessageBox));
		this.buttonsPanel = new System.Windows.Forms.Panel();
		this.messageLabel = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.buttonsPanel.BackColor = System.Drawing.SystemColors.Control;
		componentResourceManager.ApplyResources(this.buttonsPanel, "buttonsPanel");
		this.buttonsPanel.Name = "buttonsPanel";
		componentResourceManager.ApplyResources(this.messageLabel, "messageLabel");
		this.messageLabel.MaximumSize = new System.Drawing.Size(375, 500);
		this.messageLabel.Name = "messageLabel";
		this.messageLabel.Resize += new System.EventHandler(messageLabel_Resize);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.messageLabel);
		base.Controls.Add(this.buttonsPanel);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "InstallerMessageBox";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.Load += new System.EventHandler(InstallerMessageBox_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
