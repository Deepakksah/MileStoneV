using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace VideoOS.Mobile.ReverseProxy;

public static class HttpRequestHeadersExtensions
{
	public static void ExtendHeader(this HttpRequestHeaders headers, string name, string value)
	{
		if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
		{
			return;
		}
		List<string> list = new List<string>();
		if (((HttpHeaders)headers).Contains(name))
		{
			foreach (string value2 in ((HttpHeaders)headers).GetValues(name))
			{
				if (!string.IsNullOrEmpty(value))
				{
					list.Add(value2 + ',' + value);
				}
			}
		}
		if (!list.Any())
		{
			list.Add(value);
		}
		if (((HttpHeaders)headers).Contains(name))
		{
			((HttpHeaders)headers).Remove(name);
		}
		foreach (string item in list)
		{
			((HttpHeaders)headers).Add(name, item);
		}
	}

	public static void Set(this HttpRequestHeaders headers, string name, string value)
	{
		if (((HttpHeaders)headers).Contains(name))
		{
			((HttpHeaders)headers).Remove(name);
		}
		((HttpHeaders)headers).Add(name, value);
	}
}
