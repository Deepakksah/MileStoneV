using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;

namespace VideoOS.Common.Integration;

[EventSource(Name = "VideoOS-Licensing-Tracing")]
public sealed class LicensingEventTracer : EventSource
{
	private const ushort TracingMethodStart = 2;

	private const ushort TracingMethodStop = 3;

	private static Lazy<LicensingEventTracer> _logger = new Lazy<LicensingEventTracer>(() => new LicensingEventTracer());

	public static LicensingEventTracer Logger => _logger.Value;

	[Event(2, Level = EventLevel.Informational)]
	[Localizable(false)]
	public void TraceMethodStart(string methodName)
	{
		if (IsEnabled())
		{
			WriteEvent(2, methodName);
		}
	}

	[Event(3, Level = EventLevel.Informational)]
	[Localizable(false)]
	public void TraceMethodStop(string methodName)
	{
		if (IsEnabled())
		{
			WriteEvent(3, methodName);
		}
	}
}
