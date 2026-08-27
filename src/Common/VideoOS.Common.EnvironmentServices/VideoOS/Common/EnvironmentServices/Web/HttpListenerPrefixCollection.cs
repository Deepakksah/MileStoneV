using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpListenerPrefixCollection : IHttpListenerPrefixCollection
{
	private System.Net.HttpListenerPrefixCollection _prefixes;

	public HttpListenerPrefixCollection(System.Net.HttpListenerPrefixCollection prefixes)
	{
		_prefixes = prefixes;
	}

	public void Add(string prefix)
	{
		_prefixes.Add(prefix);
	}
}
