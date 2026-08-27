using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class SplashScreen : Form
{
	private int _autoCloseTimeOut;

	private bool _closeOnMouseClick;

	private IContainer components;

	private Timer _autoCloseTimer;

	public Image SplashImage
	{
		get
		{
			return BackgroundImage;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			BackgroundImage = value;
			base.Width = value.Width + 2;
			base.Height = value.Height + 2;
		}
	}

	public int AutoCloseTimeOut
	{
		get
		{
			return _autoCloseTimeOut;
		}
		set
		{
			_autoCloseTimeOut = value;
		}
	}

	public bool CloseOnMouseClick
	{
		get
		{
			return _closeOnMouseClick;
		}
		set
		{
			_closeOnMouseClick = value;
		}
	}

	public SplashScreen()
	{
		InitializeComponent();
	}

	private void SplashScreen_Load(object sender, EventArgs e)
	{
		_autoCloseTimer.Stop();
		if (_autoCloseTimeOut != 0)
		{
			_autoCloseTimer.Interval = _autoCloseTimeOut;
			_autoCloseTimer.Start();
		}
	}

	private void _autoCloseTimer_Tick(object sender, EventArgs e)
	{
		_autoCloseTimer.Stop();
		if (_autoCloseTimeOut != 0)
		{
			Close();
		}
	}

	private void SplashScreen_Click(object sender, EventArgs e)
	{
		if (_closeOnMouseClick)
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.SplashScreen));
		this._autoCloseTimer = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this, "$this");
		this.BackColor = System.Drawing.SystemColors.WindowFrame;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "SplashScreen";
		base.TransparencyKey = System.Drawing.SystemColors.WindowFrame;
		base.ResumeLayout(false);
	}
}
