namespace VideoOS.EventServer.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public static class StringExt
{
	public static string Truncate(this string value, int maxLength)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		if (value.Length > maxLength)
		{
			return value.Substring(0, maxLength) + "...";
		}
		return value;
	}
}
