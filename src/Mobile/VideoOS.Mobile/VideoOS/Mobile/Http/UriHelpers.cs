using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoOS.Mobile.Http;

public static class UriHelpers
{
	public static Uri FindUriByScheme(this IEnumerable<Task<HttpResponseMessage>> tasks, string scheme)
	{
		Task<HttpResponseMessage> task = tasks.FirstOrDefault((Task<HttpResponseMessage> t) => t.Result.RequestMessage.RequestUri.Scheme.Equals(scheme));
		if (task == null)
		{
			return null;
		}
		return task.Result.RequestMessage.RequestUri;
	}
}
