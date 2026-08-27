using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Platform.Admin;

public class AddUserControl : UserControl
{
	private IContainer components;

	public bool ItemAlreadySaved { get; set; }

	public AddUserControl()
	{
		InitializeComponent();
		ItemAlreadySaved = false;
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "AddUserControl";
		base.Size = new System.Drawing.Size(325, 83);
		base.ResumeLayout(false);
	}
}
