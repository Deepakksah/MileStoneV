using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoOS.Common.Integration.WebApi;

public class UnhandledExceptionResult : IHttpActionResult, IDisposable
{
	private HttpResponseMessage _httpResponseMessage;

	private bool _disposed;

	public UnhandledExceptionResult(HttpResponseMessage httpResponseMessage)
	{
		_httpResponseMessage = httpResponseMessage;
	}

	public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult<HttpResponseMessage>(_httpResponseMessage);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (_httpResponseMessage != null)
			{
				_httpResponseMessage.Dispose();
			}
			_disposed = true;
		}
	}
}
