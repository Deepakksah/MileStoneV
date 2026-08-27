using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Installer.UI;

public class UserControlGeneralError : UserControl
{
	private IContainer components;

	private Label labelMessage;

	public UserControlGeneralError()
	{
		InitializeComponent();
		labelMessage.UseMnemonic = false;
	}

	public void SetErrorText(string text)
	{
		labelMessage.Text = text;
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
		this.labelMessage = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.labelMessage.Font = new System.Drawing.Font("Arial", 9f);
		this.labelMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMessage.Location = new System.Drawing.Point(0, 0);
		this.labelMessage.Name = "labelMessage";
		this.labelMessage.Size = new System.Drawing.Size(534, 258);
		this.labelMessage.TabIndex = 6;
		this.labelMessage.Text = ".NET 4.0              Microsoft Corporation";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.labelMessage);
		base.Name = "UserControlGeneralError";
		base.Size = new System.Drawing.Size(534, 258);
		base.ResumeLayout(false);
	}
}
