using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Installer.Common;

public class YesNoUserControl : ButtonsUserControl
{
	private IContainer components;

	private Button btnYes;

	private Button btnNo;

	public YesNoUserControl()
	{
		InitializeComponent();
	}

	private void btnYes_Click(object sender, EventArgs e)
	{
		RaseOnButtonClicked(DialogResult.Yes);
	}

	private void btnNo_Click(object sender, EventArgs e)
	{
		RaseOnButtonClicked(DialogResult.No);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Common.YesNoUserControl));
		this.btnYes = new System.Windows.Forms.Button();
		this.btnNo = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnYes, "btnYes");
		this.btnYes.Name = "btnYes";
		this.btnYes.UseVisualStyleBackColor = true;
		this.btnYes.Click += new System.EventHandler(btnYes_Click);
		componentResourceManager.ApplyResources(this.btnNo, "btnNo");
		this.btnNo.Name = "btnNo";
		this.btnNo.UseVisualStyleBackColor = true;
		this.btnNo.Click += new System.EventHandler(btnNo_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnNo);
		base.Controls.Add(this.btnYes);
		base.Name = "YesNoUserControl";
		base.ResumeLayout(false);
	}
}
