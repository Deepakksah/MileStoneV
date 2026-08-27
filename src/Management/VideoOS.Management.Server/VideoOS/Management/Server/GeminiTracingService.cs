using System;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Management.Server;

public sealed class GeminiTracingService
{
	public static void TraceExceptionEtw(int tracepoint, string area, string layer, Exception ex)
	{
		TraceEtw(tracepoint, area, layer, "{0}", ex);
	}

	public static void TraceEnterEtw(int tracepoint, string area, string layer, string methodName, params object[] args)
	{
		TraceEtw(tracepoint, TraceLevel.Verbose, area, layer, "performance", "Entering " + methodName, args);
	}

	public static void TraceLeaveEtw(int tracepoint, string area, string layer, string methodName, params object[] args)
	{
		TraceEtw(tracepoint, TraceLevel.Verbose, area, layer, "performance", "Leaving " + methodName, args);
	}

	[Localizable(false)]
	public static void TraceEtw(int tracepoint, TraceLevel level, string area, string layer, string message, params object[] args)
	{
		TraceEtw(tracepoint, level, area, layer, null, message, args);
	}

	[Localizable(false)]
	public static void TraceEtw(int tracepoint, TraceLevel level, string area, string layer, string[] tags, string message, params object[] args)
	{
	}

	[Localizable(false)]
	public static void TraceEtw(int tracepoint, string area, string layer, string message, params object[] args)
	{
	}
}
