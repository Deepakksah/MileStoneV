using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data.MediaDataHelpers;

[Serializable]
public class MediaDataQueueOverflowException : Exception
{
	public TimeSpan DataSkipped { get; private set; }

	public int SkippedItemsCount { get; private set; }

	public MediaDataQueueOverflowException(TimeSpan dataSkipped, int skipItemsCount)
		: base("Buffer overflow in Audio Media Data Queue")
	{
		DataSkipped = dataSkipped;
		SkippedItemsCount = skipItemsCount;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("DataSkippedMs", (int)DataSkipped.TotalMilliseconds);
	}
}
