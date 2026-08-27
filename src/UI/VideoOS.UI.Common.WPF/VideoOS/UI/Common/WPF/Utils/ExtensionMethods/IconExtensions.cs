using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class IconExtensions
{
	public static ImageSource GetImageSource(this Icon icon, double desiredSize, Func<ImageSource> fallbackImage)
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

	private static ImageSource GetClosestFrameOrFallback(ReadOnlyCollection<BitmapFrame> frames, double desiredSize, Func<ImageSource> fallbackImage)
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
