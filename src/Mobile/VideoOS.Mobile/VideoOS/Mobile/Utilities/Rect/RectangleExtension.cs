using System.Drawing;
using VideoOS.Mobile.Portable.Utilities.Rects;

namespace VideoOS.Mobile.Utilities.Rect;

public static class RectangleExtension
{
	public static RectangleF ToRectnagleF(this IntRect tIntRect)
	{
		return new RectangleF(tIntRect.X, tIntRect.Y, tIntRect.Width, tIntRect.Height);
	}
}
