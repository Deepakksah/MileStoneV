using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Installer.UI;

public class UserControlMeasures : UserControl
{
	private IContainer components;

	private TextBox textBox1;

	private TextBox textBox7;

	private TextBox textBox8;

	private TextBox textBox9;

	private TextBox textBox12;

	private TextBox textBox10;

	private TextBox textBox11;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Button buttonDone;

	private Button button1;

	private ProgressBar progressBarTotalProgress;

	private Label labelComponentProgress;

	private TextBox textBox2;

	public UserControlMeasures()
	{
		InitializeComponent();
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
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.textBox7 = new System.Windows.Forms.TextBox();
		this.textBox8 = new System.Windows.Forms.TextBox();
		this.textBox9 = new System.Windows.Forms.TextBox();
		this.textBox12 = new System.Windows.Forms.TextBox();
		this.textBox10 = new System.Windows.Forms.TextBox();
		this.textBox11 = new System.Windows.Forms.TextBox();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.buttonDone = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.progressBarTotalProgress = new System.Windows.Forms.ProgressBar();
		this.labelComponentProgress = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Location = new System.Drawing.Point(50, 468);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(534, 24);
		this.textBox1.TabIndex = 83;
		this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox7.Location = new System.Drawing.Point(36, 313);
		this.textBox7.Multiline = true;
		this.textBox7.Name = "textBox7";
		this.textBox7.Size = new System.Drawing.Size(10, 179);
		this.textBox7.TabIndex = 81;
		this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox8.Location = new System.Drawing.Point(27, 240);
		this.textBox8.Multiline = true;
		this.textBox8.Name = "textBox8";
		this.textBox8.Size = new System.Drawing.Size(14, 252);
		this.textBox8.TabIndex = 82;
		this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox9.Location = new System.Drawing.Point(18, 150);
		this.textBox9.Multiline = true;
		this.textBox9.Name = "textBox9";
		this.textBox9.Size = new System.Drawing.Size(12, 342);
		this.textBox9.TabIndex = 80;
		this.textBox12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox12.Location = new System.Drawing.Point(50, 447);
		this.textBox12.Multiline = true;
		this.textBox12.Name = "textBox12";
		this.textBox12.Size = new System.Drawing.Size(534, 45);
		this.textBox12.TabIndex = 79;
		this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox10.Location = new System.Drawing.Point(9, 93);
		this.textBox10.Multiline = true;
		this.textBox10.Name = "textBox10";
		this.textBox10.Size = new System.Drawing.Size(19, 399);
		this.textBox10.TabIndex = 77;
		this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox11.Location = new System.Drawing.Point(0, 40);
		this.textBox11.Multiline = true;
		this.textBox11.Name = "textBox11";
		this.textBox11.Size = new System.Drawing.Size(12, 452);
		this.textBox11.TabIndex = 78;
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 84;
		this.labelHeadline.Text = "Caption (Arial Regular 16pt)";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(540, 46);
		this.labelSubHeading.TabIndex = 85;
		this.labelSubHeading.Text = "Description (Arial Regular 9pt)";
		this.buttonDone.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.buttonDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.buttonDone.Font = new System.Drawing.Font("Tahoma", 9f);
		this.buttonDone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.buttonDone.Location = new System.Drawing.Point(484, 447);
		this.buttonDone.Name = "buttonDone";
		this.buttonDone.Size = new System.Drawing.Size(100, 22);
		this.buttonDone.TabIndex = 86;
		this.buttonDone.Text = "Button2";
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.button1.Location = new System.Drawing.Point(382, 447);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(100, 22);
		this.button1.TabIndex = 86;
		this.button1.Text = "Button1";
		this.progressBarTotalProgress.Location = new System.Drawing.Point(50, 313);
		this.progressBarTotalProgress.Name = "progressBarTotalProgress";
		this.progressBarTotalProgress.Size = new System.Drawing.Size(534, 10);
		this.progressBarTotalProgress.TabIndex = 87;
		this.labelComponentProgress.AutoSize = true;
		this.labelComponentProgress.BackColor = System.Drawing.Color.Transparent;
		this.labelComponentProgress.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelComponentProgress.Location = new System.Drawing.Point(47, 296);
		this.labelComponentProgress.Name = "labelComponentProgress";
		this.labelComponentProgress.Size = new System.Drawing.Size(116, 14);
		this.labelComponentProgress.TabIndex = 88;
		this.labelComponentProgress.Text = "Label (Tahoma 9pt)";
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox2.Location = new System.Drawing.Point(66, 428);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(517, 20);
		this.textBox2.TabIndex = 89;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.textBox2);
		base.Controls.Add(this.labelComponentProgress);
		base.Controls.Add(this.progressBarTotalProgress);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.buttonDone);
		base.Controls.Add(this.labelHeadline);
		base.Controls.Add(this.labelSubHeading);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.textBox7);
		base.Controls.Add(this.textBox8);
		base.Controls.Add(this.textBox9);
		base.Controls.Add(this.textBox12);
		base.Controls.Add(this.textBox10);
		base.Controls.Add(this.textBox11);
		base.Name = "UserControlMeassures";
		base.Size = new System.Drawing.Size(634, 492);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
