using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace VideoOS.Platform.Data;

public abstract class BitmapData : CroppingInfo, IDisposable
{
	public DateTime DateTime;

	public bool IsNextAvailable;

	public DateTime NextDateTime;

	public bool IsPreviousAvailable;

	public DateTime PreviousDateTime;

	private string _hardwareDecodingStatus = "ipp";

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

	public abstract BitmapFormat Format { get; }

	public abstract BitmapColorSpace ColorSpace { get; }

	public abstract double PixelAspectRatio { get; }

	public IDisposable ObjectToDispose { get; set; }

	public BitmapVideoSource BitmapPlaybackSource { get; set; }

	public Bitmap GetBitmap()
	{
		int planeWidth = GetPlaneWidth(0);
		int planeHeight = GetPlaneHeight(0);
		int planeStride = GetPlaneStride(0);
		IntPtr planePointer = GetPlanePointer(0);
		return new Bitmap(planeWidth, planeHeight, planeStride, PixelFormat.Format24bppRgb, planePointer);
	}

	public BitmapSource ConvertToBitmapSource()
	{
		using Bitmap bitmap = GetBitmap();
		using MemoryStream memoryStream = new MemoryStream();
		bitmap.Save(memoryStream, ImageFormat.Bmp);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.StreamSource = memoryStream;
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return bitmapImage;
	}

	public abstract byte[] GetPlaneBytes(int plane);

	public abstract int GetPlaneHeight(int plane);

	public abstract int GetPlaneWidth(int plane);

	public abstract int GetPlaneStride(int plane);

	public abstract IntPtr GetPlanePointer(int plane);

	public void Dispose(bool native)
	{
		if (ObjectToDispose != null)
		{
			ObjectToDispose.Dispose();
			ObjectToDispose = null;
		}
	}

	public void Dispose()
	{
		Dispose(native: true);
	}
}
