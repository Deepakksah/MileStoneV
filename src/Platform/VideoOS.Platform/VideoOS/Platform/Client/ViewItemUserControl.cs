using System;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use ViewItemWpfUserControl instead.", false)]
public class ViewItemUserControl : UserControl
{
	protected bool _selected;

	protected bool _maximized;

	protected bool _hidden;

	private float FontDivFactor = 3.5f;

	public virtual bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
		}
	}

	public virtual bool Maximized
	{
		get
		{
			return _maximized;
		}
		set
		{
			_maximized = value;
		}
	}

	public virtual bool Hidden
	{
		get
		{
			return _hidden;
		}
		set
		{
			_hidden = value;
		}
	}

	public virtual bool Selectable => true;

	public virtual bool Maximizable => true;

	public virtual bool ShowToolbar => true;

	public virtual int CalculateHeaderHeight
	{
		get
		{
			if (base.Height < 20)
			{
				return 0;
			}
			float num = (float)base.Height / FontDivFactor;
			if (num < 68f)
			{
				num = 68f;
			}
			if (num > 120f)
			{
				num = 120f;
			}
			return new Font(Font.FontFamily, num).Height / 10 + 2;
		}
	}

	public virtual Font CalculateHeaderFont
	{
		get
		{
			if (base.Height < 20)
			{
				return Font;
			}
			float num = (float)base.Height / FontDivFactor;
			if (num < 68f)
			{
				num = 68f;
			}
			if (num > 120f)
			{
				num = 120f;
			}
			return new Font(Font.FontFamily, num);
		}
	}

	public WindowInformation WindowInformation { get; set; }

	public event EventHandler SelectRequestEvent;

	public event EventHandler DeselectRequestEvent;

	public event EventHandler MaximizeRequestEvent;

	public event EventHandler RestoreRequestEvent;

	public event EventHandler ClickEvent;

	public event EventHandler DoubleClickEvent;

	protected virtual void FireSelectRequestEvent()
	{
		if (this.SelectRequestEvent != null)
		{
			this.SelectRequestEvent(this, new EventArgs());
		}
	}

	protected virtual void FireDeselectRequestEvent()
	{
		if (this.DeselectRequestEvent != null)
		{
			this.DeselectRequestEvent(this, new EventArgs());
		}
	}

	protected virtual void FireMaximizeRequestEvent()
	{
		if (this.MaximizeRequestEvent != null)
		{
			this.MaximizeRequestEvent(this, new EventArgs());
		}
	}

	protected virtual void FireRestoreRequestEvent()
	{
		if (this.RestoreRequestEvent != null)
		{
			this.RestoreRequestEvent(this, new EventArgs());
		}
	}

	protected virtual void FireClickEvent()
	{
		if (this.ClickEvent != null)
		{
			this.ClickEvent(this, new EventArgs());
		}
	}

	protected virtual void FireDoubleClickEvent()
	{
		if (this.DoubleClickEvent != null)
		{
			this.DoubleClickEvent(this, new EventArgs());
		}
	}

	public ViewItemUserControl()
	{
		new TrackPageViewData(typeof(ViewItemUserControl), deprecated: true).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void Print(string name, string information, string cameraName)
	{
		using Bitmap bitmap = new Bitmap(base.Width, base.Height);
		DrawToBitmap(bitmap, new Rectangle(0, 0, base.Width, base.Height));
		ClientControl.Instance.Print(bitmap, name, information, cameraName);
	}

	public virtual void Print(string name, string information)
	{
		Print(name, information, "");
	}

	public virtual void Print()
	{
		Print(base.Name, "");
	}
}
