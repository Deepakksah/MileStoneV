using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class RequestDetailsComparer : IEqualityComparer<RequestDetails>
{
	public int MaxCompressedThreshold { get; set; }

	public GeminiWebRequestLoggingLevel LoggingLevel { get; set; }

	public RequestDetailsComparer(int maxCompressedThreshold, GeminiWebRequestLoggingLevel loggingLevel)
	{
		MaxCompressedThreshold = maxCompressedThreshold;
		LoggingLevel = loggingLevel;
	}

	public bool Equals(RequestDetails x, RequestDetails y)
	{
		if (x == y)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		if ((x.ExecutionCount == 1 && x.ExecutionTime > MaxCompressedThreshold) || (y.ExecutionCount == 1 && y.ExecutionTime > MaxCompressedThreshold))
		{
			return false;
		}
		if (string.Equals(x.IdentityName, y.IdentityName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.WebMethodName, y.WebMethodName, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	public int GetHashCode(RequestDetails obj)
	{
		return obj?.GetHashCode() ?? 0;
	}
}
