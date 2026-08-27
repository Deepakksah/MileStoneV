namespace VideoOS.Platform.SDK.Util;

public static class ServerTrace
{
	public static bool TraceServerCommunication;

	public static void BeforeCall(string where, string name, string uriOrId)
	{
		if (TraceServerCommunication)
		{
			EnvironmentManager.Instance.Log(error: false, where, string.Format("Before: {0}  -- {1}", name, uriOrId ?? ""));
		}
	}

	public static void AfterCall(string where, string name, string uriOrId)
	{
		if (TraceServerCommunication)
		{
			EnvironmentManager.Instance.Log(error: false, where, string.Format("After:  {0}  -- {1}", name, uriOrId ?? ""));
		}
	}

	public static void Info(string where, string name, string uriOrId)
	{
		if (TraceServerCommunication)
		{
			EnvironmentManager.Instance?.Log(error: false, where, string.Format("Info:   {0}  -- {1}", name, uriOrId ?? ""));
		}
	}

	public static void Info(string where, string text)
	{
		if (TraceServerCommunication)
		{
			EnvironmentManager.Instance?.Log(error: false, where, string.Format(text));
		}
	}
}
