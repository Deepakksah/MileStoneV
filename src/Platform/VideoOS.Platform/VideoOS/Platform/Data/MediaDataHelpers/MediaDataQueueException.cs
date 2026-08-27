using System;

namespace VideoOS.Platform.Data.MediaDataHelpers;

[Serializable]
public class MediaDataQueueException : Exception
{
	public MediaDataQueueException(string message)
		: base(message)
	{
	}
}
