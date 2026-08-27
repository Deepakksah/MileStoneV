using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;

namespace VideoOS.Management.Server;

public sealed class WebRequestLogger : PeriodicTimerServiceBase, IGeminiServerService
{
	public class RequestDetailsDictionary
	{
		private int _queuedRecords;

		private ConcurrentDictionary<RequestDetails, RequestDetails> _queuedRequests;

		public int QueuedRecords => _queuedRecords;

		public ICollection<RequestDetails> Values => _queuedRequests.Values;

		public RequestDetailsDictionary(int maxRecords, RequestDetailsComparer comparer)
		{
			_queuedRequests = new ConcurrentDictionary<RequestDetails, RequestDetails>(Environment.ProcessorCount * 3, maxRecords, comparer);
			_queuedRecords = 0;
		}

		public int AddRequest(RequestDetails requestDetails)
		{
			if (_queuedRequests.AddOrUpdate(requestDetails, requestDetails, (RequestDetails k, RequestDetails v) => v.CompressRequestDetails(requestDetails)) == requestDetails)
			{
				return Interlocked.Increment(ref _queuedRecords);
			}
			return 0;
		}
	}

	private string _area = "RequestLogger";

	private string _layer = "WebRequestLogger";

	private int _maxQueuedRecords;

	private int _maxCompressedThreshold;

	private GeminiWebRequestLoggingLevel _loggingLevel;

	private const string LoggingOptionsRegKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server";

	private RequestDetailsDictionary _queuedRequests;

	private RequestDetailsComparer _requestDetailComparer;

	protected override TimeSpan InitialTimeInterval => TimeSpan.FromSeconds(10.0);

	protected override TimeSpan PeriodicTimeInterval => TimeSpan.FromSeconds(30.0);

	private void LoadSettings()
	{
		if (!Enum.TryParse<GeminiWebRequestLoggingLevel>((string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server", "WebRequestLoggingOptions", "None"), out _loggingLevel))
		{
			_loggingLevel = GeminiWebRequestLoggingLevel.None;
		}
		_maxCompressedThreshold = 30000000;
		_maxQueuedRecords = 100;
		if (_queuedRequests == null)
		{
			_requestDetailComparer = new RequestDetailsComparer(_maxCompressedThreshold, _loggingLevel);
			_queuedRequests = new RequestDetailsDictionary(_maxQueuedRecords, _requestDetailComparer);
		}
	}

	protected override void OnTimer(object state)
	{
		ProcessRecords();
	}

	public void LogRequest(IGeminiRequestContext requestContext)
	{
		try
		{
			requestContext.RequestTracer.TraceEnter(0, _area, _layer, "LogRequest");
			int num = 0;
			if (_loggingLevel != GeminiWebRequestLoggingLevel.None)
			{
				RequestDetails requestDetails = new RequestDetails(requestContext, _loggingLevel);
				if (string.IsNullOrEmpty(requestDetails.WebMethodName))
				{
					return;
				}
				num = _queuedRequests.AddRequest(requestDetails);
			}
			if (num >= _maxQueuedRecords)
			{
				requestContext.RequestTracer.Trace(0, TraceLevel.Warning, _area, _layer, "Automatic processing of web-requests cannot keep up");
				try
				{
					ProcessRecords(requestContext);
					return;
				}
				catch (Exception ex)
				{
					requestContext.RequestTracer.TraceException(0, _area, _layer, ex);
					throw;
				}
			}
		}
		catch (Exception ex2)
		{
			requestContext.RequestTracer.TraceException(0, _area, _layer, ex2);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, _area, _layer, "LogRequest");
		}
	}

	private void ProcessRecords()
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		ProcessRecords(requestContext);
	}

	private void ProcessRecords(IGeminiRequestContext requestContext)
	{
		try
		{
			requestContext.RequestTracer.TraceEnter(0, _area, _layer, "ProcessRecords");
			if (_queuedRequests.QueuedRecords > 0)
			{
				ICollection<RequestDetails> values = _queuedRequests.Values;
				_queuedRequests = new RequestDetailsDictionary(_maxQueuedRecords, _requestDetailComparer);
				using RequestLoggerComponent requestLoggerComponent = requestContext.CreateComponent<RequestLoggerComponent>();
				requestLoggerComponent.SubmitRecords(values);
				return;
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, _area, _layer, ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, _area, _layer, "ProcessRecords");
		}
	}

	protected override void StartService(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, _area, _layer, "StartService");
		try
		{
			LoadSettings();
			base.StartService(requestContext);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, _area, _layer, ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, _area, _layer, "StartService");
		}
	}

	protected override void StopService(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, _area, _layer, "StopService");
		try
		{
			ProcessRecords(requestContext);
			base.StopService(requestContext);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, _area, _layer, ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, _area, _layer, "StopService");
		}
	}
}
