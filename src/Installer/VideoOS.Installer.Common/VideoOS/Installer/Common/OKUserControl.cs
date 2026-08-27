using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Installer.Common;

public class OKUserControl : ButtonsUserControl
{
	private IContainer components;

	private Button btnOK;

	public OKUserControl()
	{
		InitializeComponent();
	}

	private void btnOK_Click(object source, EventArgs e)
	{
		RaseOnButtonClicked(DialogResult.OK);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Common.OKUserControl));
		this.btnOK = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnOK);
		base.Name = "OKUserControl";
		base.ResumeLayout(false);
	}
}
