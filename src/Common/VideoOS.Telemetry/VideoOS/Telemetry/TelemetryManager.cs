using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.Logging;

namespace VideoOS.Telemetry;

public class TelemetryManager : ITelemetryManager
{
	private readonly ILogger _logger;

	private readonly ITelemetryKeyProvider _telemetryKeyProvider;

	private readonly ITelemetryEnabler _telemetryEnabler;

	private TelemetryClient _telemetryClient;

	private Func<ITelemetryProcessor, ITelemetryProcessor>[] _telemetryProcessorFactories;

	private object _threadLock = new object();

	private bool _initalized;

	public TelemetryManager(ILogger logger, ITelemetryKeyProvider telemetryKeyProvider, ITelemetryEnabler telemetryEnabler)
		: this(logger, telemetryKeyProvider, telemetryEnabler, (ITelemetryProcessor next) => (ITelemetryProcessor)(object)new PrivacyTelemetryProcessor(next))
	{
	}

	public TelemetryManager(ILogger logger, ITelemetryKeyProvider telemetryKeyProvider, ITelemetryEnabler telemetryEnabler, params Func<ITelemetryProcessor, ITelemetryProcessor>[] telemetryProcessorFactories)
	{
		_logger = logger;
		_telemetryKeyProvider = telemetryKeyProvider ?? throw new ArgumentNullException("telemetryKeyProvider");
		_telemetryEnabler = telemetryEnabler ?? throw new ArgumentNullException("telemetryEnabler");
		_telemetryProcessorFactories = telemetryProcessorFactories;
	}

	public void InitializeTelemetryClient()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		if (_initalized)
		{
			return;
		}
		lock (_threadLock)
		{
			if (_telemetryClient != null)
			{
				return;
			}
			TelemetryConfiguration val = new TelemetryConfiguration();
			val.ConnectionString = _telemetryKeyProvider.GetKey();
			if (_telemetryProcessorFactories != null)
			{
				Func<ITelemetryProcessor, ITelemetryProcessor>[] telemetryProcessorFactories = _telemetryProcessorFactories;
				foreach (Func<ITelemetryProcessor, ITelemetryProcessor> func in telemetryProcessorFactories)
				{
					val.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Use(func);
				}
			}
			_telemetryClient = new TelemetryClient(val);
			_telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
			TelemetryDebugWriter.IsTracingDisabled = true;
			_initalized = true;
			ILogger logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug(logger, "Telemetry manager initialized with session id: {SessionId}", new object[1] { _telemetryClient.Context.Session.Id });
			}
		}
	}

	public void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null, IDictionary<string, string> properties = null)
	{
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				InitializeTelemetryClient();
				_telemetryClient.TrackAvailability(name, timeStamp, duration, runLocation, success, message, properties, (IDictionary<string, double>)null);
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Track Availability: name:{Name}, timeStamp:{TimeStamp}, duration:{Duration}, runLocation:{RunLocation}, success:{Success}, message:{Message}, properties:{{{Properties}}}", new object[7]
					{
						name,
						timeStamp,
						duration,
						runLocation,
						success,
						message,
						DictionaryToString(properties)
					});
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "TrackAvailability failed", Array.Empty<object>());
			}
		}
	}

	public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success, IDictionary<string, string> properties = null)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			if (!_telemetryEnabler.IsTelemetryEnabled)
			{
				return;
			}
			InitializeTelemetryClient();
			DependencyTelemetry val = new DependencyTelemetry
			{
				Name = dependencyName,
				Type = dependencyTypeName,
				Data = data,
				Timestamp = startTime,
				Duration = duration,
				Success = success
			};
			if (properties != null)
			{
				foreach (KeyValuePair<string, string> property in properties)
				{
					((OperationTelemetry)val).Properties.Add(property);
				}
			}
			_telemetryClient.TrackDependency(val);
			ILogger logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug(logger, "Telemetry Track Dependency: dependencyTypeName:{TypeName}, dependencyName:{Name}, data:{Data}, startTime:{StartTime}, duration:{Duration}, success:{Success}, properties:{{{Properties}}}", new object[7]
				{
					dependencyTypeName,
					dependencyName,
					data,
					startTime,
					duration,
					success,
					DictionaryToString(properties)
				});
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Dependency failed", Array.Empty<object>());
			}
		}
	}

	public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				InitializeTelemetryClient();
				_telemetryClient.TrackEvent(eventName, properties, metrics);
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Track Event: eventName:{Name}, properties:{{{Properties}}}, metrics:{{{Metrics}}}", new object[3]
					{
						eventName,
						DictionaryToString(properties),
						DictionaryToString(metrics)
					});
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Event failed", Array.Empty<object>());
			}
		}
	}

	public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				InitializeTelemetryClient();
				ExceptionDetailsInfo val = new ExceptionDetailsInfo(exception.GetHashCode(), 0, exception.GetType().FullName, "Exception message", false, exception.StackTrace, (IEnumerable<StackFrame>)new List<StackFrame>());
				ExceptionTelemetry val2 = new ExceptionTelemetry((IEnumerable<ExceptionDetailsInfo>)new List<ExceptionDetailsInfo> { val }, (SeverityLevel?)(SeverityLevel)3, string.Empty, properties ?? new Dictionary<string, string>(), metrics ?? new Dictionary<string, double>());
				_telemetryClient.TrackException(val2);
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Track Exception: exceptionDetailsInfo:{ExceptionDetails}, properties:{{{Properties}}}, metrics:{{{Metrics}}}", new object[3]
					{
						val,
						DictionaryToString(properties),
						DictionaryToString(metrics)
					});
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Exception failed", Array.Empty<object>());
			}
		}
	}

	public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
	{
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				InitializeTelemetryClient();
				_telemetryClient.TrackMetric(name, value, properties);
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Track Metric: name:{Name}, value:{Value}, properties:{{{Properties}}}", new object[3]
					{
						name,
						value,
						DictionaryToString(properties)
					});
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Metric failed", Array.Empty<object>());
			}
		}
	}

	public void TrackPageView(string name, string id = null, TimeSpan? duration = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			if (!_telemetryEnabler.IsTelemetryEnabled)
			{
				return;
			}
			InitializeTelemetryClient();
			PageViewTelemetry val = new PageViewTelemetry(name);
			if (!string.IsNullOrEmpty(id))
			{
				val.Id = id;
			}
			if (duration.HasValue)
			{
				val.Duration = duration.Value;
			}
			if (properties != null)
			{
				foreach (KeyValuePair<string, string> property in properties)
				{
					val.Properties?.Add(property.Key, property.Value);
				}
			}
			if (metrics != null)
			{
				foreach (KeyValuePair<string, double> metric in metrics)
				{
					val.Metrics?.Add(metric.Key, metric.Value);
				}
			}
			_telemetryClient.TrackPageView(val);
			ILogger logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug(logger, "Telemetry Track Page View: name:{Name}, id: {Id}, duration: {Duration}, properties:{{{Properties}}}, metrics:{{{Metrics}}}", new object[5]
				{
					name,
					id,
					duration,
					DictionaryToString(properties),
					DictionaryToString(metrics)
				});
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Page View failed", Array.Empty<object>());
			}
		}
	}

	public void TrackPageView(string name, string id, TimeSpan duration)
	{
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				TrackPageView(name, id, duration, null, null);
			}
		}
		catch (Exception ex)
		{
			ILogger logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogError(logger, ex, "Track Page View failed", Array.Empty<object>());
			}
		}
	}

	public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success, IDictionary<string, string> properties = null)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			if (!_telemetryEnabler.IsTelemetryEnabled)
			{
				return;
			}
			InitializeTelemetryClient();
			RequestTelemetry val = new RequestTelemetry
			{
				Name = name,
				Timestamp = startTime,
				Duration = duration,
				ResponseCode = responseCode,
				Success = success
			};
			if (properties != null)
			{
				foreach (KeyValuePair<string, string> property in properties)
				{
					((OperationTelemetry)val).Properties.Add(property);
				}
			}
			_telemetryClient.TrackRequest(val);
			ILogger logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug(logger, "Telemetry Track Request: name:{Name}, startTime:{StartTime}, duration:{Duration}, responseCode:{ResponseCode}, success:{Success}, properties: {Properties}", new object[6]
				{
					name,
					startTime,
					duration,
					responseCode,
					success,
					DictionaryToString(properties)
				});
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Request failed", Array.Empty<object>());
			}
		}
	}

	public void TrackTrace(string message, IDictionary<string, string> properties)
	{
		try
		{
			if (_telemetryEnabler.IsTelemetryEnabled)
			{
				InitializeTelemetryClient();
				_telemetryClient.TrackTrace(message, properties);
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Track Trace: message:{Message}, properties:{{{Properties}}}", new object[2]
					{
						message,
						DictionaryToString(properties)
					});
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Track Trace failed", Array.Empty<object>());
			}
		}
	}

	public void Flush()
	{
		try
		{
			if (_telemetryClient != null)
			{
				_telemetryClient.Flush();
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogDebug(logger, "Telemetry Flush", Array.Empty<object>());
				}
			}
		}
		catch (Exception ex)
		{
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, ex, "Flush failed", Array.Empty<object>());
			}
		}
	}

	private static string DictionaryToString(IDictionary<string, string> dict)
	{
		if (dict == null)
		{
			return string.Empty;
		}
		return string.Join(",", dict);
	}

	private static string DictionaryToString(IDictionary<string, double> dict)
	{
		if (dict == null)
		{
			return string.Empty;
		}
		return string.Join(",", dict);
	}
}
