using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class BitmapConverter : MarkupExtension, IValueConverter
{
	private static readonly Cache<System.Drawing.Image, BitmapSource> _imageSourceCache = new Cache<System.Drawing.Image, BitmapSource>(TimeSpan.FromMinutes(15.0));

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is byte[] buffer)
		{
			using MemoryStream streamSource = new MemoryStream(buffer);
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = streamSource;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
			return bitmapImage;
		}
		if (value is System.Drawing.Image drawingImage)
		{
			BitmapSource bitmapSource = ConvertImageToBitmapSource(drawingImage, ignoreCache: false);
			if (parameter != null)
			{
				Type type = parameter as Type;
				if (type != null && type == typeof(System.Windows.Controls.Image))
				{
					return new System.Windows.Controls.Image
					{
						Source = bitmapSource
					};
				}
			}
			return bitmapSource;
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Binding.DoNothing;
	}

	public static BitmapSource ConvertImageToBitmapSource(System.Drawing.Image drawingImage, bool ignoreCache)
	{
		if (drawingImage != null)
		{
			if (ignoreCache)
			{
				return ConvertImageToBitmapSourceCore(drawingImage);
			}
			return _imageSourceCache.GetOrAdd(drawingImage, (System.Drawing.Image di) => ConvertImageToBitmapSourceCore(di));
		}
		return null;
	}

	public static BitmapSource ConvertImageToBitmapSource(System.Drawing.Image drawingImage)
	{
		return ConvertImageToBitmapSource(drawingImage, ignoreCache: false);
	}

	private static BitmapSource ConvertImageToBitmapSourceCore(System.Drawing.Image drawingImage)
	{
		using MemoryStream memoryStream = new MemoryStream();
		drawingImage.Save(memoryStream, ImageFormat.Png);
		using (new MemoryStream(memoryStream.ToArray()))
		{
			return BitmapSourceFromStream(memoryStream);
		}
	}

	public static BitmapSource BitmapSourceFromStream(Stream stream)
	{
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.None;
		bitmapImage.StreamSource = stream;
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return CopyBitmapImageNoRef(bitmapImage);
	}

	public static BitmapSource ImageSourceFromUri(Uri uri)
	{
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.UriSource = uri;
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return CopyBitmapImageNoRef(bitmapImage);
	}

	private static BitmapSource CopyBitmapImageNoRef(BitmapImage bitmapImage)
	{
		int num = bitmapImage.PixelWidth * (bitmapImage.Format.BitsPerPixel / 8);
		byte[] pixels = new byte[num * bitmapImage.PixelHeight];
		bitmapImage.CopyPixels(pixels, num, 0);
		WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight, bitmapImage.DpiX, bitmapImage.DpiY, bitmapImage.Format, bitmapImage.Palette);
		writeableBitmap.WritePixels(new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight), pixels, num, 0);
		writeableBitmap.Freeze();
		return writeableBitmap;
	}

	public static BitmapSource ExtractFrameFromIcon(Icon inputImage, double imageWidth)
	{
		MemoryStream memoryStream = new MemoryStream();
		inputImage.Save(memoryStream);
		IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.None);
		BitmapFrame bitmapFrame = iconBitmapDecoder.Frames.SingleOrDefault((BitmapFrame f) => f.Width == imageWidth);
		if (bitmapFrame == null)
		{
			bitmapFrame = iconBitmapDecoder.Frames.OrderBy((BitmapFrame f) => f.Width).First();
		}
		bitmapFrame.Freeze();
		return bitmapFrame;
	}
}
