using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Server.Service.TrayController.Resources;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Service.TrayController;

public class AboutForm : Form
{
	private IContainer components;

	private PictureBox AppPicture;

	private Label lblProductName;

	private Label lblProductVersion;

	private Label lblBuildNumber;

	private Button btnOk;

	public AboutForm()
	{
		InitializeComponent();
		try
		{
			Cursor = Cursors.WaitCursor;
			Text = VideoOS.Server.Service.TrayController.Resources.Text.MsgBoxAboutTitle + " " + OemResources.Instance.Server.ApplicationName;
			base.Icon = OemResources.Instance.Server.ServerIcon;
			lblProductName.Text = OemResources.Instance.Server.ApplicationName;
			lblProductVersion.Text = VideoOS.Server.Service.TrayController.Resources.Text.MsgBoxAboutVersion + " " + OemResources.Instance.App.Version;
			lblBuildNumber.Text = VideoOS.Server.Service.TrayController.Resources.Text.MsgBoxAboutBuildNumber + " 17737";
			AppPicture.Image = base.Icon.ToBitmap();
		}
		finally
		{
			Cursor = Cursors.Default;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Server.Service.TrayController.AboutForm));
		this.AppPicture = new System.Windows.Forms.PictureBox();
		this.lblProductName = new System.Windows.Forms.Label();
		this.lblProductVersion = new System.Windows.Forms.Label();
		this.lblBuildNumber = new System.Windows.Forms.Label();
		this.btnOk = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.AppPicture).BeginInit();
		base.SuspendLayout();
		resources.ApplyResources(this.AppPicture, "AppPicture");
		this.AppPicture.Name = "AppPicture";
		this.AppPicture.TabStop = false;
		resources.ApplyResources(this.lblProductName, "lblProductName");
		this.lblProductName.Name = "lblProductName";
		resources.ApplyResources(this.lblProductVersion, "lblProductVersion");
		this.lblProductVersion.Name = "lblProductVersion";
		resources.ApplyResources(this.lblBuildNumber, "lblBuildNumber");
		this.lblBuildNumber.Name = "lblBuildNumber";
		this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		resources.ApplyResources(this.btnOk, "btnOk");
		this.btnOk.Name = "btnOk";
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.CancelButton = this.btnOk;
		base.Controls.Add(this.lblBuildNumber);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.lblProductVersion);
		base.Controls.Add(this.lblProductName);
		base.Controls.Add(this.AppPicture);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "AboutForm";
		((System.ComponentModel.ISupportInitialize)this.AppPicture).EndInit();
		base.ResumeLayout(false);
	}
}
