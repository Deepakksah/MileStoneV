namespace VideoOS.Management.Server;

public static class RequestContextExtensions
{
	public static string GetTraceTimingsAsString(this IGeminiRequestContext requestContext)
	{
		if (!(requestContext.RequestTimer is GeminiRequestContext.TimeRequest timeRequest))
		{
			return null;
		}
		return timeRequest.TraceTimingsAsString;
	}

	public static string Title(this IGeminiRequestContext requestContext)
	{
		return requestContext.Method?.Name ?? string.Empty;
	}
}
