using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;

namespace VideoOS.Common.Integration.Threading.Internal;

public class CallerThreadContext
{
	private static readonly MethodInfo getLogicalCallContextMethodInfo = typeof(Thread).GetMethod("GetLogicalCallContext", BindingFlags.Instance | BindingFlags.NonPublic);

	private static readonly MethodInfo setLogicalCallContextMethodInfo = typeof(Thread).GetMethod("SetLogicalCallContext", BindingFlags.Instance | BindingFlags.NonPublic);

	private static string HttpContextSlotName = GetHttpContextSlotName();

	private HttpContext _httpContext;

	private LogicalCallContext _callContext;

	public bool CapturedCallContext => _callContext != null;

	public bool CapturedHttpContext => _httpContext != null;

	private static string GetHttpContextSlotName()
	{
		FieldInfo field = typeof(HttpContext).GetField("CallContextSlotName", BindingFlags.Static | BindingFlags.NonPublic);
		if (field != null)
		{
			return (string)field.GetValue(null);
		}
		return "HttpContext";
	}

	private CallerThreadContext()
	{
	}

	public static CallerThreadContext Capture(bool captureCallContext, bool captureHttpContext)
	{
		CallerThreadContext callerThreadContext = new CallerThreadContext();
		if (captureCallContext && getLogicalCallContextMethodInfo != null)
		{
			callerThreadContext._callContext = (LogicalCallContext)getLogicalCallContextMethodInfo.Invoke(Thread.CurrentThread, null);
			if (callerThreadContext._callContext != null)
			{
				callerThreadContext._callContext = (LogicalCallContext)callerThreadContext._callContext.Clone();
			}
		}
		if (captureHttpContext && HttpContext.Current != null)
		{
			callerThreadContext._httpContext = HttpContext.Current;
		}
		return callerThreadContext;
	}

	public static void Apply(CallerThreadContext callerThreadContext)
	{
		if (callerThreadContext == null)
		{
			throw new ArgumentNullException("callerThreadContext");
		}
		if (callerThreadContext._callContext != null && setLogicalCallContextMethodInfo != null)
		{
			setLogicalCallContextMethodInfo.Invoke(Thread.CurrentThread, new object[1] { callerThreadContext._callContext });
		}
		if (callerThreadContext._httpContext != null)
		{
			HttpContext.Current = callerThreadContext._httpContext;
		}
	}
}
