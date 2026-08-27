using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Proxy.RestApi;

public class RestResourceCache
{
	public const int MinutesRetention = 30;

	public Dictionary<string, string> Cache { get; set; } = new Dictionary<string, string>();

	public DateTime ResetTime { get; set; }

	public void Clear()
	{
		Cache.Clear();
		ResetTime = DateTime.UtcNow + TimeSpan.FromMinutes(30.0);
	}

	public void Refresh()
	{
		if (DateTime.UtcNow >= ResetTime)
		{
			Clear();
		}
	}
}
