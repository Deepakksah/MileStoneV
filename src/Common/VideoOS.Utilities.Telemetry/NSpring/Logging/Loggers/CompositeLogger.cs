namespace NSpring.Logging.Loggers;

public sealed class CompositeLogger : Logger
{
	private sealed class EmptyEventFormatter : EventFormatter
	{
		private const string BLANK_STRING = "";

		public override string Format(Event _event)
		{
			return "";
		}

		public override string GetEventListHeader()
		{
			return null;
		}

		public override string GetEventListFooter()
		{
			return null;
		}
	}

	private static EventFormatter compositeLoggerEventFormatter = new EmptyEventFormatter();

	public CompositeLogger()
	{
		base.Level = Level.None;
		base.IsBufferingEnabled = true;
		base.AutoFlushInterval = 15000;
		eventFormatter = compositeLoggerEventFormatter;
	}

	public CompositeLogger(Logger logger1)
		: this(new Logger[1] { logger1 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2)
		: this(new Logger[2] { logger1, logger2 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3)
		: this(new Logger[3] { logger1, logger2, logger3 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4)
		: this(new Logger[4] { logger1, logger2, logger3, logger4 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5)
		: this(new Logger[5] { logger1, logger2, logger3, logger4, logger5 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5, Logger logger6)
		: this(new Logger[6] { logger1, logger2, logger3, logger4, logger5, logger6 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5, Logger logger6, Logger logger7)
		: this(new Logger[7] { logger1, logger2, logger3, logger4, logger5, logger6, logger7 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5, Logger logger6, Logger logger7, Logger logger8)
		: this(new Logger[8] { logger1, logger2, logger3, logger4, logger5, logger6, logger7, logger8 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5, Logger logger6, Logger logger7, Logger logger8, Logger logger9)
		: this(new Logger[9] { logger1, logger2, logger3, logger4, logger5, logger6, logger7, logger8, logger9 })
	{
	}

	public CompositeLogger(Logger logger1, Logger logger2, Logger logger3, Logger logger4, Logger logger5, Logger logger6, Logger logger7, Logger logger8, Logger logger9, Logger logger10)
		: this(new Logger[10] { logger1, logger2, logger3, logger4, logger5, logger6, logger7, logger8, logger9, logger10 })
	{
	}

	public CompositeLogger(Logger[] children)
	{
		if (children == null)
		{
			children = new Logger[0];
		}
		base.Level = Level.None;
		foreach (Logger logger in children)
		{
			if (logger != null)
			{
				AddChild(logger);
				if (logger.Level.Priority < base.Level.Priority)
				{
					base.Level = logger.Level;
				}
			}
		}
		base.IsBufferingEnabled = true;
		base.AutoFlushInterval = 15000;
		eventFormatter = compositeLoggerEventFormatter;
	}

	protected override void Write(Event _event)
	{
	}

	protected override void Write(Event[] _events, int _length)
	{
	}
}
