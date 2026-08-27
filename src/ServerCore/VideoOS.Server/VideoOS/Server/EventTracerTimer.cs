using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Server;

public struct EventTracerTimer : IDisposable
{
	private readonly string _methodName;

	public EventTracerTimer([CallerMemberName] string methodName = "", object additionalParameters = null)
	{
		_methodName = methodName;
		EventTracer.Log.TraceMethodStart(_methodName, additionalParameters);
	}

	public void Dispose()
	{
		EventTracer.Log.TraceMethodStop(_methodName);
	}
}
