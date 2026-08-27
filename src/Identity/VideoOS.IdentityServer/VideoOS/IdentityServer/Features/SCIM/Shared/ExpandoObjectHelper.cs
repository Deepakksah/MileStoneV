using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ExpandoObjectHelper
{
	public static string GetMetaLocation(ExpandoObject expando)
	{
		if (expando == null)
		{
			return null;
		}
		if (!(expando.FirstOrDefault<KeyValuePair<string, object>>((KeyValuePair<string, object> x) => x.Key.Equals("meta", StringComparison.OrdinalIgnoreCase)).Value is IDictionary<string, object> source))
		{
			return null;
		}
		return source.FirstOrDefault((KeyValuePair<string, object> x) => x.Key.Equals("location", StringComparison.OrdinalIgnoreCase)).Value?.ToString();
	}
}
