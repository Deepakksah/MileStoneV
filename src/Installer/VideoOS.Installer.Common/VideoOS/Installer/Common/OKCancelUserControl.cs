using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Installer.Common;

public class OKCancelUserControl : ButtonsUserControl
{
	private IContainer components;

	private Button btnOK;

	private Button button1;

	public OKCancelUserControl()
	{
		InitializeComponent();
	}

	private void btnOK_Click(object source, EventArgs e)
	{
		RaseOnButtonClicked(DialogResult.OK);
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		RaseOnButtonClicked(DialogResult.Cancel);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Common.OKCancelUserControl));
		this.btnOK = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		componentResourceManager.ApplyResources(this.button1, "button1");
		this.button1.Name = "button1";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.btnOK);
		base.Name = "OKCancelUserControl";
		base.ResumeLayout(false);
	}
}
