using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.Server.ConfigurationApi.Security;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiRequestContext : RequestContextBase, IGeminiRequestContext, IDisposable
{
	private class LogRequest : ILogRequest
	{
		private IList<MethodTime> _sqlCalls;

		public int LogicalReads { get; private set; }

		public int PhysicalReads { get; private set; }

		public int CpuTime { get; private set; }

		public int ElapsedTime { get; private set; }

		public IList<MethodTime> SqlCalls => _sqlCalls;

		public IList<MethodTime> RecursiveSqlCalls
		{
			get
			{
				List<MethodTime> list = new List<MethodTime>();
				if (_sqlCalls != null)
				{
					foreach (MethodTime sqlCall in _sqlCalls)
					{
						list.Add(new MethodTime(sqlCall.Name, sqlCall.Time, sqlCall.LogicalReads, sqlCall.PhysicalReads, sqlCall.CPUTime, sqlCall.ElapsedTime));
					}
				}
				return list;
			}
		}

		public LogRequest()
		{
		}

		public void LogSqlCall(MethodTime methodTime)
		{
			Validation.CheckObjectReference(methodTime, "methodTime");
			if (_sqlCalls == null)
			{
				_sqlCalls = new List<MethodTime>();
			}
			_sqlCalls.Add(methodTime);
			LogicalReads += methodTime.LogicalReads;
			PhysicalReads += methodTime.PhysicalReads;
			CpuTime += methodTime.CPUTime;
			ElapsedTime += methodTime.ElapsedTime;
		}
	}

	public class TimeRequest : ITimeRequest, IDisposable
	{
		private IGeminiRequestContext _requestContext;

		private DateTime _startTime;

		private DateTime _endTime;

		private long _requestStartTimestamp;

		private long _requestEndTimestamp;

		private int _timingsIndex;

		private int _fastMethodCount;

		private int _truncatedMethodsCount;

		private KeyValuePair<TraceLocation, int>[] _timings;

		private static ConcurrentStack<KeyValuePair<TraceLocation, int>[]> s_methodTrackers = new ConcurrentStack<KeyValuePair<TraceLocation, int>[]>();

		private static int s_methodTrackerCount = 0;

		private static int maxMethodTrackers = 500;

		private static int s_fastMethodMs = 5;

		public DateTime StartTime
		{
			get
			{
				return _startTime;
			}
			set
			{
				if (_startTime == DateTime.MinValue || value < _startTime)
				{
					_startTime = value;
				}
			}
		}

		public DateTime EndTime
		{
			get
			{
				return _endTime;
			}
			private set
			{
				_endTime = value;
			}
		}

		public long ExecutionTime
		{
			get
			{
				if (_requestEndTimestamp == 0L)
				{
					return ElapsedMicrosecond(_requestStartTimestamp, Stopwatch.GetTimestamp());
				}
				return ElapsedMicrosecond(_requestStartTimestamp, _requestEndTimestamp);
			}
		}

		public TimeSpan ExecutionSpan
		{
			get
			{
				if (_requestEndTimestamp == 0L)
				{
					return new TimeSpan(StopwatchToTimeSpan(_requestStartTimestamp, Stopwatch.GetTimestamp()));
				}
				return new TimeSpan(StopwatchToTimeSpan(_requestStartTimestamp, _requestEndTimestamp));
			}
		}

		public long StartTimestamp
		{
			get
			{
				return _requestStartTimestamp;
			}
			set
			{
				if (_requestStartTimestamp == 0L || (value > 0 && value < _requestStartTimestamp))
				{
					_requestStartTimestamp = value;
				}
			}
		}

		public string TraceTimingsAsString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < _timings.Length; i++)
				{
					KeyValuePair<TraceLocation, int> keyValuePair = _timings[i];
					if (!keyValuePair.Key.IsInitialized)
					{
						break;
					}
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}:{1}", keyValuePair.Key, keyValuePair.Value);
				}
				return stringBuilder.ToString();
			}
		}

		public int FastMethodsCount => _fastMethodCount;

		public int TruncatedMethodsCount => _truncatedMethodsCount;

		public string GetTraceTimings(long executionTimeThresholdMs)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Total Time:{0}ms (Threshold:{1}ms) (Fast:{2}) (Truncated:{3})\r\n", ExecutionSpan.TotalMilliseconds, executionTimeThresholdMs, FastMethodsCount, TruncatedMethodsCount);
			if (_timings != null)
			{
				for (int i = 0; i < _timings.Length; i++)
				{
					KeyValuePair<TraceLocation, int> keyValuePair = _timings[i];
					if (keyValuePair.Key.IsInitialized)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}:{1}", keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			return stringBuilder.ToString();
		}

		public TimeRequest(IGeminiRequestContext requestContext)
		{
			_requestContext = requestContext;
			_timings = GetTimingsArray(_requestContext);
		}

		private static KeyValuePair<TraceLocation, int>[] GetTimingsArray(IGeminiRequestContext requestContext)
		{
			if (s_methodTrackers.TryPop(out var result))
			{
				Interlocked.Decrement(ref s_methodTrackerCount);
				return result;
			}
			return new KeyValuePair<TraceLocation, int>[255];
		}

		private static void PutTimingsArray(IGeminiRequestContext requestContext, KeyValuePair<TraceLocation, int>[] timingsArray)
		{
			if (s_methodTrackerCount < maxMethodTrackers)
			{
				Interlocked.Increment(ref s_methodTrackerCount);
				s_methodTrackers.Push(timingsArray);
			}
		}

		public void RecordTraceEnterTiming(string area, string layer, string methodName)
		{
			if (IsSameThreadContext() && _timingsIndex < _timings.Length)
			{
				_timings[_timingsIndex] = new KeyValuePair<TraceLocation, int>(new TraceLocation(area, layer, methodName), (int)ExecutionSpan.TotalMilliseconds);
				_timingsIndex++;
			}
		}

		public void RecordTraceLeaveTiming(string area, string layer, string methodName)
		{
			if (!IsSameThreadContext() || _timingsIndex <= 0 || _timings == null)
			{
				return;
			}
			if (_timingsIndex < _timings.Length)
			{
				TraceLocation traceLocation = new TraceLocation(area, layer, methodName);
				int num = (int)ExecutionSpan.TotalMilliseconds;
				if (num - _timings[_timingsIndex - 1].Value < s_fastMethodMs && _timings[_timingsIndex - 1].Key.Equals(traceLocation))
				{
					_fastMethodCount++;
					_timings[_timingsIndex - 1] = default(KeyValuePair<TraceLocation, int>);
					_timingsIndex--;
				}
				else
				{
					_timings[_timingsIndex] = new KeyValuePair<TraceLocation, int>(traceLocation, -num);
					_timingsIndex++;
				}
			}
			else
			{
				_truncatedMethodsCount++;
			}
		}

		public void BeginRequest()
		{
			if (StartTime == DateTime.MinValue)
			{
				StartTime = DateTime.UtcNow;
			}
			if (StartTimestamp == 0L)
			{
				StartTimestamp = Stopwatch.GetTimestamp();
			}
		}

		public void EndRequest()
		{
			if (EndTime == DateTime.MinValue)
			{
				_requestEndTimestamp = Stopwatch.GetTimestamp();
				EndTime = DateTime.UtcNow;
			}
		}

		private static long StopwatchToTimeSpan(long start, long stop)
		{
			if (stop < start)
			{
				return 0L;
			}
			double num = ((double)stop - (double)start) / (double)Stopwatch.Frequency;
			return (long)(10000000.0 * num);
		}

		private static long ElapsedMicrosecond(long start, long stop)
		{
			if (stop < start)
			{
				return 0L;
			}
			double num = ((double)stop - (double)start) / (double)Stopwatch.Frequency;
			return (long)(1000000.0 * num);
		}

		private bool IsSameThreadContext()
		{
			if (_requestContext.MethodExecutionThreadId != -1 && _requestContext.MethodExecutionThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}

	private class TraceRequest : ITraceRequest
	{
		[EventSource(Name = "VideoOS-ManagementServer-Tracing")]
		private sealed class NestedEventSource : EventSource
		{
			[Event(1, Level = EventLevel.Verbose)]
			public void Message(int tracepoint, long contextId, string area, string layer, string message)
			{
				if (IsEnabled())
				{
					WriteEvent(1, tracepoint, contextId, area, layer, message);
				}
			}

			[Event(2, Level = EventLevel.Verbose)]
			public void WebRequestStart(int tracepoint, long contextId, string area, string layer, string message)
			{
				if (IsEnabled())
				{
					WriteEvent(2, tracepoint, contextId, area, layer, message);
				}
			}

			[Event(3, Level = EventLevel.Verbose)]
			public void WebRequestStop(int tracepoint, long contextId, string area, string layer, string message)
			{
				if (IsEnabled())
				{
					WriteEvent(3, tracepoint, contextId, area, layer, message);
				}
			}
		}

		private GeminiRequestContext _requestContext;

		private static NestedEventSource innerLog = new NestedEventSource();

		public static bool IsEnabled => innerLog.IsEnabled();

		public TraceRequest(GeminiRequestContext requestContext)
		{
			_requestContext = requestContext;
		}

		public bool IsTracing(int tracepoint)
		{
			return true;
		}

		public virtual void Trace(int tracepoint, TraceLevel level, string area, string layer, string format, params object[] args)
		{
			if (IsEnabled && format != null && args != null)
			{
				innerLog.Message(tracepoint, _requestContext.ContextId, area, layer, string.Format(CultureInfo.InvariantCulture, format, args));
			}
		}

		private void TraceEnter(Action<int, long, string, string, string> traceCommand, int tracepoint, long contextId, string area, string layer, string format, string methodName)
		{
			if (SqlStatisticsContext.CollectingStatistics)
			{
				SqlStatisticsContext.Enter(methodName);
			}
			_requestContext._requestTimer.RecordTraceEnterTiming(area, layer, methodName);
			if (IsEnabled)
			{
				traceCommand(tracepoint, contextId, area, layer, string.Format(CultureInfo.InvariantCulture, format, methodName));
			}
		}

		public virtual void TraceWebRequestStart(int tracepoint, string area, string layer, string methodName)
		{
			TraceEnter(delegate(int a, long b, string c, string d, string e)
			{
				innerLog.WebRequestStart(a, b, c, d, e);
			}, tracepoint, _requestContext.ContextId, area, layer, Resources.WebRequestStart, methodName);
		}

		public virtual void TraceEnter(int tracepoint, string area, string layer, string methodName)
		{
			TraceEnter(delegate(int a, long b, string c, string d, string e)
			{
				innerLog.Message(a, b, c, d, e);
			}, tracepoint, _requestContext.ContextId, area, layer, Resources.Entering, methodName);
		}

		private void TraceLeave(Action<int, long, string, string, string> traceCommand, int tracepoint, long contextId, string area, string layer, string format, string methodName)
		{
			if (SqlStatisticsContext.CollectingStatistics)
			{
				SqlStatisticsContext.Leave(methodName);
			}
			_requestContext._requestTimer.RecordTraceLeaveTiming(area, layer, methodName);
			if (IsEnabled)
			{
				traceCommand(tracepoint, contextId, area, layer, string.Format(CultureInfo.InvariantCulture, format, methodName));
			}
		}

		public virtual void TraceWebRequestStop(int tracepoint, string area, string layer, string methodName)
		{
			TraceLeave(delegate(int a, long b, string c, string d, string e)
			{
				innerLog.WebRequestStop(a, b, c, d, e);
			}, tracepoint, _requestContext.ContextId, area, layer, Resources.WebRequestStop, methodName);
		}

		public virtual void TraceLeave(int tracepoint, string area, string layer, string methodName)
		{
			TraceLeave(delegate(int a, long b, string c, string d, string e)
			{
				innerLog.Message(a, b, c, d, e);
			}, tracepoint, _requestContext.ContextId, area, layer, Resources.Leaving, methodName);
		}

		public virtual void TraceException(int tracepoint, string area, string layer, Exception ex)
		{
		}

		public virtual void TraceException(int tracepoint, TraceLevel level, string area, string layer, Exception ex)
		{
		}

		public virtual void TraceSql(int tracepoint, string dataSource, string database, string operation, short retries, bool success, int executionTime)
		{
		}
	}

	private Dictionary<Type, IGeminiServerService> _requestedServices;

	private object _requestedServicesLock = new object();

	private static readonly string _area = "RequestManagement";

	private static readonly string _layer = "Kernel";

	private GeminiApplicationHost _applicationHost;

	private long executionTimeThreshold = 10000L;

	private static long s_contextIdCounter = 0L;

	private long _contextId;

	private readonly TimeRequest _requestTimer;

	private TraceRequest _requestTracer;

	private LogRequest _requestLogger;

	private MethodInformation _methodInfo;

	private Thread _methodExecutionThread;

	private RequestContextType _requestContextType;

	private IdentityDescriptor _userContext;

	private string _authenticationType;

	private bool _isBasicUser;

	private bool _isExternalUser;

	private DateTime _userUpdatedAtSec = DateTime.MinValue;

	private string _authenticatedUserSid;

	private string _authenticatedUserName;

	private string _authenticatedUserDomain;

	private string _remoteIPAddress;

	private string _remotePort;

	private string _localIPAddress;

	private Dictionary<string, object> _items;

	private bool _isDisposed;

	private ISecurityObjectMapCache _securityObjectMapCache = new SecurityObjectMapCache();

	public GeminiApplicationHost ApplicationHost => _applicationHost;

	public virtual long ContextId => _contextId;

	public bool IsSystemContext => _requestContextType == RequestContextType.SystemContext;

	public virtual IdentityDescriptor UserContext
	{
		get
		{
			return new IdentityDescriptor(AuthenticatedUserSid);
		}
		public set
		{
			_userContext = value;
		}
	}

	public MethodInformation Method => _methodInfo;

	public ITimeRequest RequestTimer => _requestTimer;

	public ITraceRequest RequestTracer
	{
		get
		{
			if (_requestTracer == null)
			{
				_requestTracer = new TraceRequest(this);
			}
			return _requestTracer;
		}
	}

	public ILogRequest RequestLogger
	{
		get
		{
			if (_requestLogger == null)
			{
				_requestLogger = new LogRequest();
			}
			return _requestLogger;
		}
	}

	public long CpuCycles { get; private set; }

	public string AuthenticationType
	{
		get
		{
			return _authenticationType;
		}
		set
		{
			_authenticationType = value;
			_isExternalUser = StringComparer.Ordinal.Equals("external", _authenticationType) || StringComparer.Ordinal.Equals("urn:ietf:params:oauth:grant-type:token-exchange", _authenticationType);
			_isBasicUser = StringComparer.Ordinal.Equals("pwd", _authenticationType);
		}
	}

	public string AuthenticatedUserSid
	{
		get
		{
			return _authenticatedUserSid;
		}
		set
		{
			_authenticatedUserSid = value;
		}
	}

	public string AuthenticatedUserName
	{
		get
		{
			return _authenticatedUserName;
		}
		set
		{
			_authenticatedUserName = value;
		}
	}

	public string AuthenticatedUserDomain
	{
		get
		{
			return _authenticatedUserDomain;
		}
		set
		{
			_authenticatedUserDomain = value;
		}
	}

	public bool IsBasicUser => _isBasicUser;

	public bool IsExternalVmsUser => _isExternalUser;

	public DateTime UserUpdatedAt => _userUpdatedAtSec;

	public string UserDomainAccountName => string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", AuthenticatedUserDomain, AuthenticatedUserName);

	public string RemoteIPAddress
	{
		get
		{
			return _remoteIPAddress;
		}
		set
		{
			_remoteIPAddress = value;
		}
	}

	public string RemotePort
	{
		get
		{
			return _remotePort;
		}
		set
		{
			_remotePort = value;
		}
	}

	public string LocalIPAddress
	{
		get
		{
			return _localIPAddress;
		}
		set
		{
			_localIPAddress = value;
		}
	}

	public Dictionary<string, object> Items
	{
		get
		{
			if (_items == null)
			{
				_items = new Dictionary<string, object>();
			}
			return _items;
		}
	}

	public int MethodExecutionThreadId
	{
		get
		{
			if (_methodExecutionThread == null)
			{
				return -1;
			}
			return _methodExecutionThread.ManagedThreadId;
		}
	}

	IDeviceTypeLookup IGeminiRequestContext.DeviceTypeLookup => DeviceTypeLookup.Instance;

	ISecurityObjectMapCache IGeminiRequestContext.SecurityObjectMapCache => _securityObjectMapCache;

	IFeatureCacheManager IGeminiRequestContext.FeatureCacheManager => GeminiApplicationCore.FeatureCacheManager;

	protected void SetAuthenticationPropertiesUsingAccessToken()
	{
		SetAuthenticationPropertiesUsingAccessToken(null);
	}

	protected void SetAuthenticationPropertiesUsingAccessToken(HttpRequestMessage httpRequest)
	{
		object value = null;
		ServiceSecurityContext.Current?.AuthorizationContext?.Properties?.TryGetValue("ClaimsPrincipal", out value);
		ClaimsPrincipal claimsPrincipal = value as ClaimsPrincipal;
		if (claimsPrincipal == null && HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
		{
			claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
		}
		if (claimsPrincipal == null && httpRequest != null)
		{
			HttpContextWrapper httpContextWrapper = httpRequest.Properties["MS_HttpContext"] as HttpContextWrapper;
			if (httpContextWrapper.User.Identity is ClaimsIdentity { IsAuthenticated: not false })
			{
				claimsPrincipal = httpContextWrapper.User as ClaimsPrincipal;
			}
		}
		if (claimsPrincipal != null)
		{
			if (claimsPrincipal is SystemClaimsPrinciple)
			{
				SetAuthenticationPropertiesForSystemContext(claimsPrincipal);
			}
			else
			{
				SetAuthenticationPropertiesForUserContext(claimsPrincipal);
			}
		}
	}

	private void SetAuthenticationPropertiesForUserContext(ClaimsPrincipal principal)
	{
		AuthenticatedUserSid = principal.Claims.SingleOrDefault((System.Security.Claims.Claim c) => c.Type == "sub")?.Value;
		string[] array = principal.Claims.SingleOrDefault((System.Security.Claims.Claim c) => c.Type == "name")?.Value.Split('\\');
		if (array != null && array.Length > 1)
		{
			AuthenticatedUserDomain = array[0];
			AuthenticatedUserName = array[1];
		}
		else
		{
			AuthenticatedUserName = ((array != null) ? array[0] : null);
		}
		AuthenticationType = principal.Claims.SingleOrDefault((System.Security.Claims.Claim c) => c.Type == "amr")?.Value;
		if (IsBasicUser || IsExternalVmsUser)
		{
			AuthenticatedUserDomain = "[BASIC]";
		}
		if (IsExternalVmsUser)
		{
			SetUserUpdatedAt(principal.Claims.SingleOrDefault((System.Security.Claims.Claim c) => c.Type == "updated_at")?.Value);
		}
	}

	private void SetAuthenticationPropertiesForSystemContext(ClaimsPrincipal principal)
	{
		using GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		AuthenticatedUserSid = geminiRequestContext.AuthenticatedUserSid;
		AuthenticatedUserDomain = geminiRequestContext.AuthenticatedUserName.Split('\\')[0];
		AuthenticatedUserName = geminiRequestContext.AuthenticatedUserName.Split('\\')[1];
		UserContext = geminiRequestContext.UserContext;
		AuthenticationType = principal.Identity.AuthenticationType;
	}

	public void SetUserUpdatedAt(string timeEpochSec)
	{
		if (!string.IsNullOrEmpty(timeEpochSec) && long.TryParse(timeEpochSec, out var result))
		{
			_userUpdatedAtSec = DateTimeOffset.FromUnixTimeSeconds(result).UtcDateTime;
		}
	}

	public GeminiRequestContext(GeminiApplicationHost applicationHost, RequestContextType requestContextType)
	{
		_contextId = Interlocked.Increment(ref s_contextIdCounter);
		_requestContextType = requestContextType;
		_authenticationType = string.Empty;
		_authenticatedUserSid = string.Empty;
		_authenticatedUserName = string.Empty;
		_authenticatedUserDomain = string.Empty;
		_remoteIPAddress = string.Empty;
		_remotePort = string.Empty;
		_localIPAddress = string.Empty;
		_applicationHost = applicationHost;
		if (_requestContextType == RequestContextType.SystemContext)
		{
			string accountName = string.Empty;
			SecurityIdentifier accountSid = null;
			IISHelper.IdentityType identityType = IISHelper.IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSERVICE;
			IISHelper.GetServiceAccountName(ref accountName, ref accountSid, ref identityType);
			_authenticatedUserName = accountName;
			_authenticatedUserSid = accountSid.ToString();
		}
		_requestTimer = new TimeRequest(this);
		_requestTimer.BeginRequest();
	}

	public virtual void EndRequest()
	{
		try
		{
			RequestTracer.TraceEnter(0, _area, _layer, "EndRequest");
			if (RequestTimer.EndTime == DateTime.MinValue)
			{
				_requestTimer.EndRequest();
				string traceTimings = _requestTimer.GetTraceTimings(executionTimeThreshold);
				long num = (long)_requestTimer.ExecutionSpan.TotalMilliseconds;
				if (num > executionTimeThreshold)
				{
					RequestTracer.Trace(0, TraceLevel.Error, _area, _layer, "{0}", traceTimings);
				}
				else if (num > 5000)
				{
					RequestTracer.Trace(0, TraceLevel.Warning, _area, _layer, "{0}", traceTimings);
				}
				else if (num > 3000)
				{
					RequestTracer.Trace(0, TraceLevel.Info, _area, _layer, "{0}", traceTimings);
				}
				else
				{
					RequestTracer.Trace(0, TraceLevel.Verbose, _area, _layer, "{0}", traceTimings);
				}
			}
			GetService<WebRequestLogger>().LogRequest(this);
		}
		catch (Exception ex)
		{
			GeminiTracingService.TraceExceptionEtw(0, _area, _layer, ex);
			throw;
		}
		finally
		{
			RequestTracer.TraceLeave(0, _area, _layer, "EndRequest");
		}
	}

	public virtual void EnterMethod(MethodInformation methodInformation)
	{
		_methodInfo = methodInformation;
		_methodInfo.InProgress = true;
		_methodExecutionThread = Thread.CurrentThread;
		if (CpuCycles == 0L)
		{
			CpuCycles = NativeMethods.GetCPUTime();
		}
		RequestTracer.TraceWebRequestStart(0, "GeminiRequestContext", "HostManagement", _methodInfo.Name);
		GeminiPerformanceEventSource.Log.MethodStart(Guid.Empty, ApplicationHost.InstanceId, methodInformation.Name);
	}

	public virtual void LeaveMethod()
	{
		if (!_isDisposed)
		{
			if (_methodExecutionThread == Thread.CurrentThread)
			{
				long cPUTime = NativeMethods.GetCPUTime();
				CpuCycles = cPUTime - CpuCycles;
			}
			if (_methodInfo != null && _methodInfo.InProgress)
			{
				RequestTracer.TraceWebRequestStop(0, "GeminiRequestContext", "HostManagement", _methodInfo.Name);
				GeminiPerformanceEventSource.Log.MethodStop(Guid.Empty, ApplicationHost.InstanceId, _methodInfo.Name, (long)_requestTimer.ExecutionSpan.TotalMilliseconds);
				_methodInfo.InProgress = false;
			}
			_methodExecutionThread = null;
		}
	}

	private static string GetStackTrace()
	{
		StackTrace stackTrace = new StackTrace(fNeedFileInfo: true);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < stackTrace.FrameCount; i++)
		{
			StackFrame frame = stackTrace.GetFrame(i);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("High up the call stack, Method: " + frame.GetMethod());
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "High up the call stack, Line Number: {0} \n", frame.GetFileLineNumber());
		}
		return stringBuilder.ToString();
	}

	public virtual TReturn GetService<TService, TReturn>() where TService : class, IGeminiServerService
	{
		if (!typeof(TReturn).IsAssignableFrom(typeof(TService)))
		{
			throw new InvalidCastException("Cannot convert " + typeof(TService).FullName + " to the specified type " + typeof(TReturn).FullName);
		}
		return (TReturn)(object)GetService<TService>();
	}

	public virtual TService GetService<TService>() where TService : class, IGeminiServerService
	{
		if (_isDisposed)
		{
			string stackTrace = GetStackTrace();
			GeminiTrace.TraceException(TraceType.General, new ObjectDisposedException(stackTrace, stackTrace));
			GeminiApplicationCore.LogException("", new ObjectDisposedException(stackTrace));
			throw new ObjectDisposedException("GeminiRequestContext");
		}
		Type typeFromHandle = typeof(TService);
		lock (_requestedServicesLock)
		{
			if (_requestedServices == null)
			{
				_requestedServices = new Dictionary<Type, IGeminiServerService>();
			}
			if (!_requestedServices.TryGetValue(typeFromHandle, out var value))
			{
				value = _applicationHost.GetService<TService>(this);
				_requestedServices.Add(typeFromHandle, value);
			}
			return (TService)value;
		}
	}

	ISystemMonitorEventQueueService IGeminiRequestContext.GetSystemMonitorEventQueueService()
	{
		return GetService<SystemMonitorEventQueueService>();
	}

	INameLookupService IGeminiRequestContext.GetNameLookupService()
	{
		return GetService<NameLookupService>();
	}

	ISecurityManagerService IGeminiRequestContext.GetSecurityManagerService()
	{
		return GetService<SecurityManagerService>();
	}

	IIntegrationService IGeminiRequestContext.GetIntegrationService()
	{
		return GetService<IntegrationService>();
	}

	IIdentityService IGeminiRequestContext.GetIdentityService()
	{
		return GetService<IdentityService>();
	}

	IGeminiIdentityService IGeminiRequestContext.GetGeminiIdentityService()
	{
		return GetService<GeminiIdentityService>();
	}

	ITokenCacheService IGeminiRequestContext.GetTokenCacheService()
	{
		return GetService<TokenCacheService>();
	}

	IEncryptionService IGeminiRequestContext.GetEncryptionService()
	{
		return GetService<EncryptionService>();
	}

	public virtual TComponent CreateComponent<TComponent>() where TComponent : GeminiResourceComponent, new()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<TComponent>(this);
	}

	IManagementServerComponent IGeminiRequestContext.CreateManagementServerComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ManagementServerComponent>(this);
	}

	ISecurityComponent IGeminiRequestContext.CreateSecurityComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<SecurityComponent>(this);
	}

	ISecurityTokenComponent IGeminiRequestContext.CreateSecurityTokenComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<SecurityTokenComponent>(this);
	}

	IDeviceComponent IGeminiRequestContext.CreateDeviceComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceComponent>(this);
	}

	IDeviceGroupComponent IGeminiRequestContext.CreateDeviceGroupComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceGroupComponent>(this);
	}

	IHardwareComponent IGeminiRequestContext.CreateHardwareComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HardwareComponent>(this);
	}

	IIntegrationRegistrationsComponent IGeminiRequestContext.CreateIntegrationRegistrationsComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<IntegrationRegistrationsComponent>(this);
	}

	IHardwareDefinitionComponent IGeminiRequestContext.CreateHardwareDefinitionComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HardwareDefinitionComponent>(this);
	}

	IHardwareDriverComponent IGeminiRequestContext.CreateHardwareDriverComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HardwareDriverComponent>(this);
	}

	IDriverSettingsComponent IGeminiRequestContext.CreateDriverSettingsComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DriverSettingsComponent>(this);
	}

	IIdentityCacheComponent IGeminiRequestContext.CreateIdentityCacheComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<IdentityCacheComponent>(this);
	}

	IRecordingStorageComponent IGeminiRequestContext.CreateRecordingStorageComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RecordingStorageComponent>(this);
	}

	IArchiveStorageComponent IGeminiRequestContext.CreateArchiveStorageComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ArchiveStorageComponent>(this);
	}

	IPatrollingProfileComponent IGeminiRequestContext.CreatePatrollingProfileComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<PatrollingProfileComponent>(this);
	}

	IUserDefinedEventComponent IGeminiRequestContext.CreateUserDefinedEventComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<UserDefinedEventComponent>(this);
	}

	ILicenseComponent IGeminiRequestContext.CreateLicenseComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<LicenseComponent>(this);
	}

	IBasicUserComponent IGeminiRequestContext.CreateBasicUserComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<BasicUserComponent>(this);
	}

	IRoleComponent IGeminiRequestContext.CreateRoleComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RoleComponent>(this);
	}

	IRecordingServerComponent IGeminiRequestContext.CreateRecordingServerComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RecordingServerComponent>(this);
	}

	IAuthorizeCookieComponent IGeminiRequestContext.CreateAuthorizeCookieComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<AuthorizeCookieComponent>(this);
	}

	IPerformanceStateRuleConfigComponent IGeminiRequestContext.CreatePerformanceStateRuleConfigComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<PerformanceStateRuleConfigComponent>(this);
	}

	ISiteComponent IGeminiRequestContext.CreateSiteComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<SiteComponent>(this);
	}

	IDataCollectionComponent IGeminiRequestContext.CreateDataCollectionComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DataCollectionComponent>(this);
	}

	IRuleComponent IGeminiRequestContext.CreateRuleComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RuleComponent>(this);
	}

	IRuleSourceComponent IGeminiRequestContext.CreateRuleSourceComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RuleSourceComponent>(this);
	}

	IRetentionOptionRoleRelationComponent IGeminiRequestContext.CreateRetentionOptionRoleRelationComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RetentionOptionRoleRelationComponent>(this);
	}

	IVideoWallComponent IGeminiRequestContext.CreateVideoWallComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<VideoWallComponent>(this);
	}

	IMonitorComponent IGeminiRequestContext.CreateMonitorComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MonitorComponent>(this);
	}

	IClientProfileComponent IGeminiRequestContext.CreateClientProfileComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ClientProfileComponent>(this);
	}

	IFailoverServerComponent IGeminiRequestContext.CreateFailoverServerComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<FailoverComponent>(this);
	}

	IHostedHardwareComponent IGeminiRequestContext.CreateHostedHardwareComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HostedHardwareComponent>(this);
	}

	IHostingServerComponent IGeminiRequestContext.CreateHostingServerComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HostingServerComponent>(this);
	}

	ITaskComponent IGeminiRequestContext.CreateTaskComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<TaskComponent>(this);
	}

	IRegisteredServerComponent IGeminiRequestContext.CreateRegisteredServerComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RegisteredServerComponent>(this);
	}

	ICustomPropertyComponent IGeminiRequestContext.CreateCustomPropertyComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<CustomPropertyComponent>(this);
	}

	ICustomSettingComponent IGeminiRequestContext.CreateCustomSettingComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<CustomSettingComponent>(this);
	}

	IBasicOwnerInfoComponent IGeminiRequestContext.CreateBasicOwnerInfoComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<BasicOwnerInfoComponent>(this);
	}

	IMarkedDataComponent IGeminiRequestContext.CreateMarkedDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MarkedDataComponent>(this);
	}

	IRestrictedMediaComponent IGeminiRequestContext.CreateRestrictedMediaComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RestrictedMediaComponent>(this);
	}

	IServerGroupComponent IGeminiRequestContext.CreateServerGroupComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ServerGroupComponent>(this);
	}

	IPluginEventComponent IGeminiRequestContext.CreatePluginEventComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<PluginEventComponent>(this);
	}

	ITimeProfileComponent IGeminiRequestContext.CreateTimeProfileComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<TimeProfileComponent>(this);
	}

	IMIPActionComponent IGeminiRequestContext.CreateMIPActionComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MIPActionComponent>(this);
	}

	IMIPObjectComponent IGeminiRequestContext.CreateMIPObjectComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MIPObjectComponent>(this);
	}

	IMIPItemComponent IGeminiRequestContext.CreateMIPItemComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MIPItemComponent>(this);
	}

	IMIPKindComponent IGeminiRequestContext.CreateMIPKindComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MIPKindComponent>(this);
	}

	IMIPServerGroupComponent IGeminiRequestContext.CreateMIPServerGroupComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MIPServerGroupComponent>(this);
	}

	IVideoWallFunctionalityComponent IGeminiRequestContext.CreateVideoWallFunctionalityComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<VideoWallFunctionalityComponent>(this);
	}

	IVideoWallPresetComponent IGeminiRequestContext.CreateVideoWallPresetComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<VideoWallPresetComponent>(this);
	}

	IViewComponent IGeminiRequestContext.CreateViewComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ViewComponent>(this);
	}

	IViewLayoutComponent IGeminiRequestContext.CreateViewLayoutComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ViewLayoutComponent>(this);
	}

	IAudioMessageComponent<AudioMessage> IGeminiRequestContext.CreateAudioMessageComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<AudioMessageComponent<AudioMessage>>(this);
	}

	IEventTypeComponent IGeminiRequestContext.CreateEventTypeComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<EventTypeComponent>(this);
	}

	IDatabaseSettings IGeminiRequestContext.GetDatabaseSettings()
	{
		return GetService<GeminiDatabaseSettingsService>().ReadSettings(this);
	}

	ISqlLogProcessor IGeminiRequestContext.CreateLogManager()
	{
		return new VmoLogManager(this);
	}

	ICompressorComponent IGeminiRequestContext.CreateCompressorComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<CompressorComponent>(this);
	}

	IMailNotificationProfileComponent IGeminiRequestContext.CreateMailNotificationProfileComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MailNotificationProfileComponent>(this);
	}

	IAccessControlMemberComponent IGeminiRequestContext.CreateAccessControlMemberComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<AccessControlMemberComponent>(this);
	}

	IEventTypeDataComponent IGeminiRequestContext.CreateEventTypeDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<EventTypeDataComponent>(this);
	}

	IRecorderDataComponent IGeminiRequestContext.CreateRecorderDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RecorderDataComponent>(this);
	}

	IHardwareDataComponent IGeminiRequestContext.CreateHardwareDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<HardwareDataComponent>(this);
	}

	IConfigurationDataComponent IGeminiRequestContext.CreateConfigurationDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ConfigurationDataComponent>(this);
	}

	IDeviceDataComponent IGeminiRequestContext.CreateDeviceDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceDataComponent>(this);
	}

	IDeviceTrackDataComponent IGeminiRequestContext.CreateDeviceTrackDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceTrackDataComponent>(this);
	}

	IDeviceDisabledDataComponent IGeminiRequestContext.CreateDeviceDisabledDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceDisabledDataComponent>(this);
	}

	IDeviceGroupDataComponent IGeminiRequestContext.CreateDeviceGroupDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceGroupDataComponent>(this);
	}

	IDeviceLinkDataComponent IGeminiRequestContext.CreateDeviceLinkDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceLinkDataComponent>(this);
	}

	IDeviceRecorderPluginDataComponent IGeminiRequestContext.CreateDeviceRecorderPluginDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<DeviceRecorderPluginDataComponent>(this);
	}

	IRecorderPluginDataComponent IGeminiRequestContext.CreateRecorderPluginDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RecorderPluginDataComponent>(this);
	}

	IExternalPluginsDataComponent IGeminiRequestContext.CreateExternalPluginsDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<ExternalPluginsDataComponent>(this);
	}

	IMatrixViewDataComponent IGeminiRequestContext.CreateMatrixViewDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MatrixViewDataComponent>(this);
	}

	IMetadataTypeDataComponent IGeminiRequestContext.CreateMetadataTypeDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MetadataTypeDataComponent>(this);
	}

	ISearchTemplateComponent IGeminiRequestContext.CreateSearchTemplateDataComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<SearchTemplateComponent>(this);
	}

	IMetadataSearchComponent IGeminiRequestContext.CreateMetadataSearchComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<MetadataSearchComponent>(this);
	}

	ILogServerConfigurationComponent IGeminiRequestContext.CreateLogServerConfigurationComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<LogServerConfigurationComponent>(this);
	}

	IRegisteredClaimComponent IGeminiRequestContext.CreateRegisteredClaimComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<RegisteredClaimComponent>(this);
	}

	IStateGroupComponent IGeminiRequestContext.CreateStateGroupComponent()
	{
		return GetService<GeminiResourceManagerService>().CreateComponent<StateGroupComponent>(this);
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		GeminiTracingService.TraceEnterEtw(0, _area, _layer, "Dispose");
		try
		{
			try
			{
				_methodExecutionThread = null;
			}
			catch (Exception ex)
			{
				GeminiTracingService.TraceExceptionEtw(0, _area, _layer, ex);
			}
			if (_requestTimer != null)
			{
				_requestTimer.Dispose();
			}
			base.Dispose(disposing);
			_isDisposed = true;
			GC.SuppressFinalize(this);
		}
		finally
		{
			GeminiTracingService.TraceEnterEtw(0, _area, _layer, "Dispose");
		}
	}
}
