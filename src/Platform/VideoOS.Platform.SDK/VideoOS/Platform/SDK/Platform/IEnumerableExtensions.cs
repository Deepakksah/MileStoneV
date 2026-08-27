using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.SDK.Platform;

public static class IEnumerableExtensions
{
	public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
	{
		return source.Reverse().Take(count).Reverse();
	}
}
