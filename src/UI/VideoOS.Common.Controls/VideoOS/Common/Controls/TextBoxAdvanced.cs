using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(TextBox))]
public class TextBoxAdvanced : TextBox
{
	private static class NativeMethods
	{
		public const int EM_SETCUEBANNER = 5377;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
	}

	private string _cueBanner = string.Empty;

	private IContainer components;

	[Category("Appearance")]
	[Description("Gets or sets the cue banner text for the text box.")]
	[DefaultValue("")]
	[Browsable(true)]
	[Localizable(true)]
	public string CueBanner
	{
		get
		{
			return _cueBanner;
		}
		set
		{
			_cueBanner = value ?? string.Empty;
			SetCueBanner(_cueBanner);
		}
	}

	public event EventHandler<KeyEventArgs> ProcessingDialogKey;

	public TextBoxAdvanced()
	{
		InitializeComponent();
		BackColor = BackColor;
	}

	public void BlockInput()
	{
		base.ReadOnly = true;
		base.TabStop = false;
		BackColor = SystemColors.Control;
	}

	private void SetCueBanner(string cueBannerText)
	{
		NativeMethods.SendMessage(base.Handle, 5377, IntPtr.Zero, cueBannerText);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (!string.IsNullOrEmpty(CueBanner))
		{
			SetCueBanner(CueBanner);
		}
		base.OnHandleCreated(e);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		KeyEventArgs e = new KeyEventArgs(keyData);
		this.ProcessingDialogKey?.Invoke(this, e);
		if (e.Handled || e.SuppressKeyPress)
		{
			return true;
		}
		return base.ProcessDialogKey(keyData);
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
	}
}
