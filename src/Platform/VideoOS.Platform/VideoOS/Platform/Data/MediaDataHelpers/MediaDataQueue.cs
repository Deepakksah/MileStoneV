using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Util.Error;

namespace VideoOS.Platform.Data.MediaDataHelpers;

public class MediaDataQueue
{
	private readonly ErrorHandler<Exception> _errorHandler = new ErrorHandler<Exception>();

	protected Queue<MediaData> MediaQueue { get; } = new Queue<MediaData>();

	protected object MediaQueueLocker { get; } = new object();

	protected DateTime LastSentDateTime { get; set; } = DateTime.MinValue;

	public TimeSpan MaxBufferSize { get; set; } = TimeSpan.FromSeconds(2.0);

	public IErrorHandler<Exception> ErrorHandler => _errorHandler;

	public void OnData(MediaData mediaData)
	{
		lock (MediaQueueLocker)
		{
			MediaQueue.Enqueue(mediaData);
			DequeueToThreshold();
		}
	}

	private void DequeueToThreshold()
	{
		List<MediaData> list = new List<MediaData>();
		while (MaxBufferSize < GetTimeSpan(MediaQueue))
		{
			list.Add(MediaQueue.Dequeue());
		}
		if (list.Any())
		{
			_errorHandler.LastError = new MediaDataQueueOverflowException(GetTimeSpan(list), list.Count);
		}
	}

	public virtual MediaData GetMediaData(MediaProviderSession session)
	{
		lock (MediaQueueLocker)
		{
			LastSentDateTime = DateTime.UtcNow;
			if (MediaQueue.Any())
			{
				return MediaQueue.Dequeue();
			}
			return null;
		}
	}

	protected virtual TimeSpan GetTimeSpan(IEnumerable<MediaData> buffers)
	{
		if (!buffers.Any())
		{
			return TimeSpan.Zero;
		}
		return buffers.Last().DateTime - buffers.First().DateTime;
	}

	public void Reset()
	{
		lock (MediaQueueLocker)
		{
			MediaQueue.Clear();
		}
		LastSentDateTime = DateTime.UtcNow;
	}
}
