using System;

namespace VideoOS.Platform.Data;

public class BitmapDataResult
{
	public bool FailedToRetrieveImage { get; }

	public bool NoImageAvailable { get; }

	public Exception Exception { get; }

	public BitmapData BitmapData { get; }

	public BitmapDataResult(BitmapData bitmapData)
	{
		BitmapData = bitmapData;
	}

	public BitmapDataResult(bool failedToRetrieveImage, bool noImageAvailable)
	{
		FailedToRetrieveImage = failedToRetrieveImage;
		NoImageAvailable = noImageAvailable;
	}

	public BitmapDataResult(bool failedToRetrieveImage, bool noImageAvailable, Exception exception)
		: this(failedToRetrieveImage, noImageAvailable)
	{
		Exception = exception;
	}
}
