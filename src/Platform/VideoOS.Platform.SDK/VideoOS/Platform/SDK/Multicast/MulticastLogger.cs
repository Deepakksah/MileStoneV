namespace VideoOS.Platform.SDK.Multicast;

public static class MulticastLogger
{
	public static void LogDebug(string where, string tx)
	{
		if (EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("MULTICASTLOG"))
		{
			EnvironmentManager.Instance.Log(error: false, where, tx);
		}
	}

	public static void LogError(string where, string tx)
	{
		EnvironmentManager.Instance.Log(error: true, where, tx);
	}
}
