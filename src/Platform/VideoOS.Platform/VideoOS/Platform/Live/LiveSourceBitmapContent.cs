using System;

namespace VideoOS.Platform.Live;

public abstract class LiveSourceBitmapContent : LiveSourceContent
{
	public abstract BitmapFormat Format { get; }

	public abstract BitmapColorSpace ColorSpace { get; }

	public abstract double PixelAspectRatio { get; }

	public IDisposable ObjectToDispose { get; set; }

	public abstract byte[] GetPlaneBytes(int plane);

	public abstract int GetPlaneHeight(int plane);

	public abstract int GetPlaneWidth(int plane);

	public abstract int GetPlaneStride(int plane);

	public abstract IntPtr GetPlanePointer(int plane);

	public override void Dispose()
	{
		if (ObjectToDispose != null)
		{
			ObjectToDispose.Dispose();
			ObjectToDispose = null;
		}
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
