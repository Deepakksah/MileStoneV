using System;
using System.Net.Http;

namespace VideoOS.Mobile.ReverseProxy;

public class HttpMethods
{
	public static bool IsGet(string method)
	{
		return method.Equals(HttpMethod.Get.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsDelete(string method)
	{
		return method.Equals(HttpMethod.Delete.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsHead(string method)
	{
		return method.Equals(HttpMethod.Head.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsOptions(string method)
	{
		return method.Equals(HttpMethod.Options.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsPost(string method)
	{
		return method.Equals(HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsPut(string method)
	{
		return method.Equals(HttpMethod.Put.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool IsTrace(string method)
	{
		return method.Equals(HttpMethod.Trace.Method, StringComparison.InvariantCultureIgnoreCase);
	}

	public static HttpMethod GetMethod(string method)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (IsDelete(method))
		{
			return HttpMethod.Delete;
		}
		if (IsGet(method))
		{
			return HttpMethod.Get;
		}
		if (IsHead(method))
		{
			return HttpMethod.Head;
		}
		if (IsOptions(method))
		{
			return HttpMethod.Options;
		}
		if (IsPost(method))
		{
			return HttpMethod.Post;
		}
		if (IsPut(method))
		{
			return HttpMethod.Put;
		}
		if (IsTrace(method))
		{
			return HttpMethod.Trace;
		}
		return new HttpMethod(method);
	}

	public static bool HasContent(HttpMethod method)
	{
		if (!method.Equals(HttpMethod.Get) && !method.Equals(HttpMethod.Head) && !method.Equals(HttpMethod.Delete))
		{
			return !method.Equals(HttpMethod.Trace);
		}
		return false;
	}
}
