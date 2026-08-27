using System;
using VideoOS.DataCollector.Service.Common;

namespace VideoOS.DataCollector.Service.Sample.Events;

public class SampleRulesEvent : BaseEvent
{
	public DateTime Item { get; }

	public SampleRulesEvent(DateTime item)
	{
		Item = item;
	}
}
