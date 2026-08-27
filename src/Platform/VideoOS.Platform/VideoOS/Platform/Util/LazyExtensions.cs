using System;

namespace VideoOS.Platform.Util;

public static class LazyExtensions
{
	public static Lazy<T> Force<T>(this Lazy<T> lazyInstance)
	{
		if (!lazyInstance.IsValueCreated)
		{
			_ = lazyInstance.Value;
		}
		return lazyInstance;
	}
}
