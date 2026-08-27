using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public interface ILogConfig
{
	TimeSpan BundleInterval { get; }

	int BundleMaxCount { get; }

	TimeSpan IdpBundleInterval { get; }

	int IdpBundleMaxCount { get; }

	string CacheFile { get; set; }

	Dictionary<LogType, LogDefinition> Logs { get; }
}
