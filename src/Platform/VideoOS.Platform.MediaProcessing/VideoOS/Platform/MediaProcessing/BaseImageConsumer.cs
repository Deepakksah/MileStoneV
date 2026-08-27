using System;

namespace VideoOS.Platform.MediaProcessing;

public abstract class BaseImageConsumer : IImageConsumer
{
	public abstract void OnImage(Image image);

	public virtual void OnError()
	{
	}

	public virtual void OnException(Exception ex)
	{
		throw ex;
	}

	public BaseImageConsumer()
	{
	}
}
