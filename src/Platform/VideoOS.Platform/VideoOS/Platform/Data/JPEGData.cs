using System;
using System.IO;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class JPEGData
{
	public DateTime DateTime;

	public byte[] Bytes;

	public bool IsNextAvailable;

	public DateTime NextDateTime;

	public bool IsPreviousAvailable;

	public DateTime PreviousDateTime;

	private string _hardwareDecodingStatus = "ipp";

	private int _width;

	private int _height;

	public virtual string HardwareDecodingStatus
	{
		get
		{
			return _hardwareDecodingStatus;
		}
		set
		{
			_hardwareDecodingStatus = value;
		}
	}

	public int Width
	{
		get
		{
			if (_width == 0)
			{
				JpegUtil.FindWidthHeight(Bytes, out _width, out _height);
			}
			return _width;
		}
	}

	public int Height
	{
		get
		{
			if (_height == 0)
			{
				JpegUtil.FindWidthHeight(Bytes, out _width, out _height);
			}
			return _height;
		}
	}

	public bool CroppingDefined { get; set; }

	public double CropX { get; set; }

	public double CropY { get; set; }

	public double CropWidth { get; set; }

	public double CropHeight { get; set; }

	public object CustomData { get; set; }

	public BitmapSource ConvertToBitmapSource()
	{
		if (Bytes == null)
		{
			return null;
		}
		using MemoryStream streamSource = new MemoryStream(Bytes);
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.StreamSource = streamSource;
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return bitmapImage;
	}
}
