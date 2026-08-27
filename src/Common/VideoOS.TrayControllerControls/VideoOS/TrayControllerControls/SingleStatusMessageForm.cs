using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace VideoOS.TrayControllerControls;

public class SingleStatusMessageForm : Form
{
	private IContainer components;

	private Label label1;

	private Label label2;

	private TextBox txtMessage;

	private TextBox txtTime;

	private Button btnClose;

	public SingleStatusMessageForm(string time, string message)
	{
		InitializeComponent();
		txtTime.Text = time;
		txtMessage.Text = message;
		txtMessage.Select(0, 0);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.TrayControllerControls.SingleStatusMessageForm));
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtMessage = new System.Windows.Forms.TextBox();
		this.txtTime = new System.Windows.Forms.TextBox();
		this.btnClose = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.label1, "label1");
		this.label1.Name = "label1";
		componentResourceManager.ApplyResources(this.label2, "label2");
		this.label2.Name = "label2";
		componentResourceManager.ApplyResources(this.txtMessage, "txtMessage");
		this.txtMessage.Name = "txtMessage";
		this.txtMessage.ReadOnly = true;
		componentResourceManager.ApplyResources(this.txtTime, "txtTime");
		this.txtTime.Name = "txtTime";
		this.txtTime.ReadOnly = true;
		componentResourceManager.ApplyResources(this.btnClose, "btnClose");
		this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnClose.Name = "btnClose";
		this.btnClose.UseVisualStyleBackColor = true;
		base.AcceptButton = this.btnClose;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnClose;
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.txtTime);
		base.Controls.Add(this.txtMessage);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Name = "SingleStatusMessageForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
