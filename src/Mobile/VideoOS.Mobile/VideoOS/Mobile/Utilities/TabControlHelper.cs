using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities;

public class TabControlHelper
{
	public static void DrawTabControlTabs(TabControl tabControl, DrawItemEventArgs e, ImageList images)
	{
		Rectangle tabRect = tabControl.GetTabRect(tabControl.TabPages.Count - 1);
		RectangleF rectangleF = new RectangleF(tabRect.X + tabRect.Width, tabRect.Y - 5, tabControl.Width - (tabRect.X + tabRect.Width), tabRect.Height + 5);
		if (tabControl.Parent.BackgroundImage != null)
		{
			RectangleF srcRect = new RectangleF(rectangleF.X + (float)tabControl.Left, rectangleF.Y + (float)tabControl.Top, rectangleF.Width, rectangleF.Height);
			e.Graphics.DrawImage(tabControl.Parent.BackgroundImage, rectangleF, srcRect, GraphicsUnit.Pixel);
		}
		else
		{
			using Brush brush = new SolidBrush(tabControl.Parent.BackColor);
			e.Graphics.FillRectangle(brush, rectangleF);
		}
		TabPage tabPage = tabControl.TabPages[e.Index];
		Brush brush2 = new SolidBrush(tabPage.BackColor);
		Brush brush3 = new SolidBrush(tabPage.ForeColor);
		string text = tabPage.Text;
		int num = 0;
		Rectangle bounds = e.Bounds;
		e.Graphics.FillRectangle(brush2, bounds);
		if (images != null)
		{
			Size imageSize = images.ImageSize;
			Image image = null;
			if (tabPage.ImageIndex > -1)
			{
				image = images.Images[tabPage.ImageIndex];
			}
			else if (tabPage.ImageKey != "")
			{
				image = images.Images[tabPage.ImageKey];
			}
			if (image != null)
			{
				Point location = new Point(bounds.X + 2 + (bounds.Height - imageSize.Height) / 2, bounds.Y + 2 + (bounds.Height - imageSize.Height) / 2);
				e.Graphics.DrawImage(image, new Rectangle(location, imageSize));
				num = imageSize.Width + 4;
			}
		}
		Rectangle rectangle = new Rectangle(bounds.X + num, bounds.Y + 3, bounds.Width - num, bounds.Height - 3);
		StringFormat stringFormat = new StringFormat();
		stringFormat.Alignment = StringAlignment.Center;
		e.Graphics.DrawString(text, e.Font, brush3, rectangle, stringFormat);
		stringFormat.Dispose();
		brush2.Dispose();
		brush3.Dispose();
	}
}
