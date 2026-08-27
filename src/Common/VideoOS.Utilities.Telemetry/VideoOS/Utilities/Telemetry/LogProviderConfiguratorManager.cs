using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public class LogProviderConfiguratorManager : ILogProviderConfiguratorManager
{
	private readonly Dictionary<string, ILogProviderConfigurator> _providers = new Dictionary<string, ILogProviderConfigurator>(StringComparer.OrdinalIgnoreCase);

	public void AddProviderConfigurator(ILogProviderConfigurator logProviderConfigurator)
	{
		if (logProviderConfigurator == null)
		{
			throw new ArgumentNullException("logProviderConfigurator", "Log provider configurator cannot be null.");
		}
		if (_providers.ContainsKey(logProviderConfigurator.Name))
		{
			throw new InvalidOperationException("A log provider configurator with the name '" + logProviderConfigurator.Name + "' is already registered.");
		}
		_providers[logProviderConfigurator.Name] = logProviderConfigurator;
	}

	public void ConfigureProvider(ILoggingBuilder builder, string logProviderName, ILogConfigurationProvider logConfigurationProvider, ILogger serviceLogger)
	{
		if (builder == null)
		{
			throw new ArgumentNullException("builder", "Logging builder cannot be null.");
		}
		if (logConfigurationProvider == null)
		{
			throw new ArgumentNullException("logConfigurationProvider", "Log configuration provider cannot be null.");
		}
		if (string.IsNullOrWhiteSpace(logProviderName))
		{
			throw new ArgumentException("Provider name cannot be null or empty.", "logProviderName");
		}
		if (_providers.Count == 0)
		{
			throw new InvalidOperationException("No log providers have been registered.");
		}
		string logProviderRegisteredName = GetLogProviderRegisteredName(logProviderName);
		if (string.IsNullOrWhiteSpace(logProviderRegisteredName))
		{
			throw new ArgumentException("Log logProvider registered name substring not found.", "logProviderName");
		}
		if (!_providers.TryGetValue(logProviderRegisteredName, out var value))
		{
			throw new InvalidOperationException("No logProvider registered for '" + logProviderRegisteredName + "'.");
		}
		value.Configure(builder, logProviderName, logConfigurationProvider, serviceLogger);
	}

	private static string GetLogProviderRegisteredName(string logProviderName)
	{
		return logProviderName.Split('-')[0] ?? string.Empty;
	}
}
