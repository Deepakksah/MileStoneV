using System;
using NSpring.Logging.EventFormatters;

namespace NSpring.Logging.Loggers;

public class ConsoleLogger : Logger
{
	public ConsoleLogger()
	{
		eventFormatter = new PatternEventFormatter("{ts}  {msg}");
	}

	public ConsoleLogger(string _pattern)
	{
		if (_pattern == null)
		{
			throw new ArgumentNullException();
		}
		eventFormatter = new PatternEventFormatter(_pattern);
	}

	protected override void Write(Event _event)
	{
		Console.WriteLine(eventFormatter.Format(_event));
	}

	protected override void Write(Event[] events, int length)
	{
		for (int i = 0; i < length; i++)
		{
			Console.WriteLine(eventFormatter.Format(events[i]));
		}
	}
}
