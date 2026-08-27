using System.ComponentModel;
using System.Windows.Forms;

namespace VideoOS.Installer.Common;

public class ButtonsUserControl : UserControl
{
	private IContainer components;

	public event ButtonClicked OnButtonClicked;

	public ButtonsUserControl()
	{
		InitializeComponent();
	}

	public void RaseOnButtonClicked(DialogResult e)
	{
		this.OnButtonClicked?.Invoke(e);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Common.ButtonsUserControl));
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ButtonsUserControl";
		base.ResumeLayout(false);
	}
}
