using System;
using System.Net;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpListener : IHttpListener, IDisposable
{
	private System.Net.HttpListener _inner;

	private IHttpListenerPrefixCollection _prefixes;

	public IHttpListenerPrefixCollection Prefixes => _prefixes ?? (_prefixes = new HttpListenerPrefixCollection(_inner.Prefixes));

	public HttpListener()
	{
		_inner = new System.Net.HttpListener();
	}

	public void Dispose()
	{
		((IDisposable)_inner).Dispose();
		GC.SuppressFinalize(this);
	}

	public async Task<IHttpListenerContext> GetContextAsync()
	{
		return new HttpListenerContext(await _inner.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false));
	}

	public void Start()
	{
		_inner.Start();
	}

	public void Stop()
	{
		_inner.Stop();
	}
}
