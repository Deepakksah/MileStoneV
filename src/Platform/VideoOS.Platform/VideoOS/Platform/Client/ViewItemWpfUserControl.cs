using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class ViewItemWpfUserControl : UserControl
{
	public virtual bool Selected { get; set; }

	public virtual bool Maximized { get; set; }

	public virtual bool Hidden { get; set; }

	public virtual bool Selectable => true;

	public virtual bool Maximizable => true;

	public virtual bool ShowToolbar => true;

	public virtual int CalculateHeaderHeight
	{
		get
		{
			if (base.Height < 20.0)
			{
				return 0;
			}
			if (base.Height < 350.0)
			{
				return 13;
			}
			return 23;
		}
	}

	public virtual double CalculateHeaderFontSize
	{
		get
		{
			if (base.Height < 350.0)
			{
				return 10.0;
			}
			return 16.0;
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

	public ViewItemWpfUserControl()
	{
		new TrackPageViewData(typeof(ViewItemWpfUserControl)).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void Print(string name, string information, string cameraName)
	{
		Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this);
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)descendantBounds.Width, (int)descendantBounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
		DrawingVisual drawingVisual = new DrawingVisual();
		using (DrawingContext drawingContext = drawingVisual.RenderOpen())
		{
			VisualBrush brush = new VisualBrush(this);
			drawingContext.DrawRectangle(brush, null, new Rect(default(Point), descendantBounds.Size));
		}
		renderTargetBitmap.Render(drawingVisual);
		ClientControl.Instance.Print(BitmapFrame.Create(renderTargetBitmap), name, information, cameraName);
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
