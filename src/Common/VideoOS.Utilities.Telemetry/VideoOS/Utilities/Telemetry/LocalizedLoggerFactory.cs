using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public class LocalizedLoggerFactory : ILocalizedLoggerFactory, IDisposable
{
	private CultureInfo _cultureInfo { get; set; }

	private ILoggerFactory _loggerFactory { get; set; }

	public LocalizedLoggerFactory(ILoggerFactory loggerFactory, CultureInfo cultureInfo)
	{
		_loggerFactory = loggerFactory;
		_cultureInfo = cultureInfo;
	}

	public ILocalizedLogger CreateLocalizedLogger(string name)
	{
		return new LocalizedLogger(_loggerFactory.CreateLogger(name), _cultureInfo);
	}

	public void Dispose()
	{
		((IDisposable)_loggerFactory).Dispose();
		GC.SuppressFinalize(this);
	}
}
