using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.Misc;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class StatusDialogForm : Form
{
	private delegate void CloseFormInvoke();

	private delegate void SetStatusMessageInvoke(string message);

	private IContainer components;

	private PictureBox PictureLogo;

	private SpinnerBox statusProgressBar;

	private UltraLabel lblStatus;

	public StatusDialogForm()
	{
		InitializeComponent();
	}

	private void StatusDialogForm_Load(object sender, EventArgs e)
	{
		try
		{
			PictureLogo.Image = OemResources.Instance.Manager.LoginLogo;
			statusProgressBar.GradientColor1 = Color.FromName(OemResources.Instance.Manager.LoginBarColorName1);
			statusProgressBar.GradientColor2 = Color.FromName(OemResources.Instance.Manager.LoginBarColorName2);
		}
		catch (FileNotFoundException)
		{
		}
		catch (NullReferenceException)
		{
		}
	}

	public void SetStatusMessage(string message)
	{
		if (base.InvokeRequired)
		{
			Invoke(new SetStatusMessageInvoke(SetStatusMessage), message);
		}
		else
		{
			((Control)(object)lblStatus).Text = message;
		}
	}

	public void SetSpeed(int speed)
	{
		statusProgressBar.Speed = speed;
	}

	public void CloseForm()
	{
		if (base.InvokeRequired)
		{
			Invoke(new CloseFormInvoke(CloseForm));
		}
		else
		{
			Close();
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.StatusDialogForm));
		Appearance val = new Appearance();
		this.PictureLogo = new System.Windows.Forms.PictureBox();
		this.statusProgressBar = new VideoOS.Common.Controls.SpinnerBox();
		this.lblStatus = new UltraLabel();
		((System.ComponentModel.ISupportInitialize)this.PictureLogo).BeginInit();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.PictureLogo, "PictureLogo");
		this.PictureLogo.Name = "PictureLogo";
		this.PictureLogo.TabStop = false;
		this.statusProgressBar.BackColor = System.Drawing.Color.White;
		componentResourceManager.ApplyResources(this.statusProgressBar, "statusProgressBar");
		this.statusProgressBar.GradientColor1 = System.Drawing.Color.DodgerBlue;
		this.statusProgressBar.GradientColor2 = System.Drawing.Color.White;
		this.statusProgressBar.Interval = 10;
		this.statusProgressBar.Name = "statusProgressBar";
		this.statusProgressBar.ScrollWay = VideoOS.Common.Controls.SpinnerBox.SCROLLGRADIENTALIGN.HORIZONTAL;
		this.statusProgressBar.Speed = 0;
		componentResourceManager.ApplyResources(val, "appearance1");
		componentResourceManager.ApplyResources(((AppearanceBase)val).FontData, "appearance1.FontData");
		((SubObjectBase)val).ForceApplyResources = "FontData|";
		((ControlBase)this.lblStatus).Appearance = (AppearanceBase)(object)val;
		componentResourceManager.ApplyResources(this.lblStatus, "lblStatus");
		((System.Windows.Forms.Control)(object)this.lblStatus).Name = "lblStatus";
		((System.Windows.Forms.Control)(object)this.lblStatus).UseWaitCursor = true;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ControlBox = false;
		base.Controls.Add((System.Windows.Forms.Control)(object)this.lblStatus);
		base.Controls.Add(this.statusProgressBar);
		base.Controls.Add(this.PictureLogo);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "StatusDialogForm";
		base.ShowInTaskbar = false;
		base.Load += new System.EventHandler(StatusDialogForm_Load);
		((System.ComponentModel.ISupportInitialize)this.PictureLogo).EndInit();
		base.ResumeLayout(false);
	}
}
