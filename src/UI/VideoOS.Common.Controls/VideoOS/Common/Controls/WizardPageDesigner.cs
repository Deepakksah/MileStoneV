using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VideoOS.Common.Controls;

public class WizardPageDesigner : ParentControlDesigner
{
	private static SolidBrush m_brushBack = new SolidBrush(Color.Navy);

	private static SolidBrush m_brushText = new SolidBrush(Color.White);

	private static Font m_font = new Font(FontFamily.GenericSansSerif, 10f);

	private static RectangleF m_rect = new RectangleF(0f, 0f, 32f, 16f);

	private bool m_bShowLines = true;

	public bool ShowPageLimits
	{
		get
		{
			return m_bShowLines;
		}
		set
		{
			if (value != m_bShowLines)
			{
				m_bShowLines = value;
				if (base.Control != null)
				{
					base.Control.Invalidate();
				}
			}
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		base.DrawGrid = false;
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		properties.Remove("Dock");
		properties.Remove("Anchor");
		properties.Remove("AutoScroll");
		properties.Remove("AutoScrollMargin");
		properties.Remove("AutoScrollMinSize");
		properties.Remove("DockPadding");
		properties.Remove("Location");
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		base.OnPaintAdornments(pe);
		if (pe == null)
		{
			throw new ArgumentNullException("pe");
		}
		Graphics graphics = pe.Graphics;
		WizardPageBase wizardPageBase = Control as WizardPageBase;
		if (wizardPageBase == null)
		{
			wizardPageBase = base.Component as WizardPageBase;
		}
		WizardForm wizardForm = wizardPageBase?.WizardPageParent;
		string text;
		if (wizardPageBase != null && wizardForm != null)
		{
			text = (wizardForm.Pages.IndexOf(wizardPageBase) + 1).ToString();
		}
		else
		{
			text = "-";
			if (wizardPageBase == null)
			{
				text += "p";
			}
			if (wizardForm == null)
			{
				text += "P";
			}
		}
		graphics.FillRectangle(m_brushBack, m_rect);
		StringFormat stringFormat = new StringFormat();
		stringFormat.Alignment = StringAlignment.Center;
		stringFormat.LineAlignment = StringAlignment.Center;
		graphics.DrawString(text, m_font, m_brushText, m_rect, stringFormat);
		if (wizardForm != null)
		{
			Rectangle currentPageRect = wizardForm.GetCurrentPageRect();
			if (currentPageRect.Width % 8 > 0)
			{
				currentPageRect.Width = currentPageRect.Width / 8 * 8;
			}
			if (currentPageRect.Height % 8 > 0)
			{
				currentPageRect.Height = currentPageRect.Height / 8 * 8;
			}
			if (wizardPageBase != null && !wizardPageBase.WelcomePage && m_bShowLines)
			{
				Point[] points = new Point[15]
				{
					new Point(new Size(currentPageRect.X + 24, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.X + 24, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.X + 40, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.X + 40, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.X + 56, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.X + 56, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.Right - 56, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.Right - 56, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.Right - 40, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.Right - 40, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.Right - 24, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.Right - 24, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.X + 24, currentPageRect.Y + 8)),
					new Point(new Size(currentPageRect.X + 24, currentPageRect.Bottom - 8)),
					new Point(new Size(currentPageRect.Right - 24, currentPageRect.Bottom - 8))
				};
				Pen pen = new Pen(Brushes.Black);
				pen.DashStyle = DashStyle.DashDotDot;
				graphics.DrawLines(pen, points);
				pen.Dispose();
			}
		}
	}
}
