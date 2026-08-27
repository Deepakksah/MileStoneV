using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.Server;

public interface ITraceRequest
{
	bool IsTracing(int tracepoint);

	void Trace(int tracepoint, TraceLevel level, string area, string layer, string format, params object[] args);

	void TraceEnter(int tracepoint, string area, string layer, [CallerMemberName] string methodName = null);

	void TraceLeave(int tracepoint, string area, string layer, [CallerMemberName] string methodName = null);

	void TraceWebRequestStart(int tracepoint, string area, string layer, string methodName);

	void TraceWebRequestStop(int tracepoint, string area, string layer, string methodName);

	void TraceException(int tracepoint, string area, string layer, Exception ex);

	void TraceException(int tracepoint, TraceLevel level, string area, string layer, Exception ex);

	void TraceSql(int tracepoint, string dataSource, string database, string operation, short retries, bool success, int executionTime);
}
