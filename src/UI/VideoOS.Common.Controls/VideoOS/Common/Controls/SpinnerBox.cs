using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class SpinnerBox : Control
{
	public enum SCROLLGRADIENTALIGN
	{
		HORIZONTAL,
		VERTICAL
	}

	private const int DEF_WIDTH = 50;

	private const int DEF_HEIGHT = 18;

	private Color _color1;

	private Color _color2;

	private Bitmap _bitmap;

	private int xloc;

	private int _speed = 5;

	private SCROLLGRADIENTALIGN _scrollway;

	private IContainer components;

	private Timer updateTimer;

	[Description("Gets or sets the Interval of timer tick count for Speed of GradientWaitingBar")]
	[Category("Behavior")]
	public int Interval
	{
		get
		{
			return updateTimer.Interval;
		}
		set
		{
			updateTimer.Interval = value;
		}
	}

	[Description("Gets or sets the speed of gradient for GradientWaitingBar")]
	[Category("Behavior")]
	public int Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
		}
	}

	[Description("Gets or sets the starting color of the gradient for GradientWaitingBar")]
	[Category("Appearance")]
	public Color GradientColor1
	{
		get
		{
			return _color1;
		}
		set
		{
			_color1 = value;
			MakeBitmapForWaitingBar();
		}
	}

	[Description("Gets or sets the ending color of the gradient for GradientWaitingBar")]
	[Category("Appearance")]
	public Color GradientColor2
	{
		get
		{
			return _color2;
		}
		set
		{
			_color2 = value;
			MakeBitmapForWaitingBar();
		}
	}

	[Description("Gets or sets the direction of scrolling the gradient")]
	[Category("Appearance")]
	public SCROLLGRADIENTALIGN ScrollWay
	{
		get
		{
			return _scrollway;
		}
		set
		{
			_scrollway = value;
			MakeBitmapForWaitingBar();
		}
	}

	public SpinnerBox()
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
		SetStyle(ControlStyles.DoubleBuffer, value: true);
		SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
		SetStyle(ControlStyles.UserPaint, value: true);
		SetStyle(ControlStyles.ResizeRedraw, value: true);
		base.Left = 0;
		base.Top = 0;
		base.Width = 50;
		base.Height = 18;
		InitializeComponent();
		_color1 = Color.White;
		_color2 = Color.Blue;
		BackColor = _color2;
		MakeBitmapForWaitingBar();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		MakeBitmapForWaitingBar();
		base.OnSizeChanged(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_bitmap != null)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			if (_scrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
			{
				clientRectangle.X = clientRectangle.Width - xloc;
				clientRectangle.Width = xloc;
				graphics.DrawImage(_bitmap, xloc, 0, base.ClientRectangle, GraphicsUnit.Pixel);
				graphics.DrawImage(_bitmap, 0, 0, clientRectangle, GraphicsUnit.Pixel);
			}
			else
			{
				clientRectangle.Y = clientRectangle.Height - xloc;
				clientRectangle.Height = xloc;
				graphics.DrawImage(_bitmap, 0, xloc, base.ClientRectangle, GraphicsUnit.Pixel);
				graphics.DrawImage(_bitmap, 0, 0, clientRectangle, GraphicsUnit.Pixel);
			}
		}
		base.OnPaint(e);
	}

	private void updateTimer_Tick(object sender, EventArgs e)
	{
		xloc += _speed;
		if (_scrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
		{
			if (xloc > base.ClientSize.Width)
			{
				xloc = 0;
			}
		}
		else if (xloc > base.ClientSize.Height)
		{
			xloc = 0;
		}
		Invalidate();
	}

	private void MakeBitmapForWaitingBar()
	{
		if (base.ClientRectangle.Width != 0 && base.ClientRectangle.Height != 0)
		{
			if (_bitmap != null)
			{
				_bitmap.Dispose();
			}
			Graphics graphics = null;
			graphics = Graphics.FromHwnd(base.Handle);
			_bitmap = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height, graphics);
			graphics.Dispose();
			Rectangle clientRectangle;
			Rectangle rect;
			Brush brush;
			Brush brush2;
			if (_scrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
			{
				clientRectangle = base.ClientRectangle;
				clientRectangle.Width = base.ClientRectangle.Width / 2 + 1;
				rect = clientRectangle;
				rect.X = base.ClientRectangle.Width / 2;
				rect.Width = base.ClientRectangle.Width / 2 + 1;
				brush = new LinearGradientBrush(clientRectangle, _color1, _color2, LinearGradientMode.Horizontal);
				brush2 = new LinearGradientBrush(rect, _color2, _color1, LinearGradientMode.Horizontal);
			}
			else
			{
				clientRectangle = base.ClientRectangle;
				clientRectangle.Height = base.ClientRectangle.Height / 2 + 1;
				rect = clientRectangle;
				rect.Y = base.ClientRectangle.Height / 2;
				rect.Height = base.ClientRectangle.Height / 2 + 1;
				brush = new LinearGradientBrush(clientRectangle, _color1, _color2, LinearGradientMode.Vertical);
				brush2 = new LinearGradientBrush(rect, _color2, _color1, LinearGradientMode.Vertical);
			}
			Graphics graphics2 = Graphics.FromImage(_bitmap);
			graphics2.FillRectangle(brush2, rect);
			graphics2.FillRectangle(brush, clientRectangle);
			graphics2.Dispose();
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
		this.updateTimer = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.updateTimer.Enabled = true;
		this.updateTimer.Tick += new System.EventHandler(updateTimer_Tick);
		base.ResumeLayout(false);
	}
}
