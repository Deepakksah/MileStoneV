using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace VideoOS.Installer.UI.Framework;

[CLSCompliant(true)]
public abstract class WizardPageWpfBase : WizardPageBase
{
	private readonly Func<System.Windows.Controls.UserControl> _wpfControlFactory;

	private IContainer components;

	private ElementHost _elementHost;

	protected WizardPageWpfBase(string pageName, Func<System.Windows.Controls.UserControl> wpfControlFactory)
	{
		InitializeComponent();
		PageName = pageName;
		_wpfControlFactory = wpfControlFactory;
		_elementHost.Child = wpfControlFactory?.Invoke();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		_elementHost.Child = _wpfControlFactory?.Invoke();
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
		this._elementHost = new System.Windows.Forms.Integration.ElementHost();
		base.SuspendLayout();
		this._elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
		this._elementHost.Location = new System.Drawing.Point(0, 0);
		this._elementHost.Name = "elementHost";
		this._elementHost.Size = new System.Drawing.Size(634, 492);
		this._elementHost.TabIndex = 1;
		this._elementHost.Child = null;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageWpfBase";
		base.WizardPage.Controls.Add(this._elementHost);
		base.ResumeLayout(false);
	}
}
