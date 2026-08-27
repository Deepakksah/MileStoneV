using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VideoOS.UI.Common.WPF.Utils.ValueConverters;

public class SilentByteToImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType != typeof(ImageSource))
		{
			throw new InvalidOperationException("The target must be an Image.");
		}
		if (value != null)
		{
			using MemoryStream bitmapStream = new MemoryStream((byte[])value);
			try
			{
				BitmapFrame bitmapFrame = new JpegBitmapDecoder(bitmapStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0];
				bitmapFrame.Freeze();
				return bitmapFrame;
			}
			catch (FileFormatException)
			{
			}
			catch (OutOfMemoryException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}
		return new Image();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
