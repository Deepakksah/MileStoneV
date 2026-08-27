using System;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.IM.Server.Logger;

public interface ILoggerProvider : IDisposable
{
	[NotNull]
	ILogger Logger { get; }

	[NotNull]
	IAuditLogger AuditLogger { get; }

	[NotNull]
	ILocalizedLogger AuditAndSystemLogger { get; }

	void InitLogger([NotNull] string loggerPath, TokenCacheDelegatingHandler tokenCacheDelegatingHandler, string configFilePath);
}
