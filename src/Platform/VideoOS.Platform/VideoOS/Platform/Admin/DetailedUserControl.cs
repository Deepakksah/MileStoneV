using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Platform.Admin;

public class DetailedUserControl : UserControl
{
	private IContainer components;

	public virtual string TabHelpId { get; set; }

	public virtual Image Image { get; set; }

	public virtual string TabDisplayName { get; set; }

	public DetailedUserControl()
	{
		InitializeComponent();
	}

	public virtual void Selected()
	{
	}

	public virtual void Deselected(bool saveDetails)
	{
	}

	public virtual bool ValidateDetails()
	{
		return true;
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
		this.BackColor = System.Drawing.Color.Transparent;
		base.Name = "DetailedUserControl";
		base.Size = new System.Drawing.Size(425, 283);
		base.ResumeLayout(false);
	}
}
