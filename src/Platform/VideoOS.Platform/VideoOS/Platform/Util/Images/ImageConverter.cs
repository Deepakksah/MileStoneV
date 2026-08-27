using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Util.Images;

public class ImageConverter
{
	public static BitmapSource ConvertImage(Image image)
	{
		try
		{
			if (image != null)
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				MemoryStream memoryStream = new MemoryStream();
				image.Save(memoryStream, ImageFormat.Png);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				return bitmapImage;
			}
		}
		catch
		{
		}
		return null;
	}

	public static VideoOSIconSourceBase ConvertIconToIconSourceBase(Icon icon)
	{
		if (icon != null)
		{
			return new VideoOSIconBitmapSource
			{
				BitmapSource = GetBitmapSource(icon, 24.0, () => (BitmapSource)null)
			};
		}
		return null;
	}

	private static BitmapSource GetBitmapSource(Icon icon, double desiredSize, Func<BitmapSource> fallbackImage)
	{
		if (icon == null)
		{
			return fallbackImage();
		}
		using MemoryStream memoryStream = new MemoryStream();
		icon.Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return GetClosestFrameOrFallback(BitmapDecoder.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames, desiredSize, fallbackImage);
	}

	private static BitmapSource GetClosestFrameOrFallback(ReadOnlyCollection<BitmapFrame> frames, double desiredSize, Func<BitmapSource> fallbackImage)
	{
		if (frames.Count == 0)
		{
			return fallbackImage();
		}
		BitmapFrame bitmapFrame = frames.Aggregate((BitmapFrame curMin, BitmapFrame i) => (curMin != null && !(Math.Abs(i.Width - desiredSize) < Math.Abs(curMin.Width - desiredSize))) ? curMin : i);
		bitmapFrame.Freeze();
		return bitmapFrame;
	}
}
