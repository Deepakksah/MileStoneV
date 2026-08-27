using System;
using VideoOS.DataCollector.Service.Common;

namespace VideoOS.DataCollector.Service.Sample.Events;

public class CollectDataEvent : BaseEvent
{
	public DateTime Item { get; }

	public CollectDataEvent(DateTime item)
	{
		Item = item;
	}
}
