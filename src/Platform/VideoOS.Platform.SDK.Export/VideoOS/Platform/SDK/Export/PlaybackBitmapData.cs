using System;
using VideoOS.Platform.Data;
using VideoOS.Toolkit;

namespace VideoOS.Platform.SDK.Export;

public class PlaybackBitmapData : BitmapData
{
	private IImage _image;

	public override BitmapColorSpace ColorSpace => _image.ColorSpaceDetail switch
	{
		IImage.ImageColorSpaceDetail.NotApplicable => BitmapColorSpace.NotApplicable, 
		IImage.ImageColorSpaceDetail.YCbCr_JFIF => BitmapColorSpace.YCbCr_JFIF, 
		IImage.ImageColorSpaceDetail.YCbCr_RecBT601 => BitmapColorSpace.YCbCr_RecBT601, 
		IImage.ImageColorSpaceDetail.YCbCr_FullRange => BitmapColorSpace.YCbCr_FullRange, 
		_ => BitmapColorSpace.NotApplicable, 
	};

	public override BitmapFormat Format => _image.Format switch
	{
		IImage.ImageFormat.Gray => BitmapFormat.Gray, 
		IImage.ImageFormat.RGB24 => BitmapFormat.RGB24, 
		IImage.ImageFormat.RGB32 => BitmapFormat.RGB32, 
		IImage.ImageFormat.RGBA => BitmapFormat.RGBA, 
		IImage.ImageFormat.BGR24 => BitmapFormat.BGR24, 
		IImage.ImageFormat.BGR32 => BitmapFormat.BGR32, 
		IImage.ImageFormat.BGRA => BitmapFormat.BGRA, 
		IImage.ImageFormat.YCbCr420_Planar => BitmapFormat.YCbCr420_Planar, 
		IImage.ImageFormat.YCbCr422_Planar => BitmapFormat.YCbCr422_Planar, 
		IImage.ImageFormat.YCbCr444_Planar => BitmapFormat.YCbCr444_Planar, 
		_ => BitmapFormat.RGB24, 
	};

	public override double PixelAspectRatio => _image.PixelAspectRatio;

	public PlaybackBitmapData(IImage image)
	{
		_image = image;
	}

	public override IntPtr GetPlanePointer(int plane)
	{
		return _image.GetPlanePointer(plane);
	}

	public override byte[] GetPlaneBytes(int plane)
	{
		return _image.GetPlane(plane);
	}

	public override int GetPlaneHeight(int plane)
	{
		return _image.GetPlaneHeight(plane);
	}

	public override int GetPlaneStride(int plane)
	{
		return _image.GetPlaneStride(plane);
	}

	public override int GetPlaneWidth(int plane)
	{
		return _image.GetPlaneWidth(plane);
	}
}
