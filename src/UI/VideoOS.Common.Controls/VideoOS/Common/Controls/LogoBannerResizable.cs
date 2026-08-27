using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class LogoBannerResizable : UserControl
{
	private IContainer components;

	private Panel pnlHeader;

	private PictureBox pictureBoxRight;

	private PictureBox pictureBoxLeft;

	private PictureBox pictureBoxMiddle;

	public LogoBannerResizable()
	{
		InitializeComponent();
		try
		{
			pictureBoxLeft.Image = OemResources.Instance.Manager.BannerLeft;
			pictureBoxMiddle.BackgroundImage = OemResources.Instance.Manager.BannerMiddle;
			pictureBoxRight.Image = OemResources.Instance.Manager.BannerRight;
		}
		catch (FileNotFoundException)
		{
		}
		catch (NullReferenceException)
		{
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

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		this.pnlHeader = new System.Windows.Forms.Panel();
		this.pictureBoxMiddle = new System.Windows.Forms.PictureBox();
		this.pictureBoxLeft = new System.Windows.Forms.PictureBox();
		this.pictureBoxRight = new System.Windows.Forms.PictureBox();
		this.pnlHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxMiddle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxLeft).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxRight).BeginInit();
		base.SuspendLayout();
		this.pnlHeader.BackColor = System.Drawing.Color.White;
		this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pnlHeader.Controls.Add(this.pictureBoxMiddle);
		this.pnlHeader.Controls.Add(this.pictureBoxLeft);
		this.pnlHeader.Controls.Add(this.pictureBoxRight);
		this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlHeader.Location = new System.Drawing.Point(0, 0);
		this.pnlHeader.Name = "pnlHeader";
		this.pnlHeader.Size = new System.Drawing.Size(751, 77);
		this.pnlHeader.TabIndex = 10;
		this.pictureBoxMiddle.BackColor = System.Drawing.SystemColors.HotTrack;
		this.pictureBoxMiddle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBoxMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBoxMiddle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.pictureBoxMiddle.Location = new System.Drawing.Point(456, 0);
		this.pictureBoxMiddle.Name = "pictureBoxMiddle";
		this.pictureBoxMiddle.Size = new System.Drawing.Size(1, 77);
		this.pictureBoxMiddle.TabIndex = 9;
		this.pictureBoxMiddle.TabStop = false;
		this.pictureBoxLeft.BackColor = System.Drawing.SystemColors.HotTrack;
		this.pictureBoxLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.pictureBoxLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.pictureBoxLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.pictureBoxLeft.Location = new System.Drawing.Point(0, 0);
		this.pictureBoxLeft.Name = "pictureBoxLeft";
		this.pictureBoxLeft.Size = new System.Drawing.Size(456, 77);
		this.pictureBoxLeft.TabIndex = 8;
		this.pictureBoxLeft.TabStop = false;
		this.pictureBoxRight.BackColor = System.Drawing.SystemColors.HotTrack;
		this.pictureBoxRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.pictureBoxRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.pictureBoxRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.pictureBoxRight.Location = new System.Drawing.Point(457, 0);
		this.pictureBoxRight.Name = "pictureBoxRight";
		this.pictureBoxRight.Size = new System.Drawing.Size(294, 77);
		this.pictureBoxRight.TabIndex = 7;
		this.pictureBoxRight.TabStop = false;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.Controls.Add(this.pnlHeader);
		this.MinimumSize = new System.Drawing.Size(751, 77);
		base.Name = "LogoBannerResizable";
		base.Size = new System.Drawing.Size(751, 77);
		this.pnlHeader.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBoxMiddle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxLeft).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxRight).EndInit();
		base.ResumeLayout(false);
	}
}
