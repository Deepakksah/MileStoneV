using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities.UI;

public class CustomProgressBar : ProgressBar
{
	public ProgressBarDisplayText DisplayStyle { get; set; }

	public string CustomText { get; set; }

	public CustomProgressBar()
	{
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, value: true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle clientRectangle = base.ClientRectangle;
		Graphics graphics = e.Graphics;
		try
		{
			ProgressBarRenderer.DrawHorizontalBar(graphics, clientRectangle);
			clientRectangle.Inflate(-3, -3);
			if (base.Value > 0)
			{
				Rectangle bounds = new Rectangle(clientRectangle.X, clientRectangle.Y, (int)Math.Round((float)base.Value / (float)base.Maximum * (float)clientRectangle.Width), clientRectangle.Height);
				ProgressBarRenderer.DrawHorizontalChunks(graphics, bounds);
			}
		}
		catch (Exception)
		{
			base.OnPaint(e);
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((DisplayStyle & ProgressBarDisplayText.CustomText) == ProgressBarDisplayText.CustomText)
			{
				stringBuilder.Append(CustomText);
			}
			if ((DisplayStyle & ProgressBarDisplayText.Percentage) == ProgressBarDisplayText.Percentage)
			{
				stringBuilder.Append(" ").Append(base.Value).Append('%');
			}
			using Font font = new Font(base.Font.FontFamily, base.Font.Size);
			string s = stringBuilder.ToString();
			SizeF sizeF = graphics.MeasureString(s, font);
			Point point = new Point(Convert.ToInt32((float)(base.Width / 2) - sizeF.Width / 2f), Convert.ToInt32((float)(base.Height / 2) - sizeF.Height / 2f));
			using Brush brush = new SolidBrush(base.Parent.ForeColor);
			graphics.DrawString(s, font, brush, point);
		}
		catch (Exception)
		{
		}
	}

	[DllImport("uxtheme.dll")]
	private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

	[DllImport("uxtheme.dll")]
	private static extern bool IsThemeActive();

	[DllImport("uxtheme.dll")]
	private static extern bool IsAppThemed();

	protected override void OnHandleCreated(EventArgs e)
	{
		if (IsAppThemed() && IsThemeActive())
		{
			SetWindowTheme(base.Handle, "", "");
		}
		base.OnHandleCreated(e);
	}
}
