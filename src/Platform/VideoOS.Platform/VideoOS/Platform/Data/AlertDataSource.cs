using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

[Obsolete("Alerts are not supported for C-Code products")]
public class AlertDataSource : DataSource
{
	public AlertDataSource(Item item)
		: base(item)
	{
		new TrackPageViewData(typeof(AlertDataSource), deprecated: true).Send();
		throw new NotSupportedMIPException("Alerts are no longer supported");
	}

	public override List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		return new List<object>();
	}

	public override string Command(object objectData, string command, string[] args)
	{
		return "Not Supported";
	}
}
