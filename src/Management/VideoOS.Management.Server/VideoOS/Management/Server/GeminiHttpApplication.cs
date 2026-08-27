using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using VideoOS.Management.Server.Properties;
using VideoOS.Management.Server.RestApi;

namespace VideoOS.Management.Server;

public abstract class GeminiHttpApplication : HttpApplication, IRegisteredObject
{
	private const string Area = "GeminiServiceHost";

	private const string Layer = "WebService";

	private static List<Type> _fatalExceptionTypes = new List<Type>(new Type[1] { typeof(OutOfMemoryException) });

	private static bool s_ShutdownReceived;

	private object _registerLock = new object();

	private bool _registered;

	private int _shutdownCount;

	public GeminiRequestContext RequestContext { get; set; }

	public static void ThrowIfShutdownReceived()
	{
		if (s_ShutdownReceived)
		{
			throw new ShutdownException();
		}
	}

	public void Stop(bool immediate)
	{
		if (immediate)
		{
			SafeUnregisterWithHostingEnvironment();
			return;
		}
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			SafeStopApplication();
			SafeUnregisterWithHostingEnvironment();
		});
	}

	public void InitiateShutdownIfNecessary(Exception ex)
	{
		if (_fatalExceptionTypes.Contains(ex.GetType()))
		{
			HostingEnvironment.InitiateShutdown();
		}
	}

	protected void Shutdown()
	{
		s_ShutdownReceived = true;
		NotificationManager.Shutdown();
	}

	private void ExceptionHandler(Exception ex, bool isInitializeError)
	{
		try
		{
			InitiateShutdownIfNecessary(ex);
			if (isInitializeError)
			{
				_ = VmoResources.VMO63012;
			}
			else
			{
				_ = VmoResources.VMO63013;
			}
		}
		catch
		{
		}
	}

	private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
	{
		ExceptionHandler((Exception)e.ExceptionObject, isInitializeError: false);
	}

	public Exception ServiceExceptionHandler(Exception ex)
	{
		if (ex is FaultException<ExceptionDetail> result)
		{
			return result;
		}
		return ServerExceptionUtilities.LogException(ex);
	}

	private void SafeRegisterWithHostingEnvironment()
	{
		lock (_registerLock)
		{
			if (!_registered)
			{
				HostingEnvironment.RegisterObject(this);
				_registered = true;
			}
		}
	}

	private void SafeUnregisterWithHostingEnvironment()
	{
		lock (_registerLock)
		{
			if (_registered)
			{
				HostingEnvironment.UnregisterObject(this);
				_registered = false;
			}
		}
	}

	protected void Application_Start(object sender, EventArgs e)
	{
		GeminiTrace.Info(TraceType.General, "Initializing VideoOS Management Server");
		try
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
			SafeRegisterWithHostingEnvironment();
			try
			{
				GeminiTraceListener.SetDefaultListener();
			}
			catch (ConfigurationErrorsException ex)
			{
				GeminiTracingService.TraceExceptionEtw(0, "GeminiServiceHost", "WebService", ex);
				GeminiApplicationCore.Log(string.Format(CultureInfo.InvariantCulture, Resources.FailedReadingTracingConfiguration, ex.Message), GeminiEventId.InvalidTracingConfiguration, EventLogEntryType.Warning);
			}
			GeminiApplicationCore.StartGemini();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			GeminiTrace.Info(TraceType.General, "VideoOS Management Server application started.");
		}
		catch (Exception)
		{
			GeminiTrace.Error(TraceType.General, "VideoOS Management Server application encountered an error during startup.");
			throw;
		}
	}

	protected void Application_BeginRequest(object sender, EventArgs e)
	{
		GeminiTracingService.TraceEnterEtw(0, "GeminiServiceHost", "WebService", "BeginRequest");
		if (base.Context.Items.Contains("WcfRequestContext"))
		{
			RequestContext = (GeminiRequestContext)base.Context.Items["WcfRequestContext"];
		}
		else
		{
			RequestContext = (GeminiRequestContext)base.Context.Items["GeminiRequestContext"];
		}
	}

	protected void Application_EndRequest(object sender, EventArgs e)
	{
		GeminiTrace.Info(TraceType.General, "Application request processing ended.");
	}

	protected void Application_End(object sender, EventArgs e)
	{
		SafeStopApplication();
	}

	private void SafeStopApplication()
	{
		if (Interlocked.Increment(ref _shutdownCount) <= 1)
		{
			if (GeminiApplicationCore.RuntimeServiceHost != null)
			{
				GeminiApplicationCore.StopGemini();
			}
			StringBuilder stringBuilder = new StringBuilder("VideoOS Management Server application stopped.");
			if (HostingEnvironment.ShutdownReason != ApplicationShutdownReason.None)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "  VideoOS Management Servers is being shutdown for the following reason: {0}", HostingEnvironment.ShutdownReason);
			}
			GeminiTrace.Info(TraceType.General, stringBuilder.ToString());
		}
	}

	protected void Application_Error(object sender, EventArgs e)
	{
		Exception lastError = base.Server.GetLastError();
		if (lastError != null)
		{
			GeminiTrace.Error(TraceType.General, VmoResources.VMO63014 + Environment.NewLine + lastError);
		}
	}
}
