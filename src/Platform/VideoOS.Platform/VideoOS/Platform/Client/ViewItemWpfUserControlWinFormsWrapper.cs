using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use ViewItemWpfUserControl instead.", false)]
public class ViewItemWpfUserControlWinFormsWrapper : ViewItemUserControl
{
	private ViewItemWpfUserControl _wpfControl;

	private IContainer components;

	private ElementHost _childHost;

	public ViewItemWpfUserControl WpfControl
	{
		set
		{
			UnsubscribeChildEvents();
			_wpfControl = value;
			_childHost.Child = value;
			SubscribeChildEvents();
		}
	}

	public override bool Hidden
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.Hidden;
			}
			return base.Hidden;
		}
		set
		{
			if (_wpfControl != null)
			{
				_wpfControl.Hidden = value;
			}
			else
			{
				base.Hidden = value;
			}
		}
	}

	public override bool Maximizable
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.Maximizable;
			}
			return base.Maximizable;
		}
	}

	public override bool Maximized
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.Maximized;
			}
			return base.Maximized;
		}
		set
		{
			if (_wpfControl != null)
			{
				_wpfControl.Maximized = value;
			}
			else
			{
				base.Maximized = value;
			}
		}
	}

	public override bool Selectable
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.Selectable;
			}
			return base.Selectable;
		}
	}

	public override bool Selected
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.Selected;
			}
			return base.Selected;
		}
		set
		{
			if (_wpfControl != null)
			{
				_wpfControl.Selected = value;
			}
			else
			{
				base.Selected = value;
			}
		}
	}

	public override bool ShowToolbar
	{
		get
		{
			if (_wpfControl != null)
			{
				return _wpfControl.ShowToolbar;
			}
			return base.ShowToolbar;
		}
	}

	public ViewItemWpfUserControlWinFormsWrapper()
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
			UnsubscribeChildEvents();
			_wpfControl.Close();
			_wpfControl = null;
		}
		base.Close();
	}

	public override void Print()
	{
		if (_wpfControl != null)
		{
			_wpfControl.Print();
		}
	}

	public override void Print(string name, string information)
	{
		if (_wpfControl != null)
		{
			_wpfControl.Print(name, information);
		}
	}

	private void SubscribeChildEvents()
	{
		if (_wpfControl != null)
		{
			_wpfControl.SelectRequestEvent += _wpfControl_SelectRequestEvent;
			_wpfControl.DeselectRequestEvent += _wpfControl_DeselectRequestEvent;
			_wpfControl.MaximizeRequestEvent += _wpfControl_MaximizeRequestEvent;
			_wpfControl.RestoreRequestEvent += _wpfControl_RestoreRequestEvent;
			_wpfControl.ClickEvent += _wpfControl_ClickEvent;
			_wpfControl.DoubleClickEvent += _wpfControl_DoubleClickEvent;
		}
	}

	private void UnsubscribeChildEvents()
	{
		if (_wpfControl != null)
		{
			_wpfControl.SelectRequestEvent -= _wpfControl_SelectRequestEvent;
			_wpfControl.DeselectRequestEvent -= _wpfControl_DeselectRequestEvent;
			_wpfControl.MaximizeRequestEvent -= _wpfControl_MaximizeRequestEvent;
			_wpfControl.RestoreRequestEvent -= _wpfControl_RestoreRequestEvent;
			_wpfControl.ClickEvent -= _wpfControl_ClickEvent;
			_wpfControl.DoubleClickEvent -= _wpfControl_DoubleClickEvent;
		}
	}

	private void _wpfControl_DoubleClickEvent(object sender, EventArgs e)
	{
		FireDoubleClickEvent();
	}

	private void _wpfControl_ClickEvent(object sender, EventArgs e)
	{
		FireClickEvent();
	}

	private void _wpfControl_RestoreRequestEvent(object sender, EventArgs e)
	{
		FireRestoreRequestEvent();
	}

	private void _wpfControl_MaximizeRequestEvent(object sender, EventArgs e)
	{
		FireMaximizeRequestEvent();
	}

	private void _wpfControl_DeselectRequestEvent(object sender, EventArgs e)
	{
		FireDeselectRequestEvent();
	}

	private void _wpfControl_SelectRequestEvent(object sender, EventArgs e)
	{
		FireSelectRequestEvent();
	}

	private void ViewItemWpfUserControlWinFormsWrapper_Load(object sender, EventArgs e)
	{
		if (_wpfControl != null && _wpfControl.WindowInformation == null)
		{
			_wpfControl.WindowInformation = base.WindowInformation;
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
		this._childHost = new System.Windows.Forms.Integration.ElementHost();
		base.SuspendLayout();
		this._childHost.Dock = System.Windows.Forms.DockStyle.Fill;
		this._childHost.Location = new System.Drawing.Point(0, 0);
		this._childHost.Name = "_childHost";
		this._childHost.Size = new System.Drawing.Size(150, 150);
		this._childHost.TabIndex = 1;
		this._childHost.Text = "elementHost1";
		this._childHost.Child = null;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.Controls.Add(this._childHost);
		this.Font = new System.Drawing.Font("Arial", 9.25f);
		base.Name = "ViewItemWpfUserControlWinFormsWrapper";
		base.Load += new System.EventHandler(ViewItemWpfUserControlWinFormsWrapper_Load);
		base.ResumeLayout(false);
	}
}
