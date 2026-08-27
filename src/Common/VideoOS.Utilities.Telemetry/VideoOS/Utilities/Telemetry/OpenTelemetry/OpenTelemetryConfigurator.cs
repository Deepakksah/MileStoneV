using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;

namespace VideoOS.Utilities.Telemetry.OpenTelemetry;

public class OpenTelemetryConfigurator : ILogProviderConfigurator
{
	private readonly Func<HttpClient> _httpClientFactory;

	public string Name => "OpenTelemetry";

	public OpenTelemetryConfigurator(Func<HttpClient> httpClientFactory)
	{
		if (httpClientFactory == null)
		{
			throw new ArgumentNullException("httpClientFactory", "The HTTP client factory cannot be null.");
		}
		_httpClientFactory = httpClientFactory;
	}

	public void Configure(ILoggingBuilder builder, string logProviderName, ILogConfigurationProvider logConfigurationProvider, ILogger serviceLogger)
	{
		if (builder == null)
		{
			throw new ArgumentNullException("builder", "Logging builder cannot be null.");
		}
		if (string.IsNullOrWhiteSpace(logProviderName))
		{
			throw new ArgumentNullException("logProviderName", "Log provider name cannot be null or empty.");
		}
		if (logConfigurationProvider == null)
		{
			throw new ArgumentNullException("logConfigurationProvider", "Log configuration provider cannot be null.");
		}
		LoggerExtensions.LogInformation(serviceLogger, "Configuring OpenTelemetry logging for provider '{LogProviderName}'", new object[1] { logProviderName });
		OpenTelemetryProviderConfiguration logProviderConfiguration = logConfigurationProvider.GetLogProviderConfiguration<OpenTelemetryProviderConfiguration>(logProviderName);
		AddOpenTelemetry(builder, logProviderConfiguration, _httpClientFactory, serviceLogger);
	}

	public static void AddOpenTelemetry(ILoggingBuilder loggingBuilder, OpenTelemetryProviderConfiguration openTelemetryProviderConfiguration, Func<HttpClient> httpClientFactory, ILogger serviceLogger)
	{
		OpenTelemetryLoggingExtensions.AddOpenTelemetry(loggingBuilder, (Action<OpenTelemetryLoggerOptions>)delegate(OpenTelemetryLoggerOptions options)
		{
			options.IncludeScopes = true;
			AddOpenTelemetryExporters(options, openTelemetryProviderConfiguration.Exporters, httpClientFactory, serviceLogger);
		});
	}

	public static void AddOpenTelemetryExporters(OpenTelemetryLoggerOptions options, Dictionary<string, Exporter> exporters, Func<HttpClient> httpClientFactory, ILogger serviceLogger)
	{
		foreach (KeyValuePair<string, Exporter> exporter in exporters)
		{
			LoggerExtensions.LogInformation(serviceLogger, "Configuring OpenTelemetry exporter '{ExporterKey}' with:\n endpoint '{ExporterEndpoint}'\n protocol 'HttpProtobuf'", new object[2]
			{
				exporter.Key,
				exporter.Value.Endpoint
			});
			OtlpLogExporterHelperExtensions.AddOtlpExporter(options, exporter.Key, (Action<OtlpExporterOptions, LogRecordExportProcessorOptions>)delegate(OtlpExporterOptions exporterOptions, LogRecordExportProcessorOptions processorOptions)
			{
				processorOptions.ExportProcessorType = (ExportProcessorType)0;
				exporterOptions.Endpoint = new Uri(exporter.Value.Endpoint);
				exporterOptions.Protocol = (OtlpExportProtocol)1;
				exporterOptions.HttpClientFactory = httpClientFactory;
			});
		}
	}
}
