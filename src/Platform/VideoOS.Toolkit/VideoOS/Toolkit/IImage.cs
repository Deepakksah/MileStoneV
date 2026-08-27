using System;

namespace VideoOS.Toolkit;

public interface IImage : IMediaData
{
	public enum ImageFormat
	{
		Gray,
		RGB24,
		BGR24,
		RGB32,
		BGR32,
		YCbCr420_Planar,
		YCbCr422_Planar,
		YCbCr444_Planar,
		RGBA,
		BGRA
	}

	public enum ImageColorSpaceDetail
	{
		NotApplicable,
		YCbCr_JFIF,
		YCbCr_RecBT601,
		YCbCr_FullRange
	}

	int Height { get; }

	int Width { get; }

	double PixelAspectRatio { get; }

	ImageColorSpaceDetail ColorSpaceDetail { get; }

	ImageFormat Format { get; }

	int GetPlaneWidth(int planeIndex);

	int GetPlaneHeight(int planeIndex);

	int GetPlaneStride(int planeIndex);

	byte[] GetPlane(int planeIndex);

	IntPtr GetPlanePointer(int planeIndex);
}
