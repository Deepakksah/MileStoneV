using System;

namespace VideoOS.Platform.Util.Queue;

public class MultiLevelResourceQueueInput<TOutput> : ICancelable
{
	public object Payload { get; }

	public Action<TOutput> OutputAction { get; }

	public bool Canceled { get; set; }

	public MultiLevelResourceQueueInput(object payload, Action<TOutput> outputAction)
	{
		Payload = payload;
		OutputAction = outputAction;
	}
}
