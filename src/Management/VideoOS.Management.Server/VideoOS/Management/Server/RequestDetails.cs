using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VideoOS.Management.Server;

public class RequestDetails
{
	private object _object;

	private static int s_tempId;

	public int TempId { get; private set; }

	public string WebMethodName { get; private set; }

	public DateTime StartTime { get; private set; }

	public long ExecutionTime { get; private set; }

	public string IdentityName { get; private set; }

	public string RemoteIPAddress { get; private set; }

	public int ExecutionCount { get; private set; }

	public MethodInformation Method { get; private set; }

	public IList<MethodTime> RecursiveSqlCalls { get; private set; }

	public int SqlLogicalReads { get; private set; }

	public int SqlPhysicalReads { get; private set; }

	public int SqlCPUTime { get; private set; }

	public int SqlElapsedTime { get; private set; }

	public long CpuCycles { get; private set; }

	public RequestDetails(IGeminiRequestContext requestContext, GeminiWebRequestLoggingLevel loggingLevel)
	{
		_object = new object();
		WebMethodName = requestContext.Title();
		StartTime = requestContext.RequestTimer.StartTime;
		ExecutionTime = requestContext.RequestTimer.ExecutionTime;
		IdentityName = requestContext.AuthenticatedUserDomain;
		RemoteIPAddress = requestContext.RemoteIPAddress;
		ExecutionCount = 1;
		Method = requestContext.Method;
		RecursiveSqlCalls = ((loggingLevel == GeminiWebRequestLoggingLevel.All && requestContext is WcfRequestContext) ? requestContext.RequestLogger.RecursiveSqlCalls : null);
		SqlLogicalReads = requestContext.RequestLogger.LogicalReads;
		SqlPhysicalReads = requestContext.RequestLogger.PhysicalReads;
		SqlCPUTime = requestContext.RequestLogger.CpuTime;
		SqlElapsedTime = requestContext.RequestLogger.ElapsedTime;
		CpuCycles = requestContext.CpuCycles;
		TempId = Interlocked.Increment(ref s_tempId);
	}

	public RequestDetails CompressRequestDetails(RequestDetails requestDetails)
	{
		lock (_object)
		{
			ExecutionTime += requestDetails.ExecutionTime;
			if (requestDetails.RecursiveSqlCalls != null)
			{
				RecursiveSqlCalls.Concat(requestDetails.RecursiveSqlCalls);
			}
			SqlLogicalReads += requestDetails.SqlLogicalReads;
			SqlPhysicalReads += requestDetails.SqlPhysicalReads;
			SqlCPUTime += requestDetails.SqlCPUTime;
			SqlElapsedTime += requestDetails.SqlElapsedTime;
			CpuCycles += requestDetails.CpuCycles;
			ExecutionCount++;
			return this;
		}
	}
}
