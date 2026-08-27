using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.Misc;
using VideoOS.Common.Controls.Properties;
using VideoOS.Utilities.Network;

namespace VideoOS.Common.Controls;

public class UnhandledExceptionDialog : Form
{
	private const int expandedHeight = 430;

	private const int collapsedHeight = 201;

	private string _applicationName;

	private Exception _exception;

	private bool hasDetails;

	private IContainer components;

	private Panel pnlTitle;

	private Label lblWeAreSorry;

	private UltraButton cmdQuit;

	private UltraButton cmdDetails;

	private TextBox txtDetails;

	private Label lblExceptionMessage;

	private ImageList imageList;

	private Label lblInfoMightBeLost;

	private GroupBox splitterTop;

	private GroupBox splitterBottom;

	private Assembly ParentAssembly
	{
		get
		{
			if (Assembly.GetEntryAssembly() == null)
			{
				return Assembly.GetCallingAssembly();
			}
			return Assembly.GetEntryAssembly();
		}
	}

	private string UserIdentity
	{
		get
		{
			using WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
			string text = windowsIdentity.Name;
			if (string.IsNullOrEmpty(text))
			{
				text = Environment.UserDomainName + "\\" + Environment.UserName;
			}
			return text;
		}
	}

	public UnhandledExceptionDialog(string applicationName, Exception exception)
	{
		InitializeComponent();
		_applicationName = applicationName;
		_exception = exception;
	}

	private void DialogUnhandledException_Load(object sender, EventArgs e)
	{
		try
		{
			string text = _exception.Message;
			if (text == string.Empty && _exception.InnerException != null)
			{
				text = _exception.InnerException.Message;
			}
			if (text == string.Empty)
			{
				text = e.GetType().ToString();
			}
			lblExceptionMessage.Text = string.Format(lblExceptionMessage.Text, text);
			lblWeAreSorry.Text = string.Format(lblWeAreSorry.Text, _applicationName);
			Text = string.Format(Text, _applicationName);
			string text2 = Resources.ExceptionText + "\r\n\r\n";
			text2 += ExceptionToString(_exception);
			text2 = text2 + "\r\n\r\n" + Resources.SystemInfo + "\r\n\r\n";
			text2 += SystemInfo();
			txtDetails.Text = text2;
		}
		catch
		{
			txtDetails.Text = Resources.UnableToRenderException;
		}
	}

	private void cmdDetails_Click(object sender, EventArgs e)
	{
		hasDetails = !hasDetails;
		((ControlBase)cmdDetails).Appearance.Image = (hasDetails ? 1 : 0);
		base.Height = (hasDetails ? 430 : 201);
	}

	private string GetHostInfo()
	{
		try
		{
			string text = IPHelper.DnsGetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
			string hostName = IPHelper.DnsGetHostEntry(Dns.GetHostName()).HostName;
			return text + " (" + hostName + ")";
		}
		catch
		{
			return Resources.UnableToGetIP;
		}
	}

	private string SystemInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			File.GetLastWriteTime(ParentAssembly.Location);
		}
		catch
		{
			_ = DateTime.MinValue;
		}
		stringBuilder.Append(Resources.DateTime);
		stringBuilder.Append(DateTime.Now);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.MachineName);
		stringBuilder.Append(Environment.MachineName);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.IPAddress);
		stringBuilder.Append(GetHostInfo());
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.CurrentUser);
		stringBuilder.Append(UserIdentity);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}

	private string ExceptionToString(Exception e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (e.InnerException != null)
		{
			stringBuilder.Append(Resources.InnerException);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(ExceptionToString(e.InnerException));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Resources.OuterException);
			stringBuilder.Append(Environment.NewLine);
		}
		stringBuilder.Append(Resources.ExceptionSource);
		stringBuilder.Append((e.Source == null) ? e.Message : e.Source);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.ExceptionType);
		stringBuilder.Append(e.GetType().FullName);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.ExceptionMessage);
		stringBuilder.Append(e.Message);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Resources.ExceptionTargetSite);
		stringBuilder.Append((e.TargetSite == null) ? e.Message : e.TargetSite.Name);
		stringBuilder.Append(Environment.NewLine);
		if (e.StackTrace != null)
		{
			string value = e.StackTrace.ToString();
			stringBuilder.Append(value);
		}
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}

	private void cmdQuit_Click(object sender, EventArgs e)
	{
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.UnhandledExceptionDialog));
		Appearance val = new Appearance();
		this.pnlTitle = new System.Windows.Forms.Panel();
		this.lblWeAreSorry = new System.Windows.Forms.Label();
		this.cmdQuit = new UltraButton();
		this.lblInfoMightBeLost = new System.Windows.Forms.Label();
		this.splitterTop = new System.Windows.Forms.GroupBox();
		this.cmdDetails = new UltraButton();
		this.imageList = new System.Windows.Forms.ImageList(this.components);
		this.txtDetails = new System.Windows.Forms.TextBox();
		this.lblExceptionMessage = new System.Windows.Forms.Label();
		this.splitterBottom = new System.Windows.Forms.GroupBox();
		this.pnlTitle.SuspendLayout();
		base.SuspendLayout();
		this.pnlTitle.BackColor = System.Drawing.SystemColors.Window;
		this.pnlTitle.Controls.Add(this.lblWeAreSorry);
		resources.ApplyResources(this.pnlTitle, "pnlTitle");
		this.pnlTitle.Name = "pnlTitle";
		resources.ApplyResources(this.lblWeAreSorry, "lblWeAreSorry");
		this.lblWeAreSorry.Name = "lblWeAreSorry";
		((UltraButtonBase)this.cmdQuit).DialogResult = System.Windows.Forms.DialogResult.Cancel;
		resources.ApplyResources(this.cmdQuit, "cmdQuit");
		((System.Windows.Forms.Control)(object)this.cmdQuit).Name = "cmdQuit";
		((System.Windows.Forms.Control)(object)this.cmdQuit).Click += new System.EventHandler(cmdQuit_Click);
		resources.ApplyResources(this.lblInfoMightBeLost, "lblInfoMightBeLost");
		this.lblInfoMightBeLost.Name = "lblInfoMightBeLost";
		resources.ApplyResources(this.splitterTop, "splitterTop");
		this.splitterTop.Name = "splitterTop";
		this.splitterTop.TabStop = false;
		((AppearanceBase)val).Image = 0;
		((ControlBase)this.cmdDetails).Appearance = (AppearanceBase)(object)val;
		((ControlBase)this.cmdDetails).ImageList = this.imageList;
		resources.ApplyResources(this.cmdDetails, "cmdDetails");
		((System.Windows.Forms.Control)(object)this.cmdDetails).Name = "cmdDetails";
		((System.Windows.Forms.Control)(object)this.cmdDetails).Click += new System.EventHandler(cmdDetails_Click);
		this.imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
		this.imageList.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList.Images.SetKeyName(0, "");
		this.imageList.Images.SetKeyName(1, "");
		resources.ApplyResources(this.txtDetails, "txtDetails");
		this.txtDetails.Name = "txtDetails";
		this.txtDetails.ReadOnly = true;
		resources.ApplyResources(this.lblExceptionMessage, "lblExceptionMessage");
		this.lblExceptionMessage.Name = "lblExceptionMessage";
		resources.ApplyResources(this.splitterBottom, "splitterBottom");
		this.splitterBottom.Name = "splitterBottom";
		this.splitterBottom.TabStop = false;
		resources.ApplyResources(this, "$this");
		base.CancelButton = (System.Windows.Forms.IButtonControl)this.cmdQuit;
		base.Controls.Add(this.splitterBottom);
		base.Controls.Add(this.lblExceptionMessage);
		base.Controls.Add(this.txtDetails);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.cmdDetails);
		base.Controls.Add(this.splitterTop);
		base.Controls.Add(this.lblInfoMightBeLost);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.cmdQuit);
		base.Controls.Add(this.pnlTitle);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "UnhandledExceptionDialog";
		base.Load += new System.EventHandler(DialogUnhandledException_Load);
		this.pnlTitle.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
