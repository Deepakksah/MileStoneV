using System;

namespace VideoOS.Platform.MediaProcessing;

public interface IImageConsumer
{
	void OnImage(Image image);

	void OnError();

	void OnException(Exception ex);
}
