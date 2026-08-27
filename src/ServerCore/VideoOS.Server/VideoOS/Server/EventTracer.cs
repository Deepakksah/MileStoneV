using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Globalization;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

[EventSource(Name = "VideoOS-ServerService-Tracing")]
public sealed class EventTracer : EventSource, IEventTrace
{
	private static class EventId
	{
		public const ushort Message = 1;

		public const ushort Method_Start = 2;

		public const ushort Method_Stop = 3;
	}

	private static Lazy<EventTracer> _log = new Lazy<EventTracer>(() => new EventTracer());

	public static EventTracer Log => _log.Value;

	[Event(1, Level = EventLevel.Verbose)]
	[Localizable(false)]
	public void Trace(string message)
	{
		if (IsEnabled())
		{
			WriteEvent(1, message);
		}
	}

	[Localizable(false)]
	public void Trace(string format, params object[] args)
	{
		if (IsEnabled() && format != null && args != null)
		{
			Trace(string.Format(CultureInfo.InvariantCulture, format, args));
		}
	}

	[Event(2, Level = EventLevel.Informational)]
	[Localizable(false)]
	public void TraceMethodStart(string methodName)
	{
		if (IsEnabled())
		{
			WriteEvent(2, methodName);
		}
	}

	[Event(2, Level = EventLevel.Informational)]
	[Localizable(false)]
	public void TraceMethodStart(string methodName, object additionalParameters)
	{
		if (IsEnabled())
		{
			WriteEvent(2, methodName, additionalParameters);
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
