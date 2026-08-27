using System;
using System.Web;

namespace VideoOS.Management.Server;

public abstract class GeminiWebServiceBase
{
	private GeminiHttpApplication _geminiHttpApplication;

	private GeminiRequestContext _requestContext;

	private MethodInformation _methodInformation;

	protected GeminiRequestContext RequestContext => _requestContext;

	protected GeminiWebServiceBase()
		: this(connect: true)
	{
	}

	protected GeminiWebServiceBase(bool connect)
	{
		HttpContext current = HttpContext.Current;
		_geminiHttpApplication = current.ApplicationInstance as GeminiHttpApplication;
		_requestContext = _geminiHttpApplication.RequestContext;
		if (HttpContext.Current.Items.Contains("WcfRequestContext"))
		{
			_requestContext = (GeminiRequestContext)HttpContext.Current.Items["WcfRequestContext"];
		}
		if (connect)
		{
			CheckConnect();
		}
	}

	protected void CheckConnect()
	{
		try
		{
			((IGeminiRequestContext)_requestContext).GetSecurityManagerService().CheckApplicationPermission(_requestContext, "CONNECT");
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	protected virtual void EnterWebMethod(MethodInformation methodInformation)
	{
		if (_requestContext != null)
		{
			_methodInformation = methodInformation;
			_requestContext.EnterMethod(methodInformation);
		}
		if (_requestContext is WcfRequestContext)
		{
			GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.CurrentServerRequests).Increment();
			GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.CurrentServerRequestsSec).Increment();
		}
	}

	protected virtual void LeaveWebMethod()
	{
		if (_requestContext != null)
		{
			_requestContext.LeaveMethod();
		}
		if (_requestContext is WcfRequestContext)
		{
			GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.CurrentServerRequests).Decrement();
			GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.AverageResponseTime).IncrementTicks(_requestContext.RequestTimer.ExecutionSpan);
			GeminiPerformanceCounterManager.GetPerformanceCounter(PerformanceCounterHelper.AverageResponseTimeBase).Increment();
		}
	}

	protected virtual Exception HandleException(Exception ex)
	{
		((GeminiHttpApplication)HttpContext.Current.ApplicationInstance).InitiateShutdownIfNecessary(ex);
		return ((GeminiHttpApplication)HttpContext.Current.ApplicationInstance).ServiceExceptionHandler(ex);
	}
}
