using System.Drawing;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.Util.Images;

namespace VideoOS.Platform.UI.Extensions;

public static class DrawingImageExtensionMethods
{
	public static VideoOSIconBitmapSource ToIconBitmapSource(this Image image)
	{
		if (image != null)
		{
			return new VideoOSIconBitmapSource
			{
				BitmapSource = VideoOS.Platform.Util.Images.ImageConverter.ConvertImage(image)
			};
		}
		return null;
	}
}
