namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public static class StringExtension
{
	public static string ToCamelcase(this string original)
	{
		return original.Length switch
		{
			0 => original, 
			1 => original.ToLowerInvariant(), 
			_ => char.ToLowerInvariant(original[0]) + original.Substring(1), 
		};
	}
}
