using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(LineFrame), "VideoOS.Controls.UI.Wizards.LineFrame.bmp")]
[Designer("VideoOS.UI.Controls.Designers.LineFrameDesigner, VideoOS.Controls.UI.Designers, Version=1.0.4.0, Culture=neutral, PublicKeyToken=fe06e967d3cf723d")]
public class LineFrame : Control
{
	public enum TLineStyle
	{
		Horizontal,
		Vertical
	}

	private static Point m_pntStart = new Point(0, 1);

	private Point m_pntEnd = new Point(0, 1);

	private TLineStyle m_enStyle;

	[Category("Appearance")]
	[DefaultValue(typeof(TLineStyle), "Horizontal")]
	[Description("GET/SET style of line")]
	public TLineStyle LineStyle
	{
		get
		{
			return m_enStyle;
		}
		set
		{
			if (value != m_enStyle)
			{
				m_enStyle = value;
				OnLineStyleChanged();
			}
		}
	}

	protected override Size DefaultSize
	{
		get
		{
			if (m_enStyle == TLineStyle.Horizontal)
			{
				return new Size(75, 4);
			}
			return new Size(4, 75);
		}
	}

	public LineFrame()
	{
		ControlStyles flag = ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.FixedHeight | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer;
		ControlStyles flag2 = ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor;
		SetStyle(flag, value: true);
		SetStyle(flag2, value: false);
		base.Size = new Size(75, 4);
		base.TabStop = false;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	protected virtual void OnLineStyleChanged()
	{
		SetStyle(ControlStyles.FixedHeight, m_enStyle == TLineStyle.Horizontal);
		SetStyle(ControlStyles.FixedWidth, m_enStyle == TLineStyle.Vertical);
		m_pntEnd.X = 1;
		m_pntEnd.Y = base.Height;
		Invalidate();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);
		if (m_enStyle == TLineStyle.Horizontal)
		{
			base.Size = new Size(base.Size.Width, 4);
			m_pntEnd.X = base.Width;
			m_pntEnd.Y = 1;
		}
		else
		{
			base.Size = new Size(4, base.Size.Height);
			m_pntEnd.X = 1;
			m_pntEnd.Y = base.Height;
		}
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		if (pe == null)
		{
			throw new ArgumentNullException("pe");
		}
		Draw3DLine(pe.Graphics, (m_enStyle == TLineStyle.Horizontal) ? m_pntStart : new Point(1, 0), m_pntEnd);
	}

	private static void Draw3DLine(Graphics graph, Point pnt1, Point pnt2)
	{
		Pen pen = new Pen(SystemColors.ControlDarkDark);
		Pen pen2 = new Pen(SystemColors.ControlLightLight);
		Point[] array = new Point[2] { pnt1, pnt2 };
		graph.DrawLine(pen2, pnt1, pnt2);
		if (pnt1.X == pnt2.X)
		{
			array[0].X--;
			array[1].X--;
		}
		else if (pnt1.Y == pnt2.Y)
		{
			array[0].Y--;
			array[1].Y--;
		}
		else
		{
			array[0].X--;
			array[0].Y--;
			array[1].X--;
			array[1].Y--;
		}
		graph.DrawLine(pen, array[0], array[1]);
		pen.Dispose();
		pen2.Dispose();
	}
}
