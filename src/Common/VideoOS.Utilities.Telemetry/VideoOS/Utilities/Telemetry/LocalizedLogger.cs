using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public class LocalizedLogger : ILocalizedLogger
{
	private readonly ILogger _baseLogger;

	private readonly CultureInfo _culture;

	public ILogger BaseLogger => _baseLogger;

	public CultureInfo Culture => _culture;

	public LocalizedLogger(ILogger baseLogger, CultureInfo culture)
	{
		_baseLogger = baseLogger ?? throw new ArgumentNullException("baseLogger");
		_culture = culture ?? throw new ArgumentNullException("culture");
	}

	private static void UsingCulture(CultureInfo culture, Action action)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			action();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			Thread.CurrentThread.CurrentUICulture = currentUICulture;
		}
	}

	public void Log(LogLevel logLevel, Func<CultureInfo, LogItem[]> parameterFunc)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (!_baseLogger.IsEnabled(logLevel))
		{
			return;
		}
		UsingCulture(_culture, delegate
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			LogItem[] source = parameterFunc(_culture);
			string text = string.Join(" ", source.Select((LogItem kvp) => "{" + kvp.Field + "}"));
			object[] array = source.Select((LogItem kvp) => kvp.Value).ToArray();
			object[] array2 = array;
			LoggerExtensions.Log(_baseLogger, logLevel, text, array2);
		});
	}
}
