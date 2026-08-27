using System.Collections.Concurrent;

namespace VideoOS.Management.Server;

public sealed class VideoWallCache
{
	private static ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

	public static string GetIdFromMonitorOrPreset(string monitorOrPresetId)
	{
		_cache.TryGetValue(monitorOrPresetId, out var value);
		return value;
	}

	public static void AddToCache(string monitorOrPresetId, string videoWallId)
	{
		_cache.TryAdd(monitorOrPresetId, videoWallId);
	}
}
