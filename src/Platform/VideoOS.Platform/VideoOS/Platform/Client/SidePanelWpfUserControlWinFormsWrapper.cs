using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use SidePanelWpfUserControl instead.", false)]
public class SidePanelWpfUserControlWinFormsWrapper : SidePanelUserControl
{
	private SidePanelWpfUserControl _wpfControl;

	private IContainer components;

	private ElementHost _childHost;

	public SidePanelWpfUserControl WpfControl
	{
		set
		{
			_wpfControl = value;
			_childHost.Child = value;
		}
	}

	public SidePanelWpfUserControlWinFormsWrapper()
	{
		InitializeComponent();
	}

	public override void Init()
	{
		if (_wpfControl != null)
		{
			_wpfControl.Init();
		}
		base.Init();
	}

	public override void Close()
	{
		_childHost.Child = null;
		_childHost.Dispose();
		if (_wpfControl != null)
		{
			_wpfControl.Close();
			_wpfControl = null;
		}
		base.Close();
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
		this._childHost = new System.Windows.Forms.Integration.ElementHost();
		base.SuspendLayout();
		this._childHost.Dock = System.Windows.Forms.DockStyle.Fill;
		this._childHost.Location = new System.Drawing.Point(0, 0);
		this._childHost.Name = "_childHost";
		this._childHost.Size = new System.Drawing.Size(150, 150);
		this._childHost.TabIndex = 0;
		this._childHost.Child = null;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this._childHost);
		base.Name = "SidePanelWpfUserControlWinFormsWrapper";
		base.ResumeLayout(false);
	}
}
