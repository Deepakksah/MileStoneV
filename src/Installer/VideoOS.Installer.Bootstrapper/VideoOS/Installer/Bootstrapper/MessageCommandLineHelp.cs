using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.Bootstrapper;

public class MessageCommandLineHelp : Form
{
	private IContainer components;

	private Label labelCaption;

	private Label labelName;

	private Label labelDescription;

	private Button button1;

	public MessageCommandLineHelp()
	{
		InitializeComponent();
		labelCaption.Text = "";
		labelName.Text = "";
		labelDescription.Text = "";
		base.Icon = GlobalData.OemHelper.InstallerIcon;
	}

	public void SetCaption(string caption)
	{
		labelCaption.Text = caption;
	}

	public void AddLines(string name, string text)
	{
		labelName.Text = $"{labelName.Text} \n{name}";
		labelDescription.Text = $"{labelDescription.Text} \n{text}";
	}

	private void button1_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.Bootstrapper.MessageCommandLineHelp));
		this.labelCaption = new System.Windows.Forms.Label();
		this.labelName = new System.Windows.Forms.Label();
		this.labelDescription = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.labelCaption.AutoSize = true;
		this.labelCaption.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelCaption.Location = new System.Drawing.Point(12, 9);
		this.labelCaption.Name = "labelCaption";
		this.labelCaption.Size = new System.Drawing.Size(48, 14);
		this.labelCaption.TabIndex = 0;
		this.labelCaption.Text = "Caption";
		this.labelName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.labelName.AutoSize = true;
		this.labelName.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelName.Location = new System.Drawing.Point(12, 44);
		this.labelName.Name = "labelName";
		this.labelName.Size = new System.Drawing.Size(38, 14);
		this.labelName.TabIndex = 1;
		this.labelName.Text = "Name";
		this.labelDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.labelDescription.AutoSize = true;
		this.labelDescription.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelDescription.Location = new System.Drawing.Point(189, 44);
		this.labelDescription.Name = "labelDescription";
		this.labelDescription.Size = new System.Drawing.Size(67, 14);
		this.labelDescription.TabIndex = 2;
		this.labelDescription.Text = "Description";
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.Location = new System.Drawing.Point(465, 369);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 3;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(552, 403);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.labelDescription);
		base.Controls.Add(this.labelName);
		base.Controls.Add(this.labelCaption);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "MessageCommandLineHelp";
		this.Text = "Command line option reference";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
