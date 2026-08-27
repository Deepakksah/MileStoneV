using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VideoOS.Common.Controls;

public class WizardWelcomePageDesigner : ParentControlDesigner
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
		properties.Remove("WelcomePage");
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
		if (wizardPageBase == null)
		{
			return;
		}
		WizardForm wizardPageParent = wizardPageBase.WizardPageParent;
		string text;
		if (wizardPageBase != null && wizardPageParent != null)
		{
			text = (wizardPageParent.Pages.IndexOf(wizardPageBase) + 1).ToString();
		}
		else
		{
			text = "-";
			if (wizardPageBase == null)
			{
				text += "p";
			}
			if (wizardPageParent == null)
			{
				text += "P";
			}
		}
		graphics.FillRectangle(m_brushBack, m_rect);
		StringFormat stringFormat = new StringFormat();
		stringFormat.Alignment = StringAlignment.Center;
		stringFormat.LineAlignment = StringAlignment.Center;
		graphics.DrawString(text, m_font, m_brushText, m_rect, stringFormat);
	}
}
