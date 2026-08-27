using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class UserControlMissingPrerqError : UserControl
{
	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private Label messageLabel;

	public UserControlMissingPrerqError()
	{
		InitializeComponent();
		messageLabel.Text = Resources.ErrorMessage_PrerequisitesRequiredLabelSubLine;
		tableLayoutPanel1.Size = new Size(base.Width, base.Size.Height - (messageLabel.Size.Height + 18));
		tableLayoutPanel1.Location = new Point(0, messageLabel.Size.Height + 18);
	}

	public void SetMissingPrerequisites(Dictionary<string, string> missingPrerequisites)
	{
		SuspendLayout();
		tableLayoutPanel1.Controls.Clear();
		int num = 0;
		foreach (KeyValuePair<string, string> missingPrerequisite in missingPrerequisites)
		{
			Label control = CreateLabel(missingPrerequisite.Key);
			tableLayoutPanel1.Controls.Add(control, 0, num);
			if (missingPrerequisite.Value != null)
			{
				LinkLabel control2 = CreateLinkLabel(missingPrerequisite.Value);
				tableLayoutPanel1.Controls.Add(control2, 1, num);
			}
			num++;
		}
		Label control3 = CreateLabel(string.Empty);
		tableLayoutPanel1.Controls.Add(control3, 0, num);
		foreach (RowStyle item in (IEnumerable)tableLayoutPanel1.RowStyles)
		{
			item.SizeType = SizeType.Absolute;
			item.Height = 20f;
		}
		ResumeLayout();
	}

	private Label CreateLabel(string text)
	{
		return new Label
		{
			Anchor = (AnchorStyles.Bottom | AnchorStyles.Left),
			Text = text,
			TextAlign = ContentAlignment.BottomLeft,
			Dock = DockStyle.Bottom,
			AutoSize = true,
			Font = new Font("Arial", 9f)
		};
	}

	private LinkLabel CreateLinkLabel(string link)
	{
		LinkLabel linkLabel = new LinkLabel();
		linkLabel.TextAlign = ContentAlignment.BottomLeft;
		linkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		linkLabel.Text = Resources.ErrorMessage_PrerequisitesRequiredMoreInfo;
		linkLabel.Font = new Font("Arial", 9f);
		linkLabel.Dock = DockStyle.Bottom;
		linkLabel.Tag = link;
		linkLabel.LinkClicked += MoreInfo_Click;
		return linkLabel;
	}

	private void MoreInfo_Click(object sender, EventArgs e)
	{
		if (!(sender is LinkLabel linkLabel))
		{
			return;
		}
		string text = linkLabel.Tag.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			try
			{
				Process.Start(text);
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
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
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.messageLabel = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.tableLayoutPanel1.AutoSize = true;
		this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 16);
		this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(534, 242);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 242f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 242f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 242f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(534, 242);
		this.tableLayoutPanel1.TabIndex = 0;
		this.messageLabel.AutoSize = true;
		this.messageLabel.Dock = System.Windows.Forms.DockStyle.Top;
		this.messageLabel.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.messageLabel.Location = new System.Drawing.Point(0, 0);
		this.messageLabel.MaximumSize = new System.Drawing.Size(532, 242);
		this.messageLabel.Name = "messageLabel";
		this.messageLabel.Size = new System.Drawing.Size(91, 15);
		this.messageLabel.TabIndex = 1;
		this.messageLabel.Text = "messageLabel";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Transparent;
		base.Controls.Add(this.messageLabel);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Name = "UserControlMissingPrerqError";
		base.Size = new System.Drawing.Size(534, 258);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
