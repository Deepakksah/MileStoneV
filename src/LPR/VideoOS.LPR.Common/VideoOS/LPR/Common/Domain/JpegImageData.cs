using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace VideoOS.LPR.Common.Domain;

public class JpegImageData : IJpegImageData
{
	private readonly byte[] _jpegBytes;

	private BitmapImage _bitmapImage;

	public IEnumerable<byte> Bytes => _jpegBytes;

	public Size Size => new Size(ImageSource.PixelWidth, ImageSource.PixelHeight);

	public BitmapSource ImageSource
	{
		get
		{
			if (_bitmapImage == null)
			{
				using MemoryStream streamSource = new MemoryStream(_jpegBytes);
				_bitmapImage = new BitmapImage();
				_bitmapImage.BeginInit();
				_bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				_bitmapImage.StreamSource = streamSource;
				_bitmapImage.EndInit();
				_bitmapImage.Freeze();
			}
			return _bitmapImage;
		}
	}

	public JpegImageData(byte[] jpegData)
	{
		if (jpegData == null)
		{
			throw new ArgumentNullException("jpegData");
		}
		_jpegBytes = jpegData;
	}

	public JpegImageData(Image image)
	{
		if (image == null)
		{
			throw new ArgumentNullException("image");
		}
		using MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Jpeg);
		_jpegBytes = memoryStream.ToArray();
	}
}
