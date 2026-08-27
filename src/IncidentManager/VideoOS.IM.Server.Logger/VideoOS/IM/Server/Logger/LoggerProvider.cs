using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using VideoOS.Diagnostics.Logging;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Logger;

public sealed class LoggerProvider : ILoggerProvider, IDisposable
{
	private static readonly object _locker = new object();

	private ILocalizedLoggerFactory _auditLoggerFactory;

	[NotNull]
	private string _loggerPath = string.Empty;

	[NotNull]
	private string _logfile = string.Empty;

	[CanBeNull]
	private ILogger _logger;

	[CanBeNull]
	private IAuditLogger _auditLogger;

	[CanBeNull]
	private ILocalizedLogger _auditAndSystemLogger;

	private const string LoggerName = "IncidentManagerWebApi";

	private static readonly Lazy<ILoggerProvider> LazyInstance = new Lazy<ILoggerProvider>(() => new LoggerProvider());

	public static ILoggerProvider Instance => LazyInstance.Value;

	public ILogger Logger
	{
		get
		{
			if (_logger == null)
			{
				lock (_locker)
				{
					if (_logger == null)
					{
						throw new InvalidOperationException("InitLogger should be called first.");
					}
				}
			}
			return _logger;
		}
	}

	public IAuditLogger AuditLogger
	{
		get
		{
			if (_auditLogger == null)
			{
				lock (_locker)
				{
					if (_auditLogger == null)
					{
						throw new InvalidOperationException("InitLogger should be called first.");
					}
				}
			}
			return _auditLogger;
		}
	}

	public ILocalizedLogger AuditAndSystemLogger
	{
		get
		{
			if (_auditAndSystemLogger == null)
			{
				lock (_locker)
				{
					if (_auditAndSystemLogger == null)
					{
						throw new InvalidOperationException("InitLogger should be called first.");
					}
				}
			}
			return _auditAndSystemLogger;
		}
	}

	private LoggerProvider()
	{
	}

	public void InitLogger([NotNull] string loggerPath, TokenCacheDelegatingHandler tokenCacheDelegatingHandler, string configFilePath)
	{
		loggerPath.NullCheck("", "InitLogger", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Logger\\LoggerProvider.cs", 97);
		lock (_locker)
		{
			_logger = null;
			_auditLogger = null;
			_loggerPath = loggerPath;
			LogManager.ModifyLogParameter -= LogManager_ModifyLogParameter;
			LogManager.ModifyLogParameter += LogManager_ModifyLogParameter;
			_logger = Telemetry.GetILogger("IncidentManagerWebApi");
			_auditLogger = new AuditLogger(_logger);
			_auditLoggerFactory = Telemetry.GetILocalizedLoggerFactory("Server", () => new HttpClient((HttpMessageHandler)(object)tokenCacheDelegatingHandler), _logger, configFilePath);
			_auditAndSystemLogger = _auditLoggerFactory.CreateLocalizedLogger("Server");
		}
	}

	public void Dispose()
	{
		_auditLoggerFactory.Dispose();
	}

	private void LogManager_ModifyLogParameter(object sender, LogManager.LogInitializeParameterEventArgs e)
	{
		if (e.LogType == "File" && (e.Key == "Filename" || e.Key == "Archive"))
		{
			string text = (string)e.Value2;
			text = text.Replace("~", "").Replace("/", "\\");
			if (Path.IsPathRooted(text))
			{
				text = text.TrimStart('\\');
			}
			if (e.Key == "Filename")
			{
				_logfile = Path.Combine(_loggerPath, text);
				e.Value = _logfile;
			}
			else
			{
				e.Value = Path.Combine(_loggerPath, text);
			}
		}
	}
}
