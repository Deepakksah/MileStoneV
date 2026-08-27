using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices;

public sealed class AsyncAutoResetEvent : IDisposable
{
	private SemaphoreSlim _inner = new SemaphoreSlim(0, 1);

	private object _lock = new object();

	public void Set()
	{
		lock (_lock)
		{
			if (_inner.CurrentCount < 1)
			{
				_inner.Release();
			}
		}
	}

	public async Task WaitAsync()
	{
		await _inner.WaitAsync();
	}

	public async Task WaitAsync(CancellationToken cancellationToken)
	{
		await _inner.WaitAsync(cancellationToken);
	}

	public void Dispose()
	{
		_inner.Dispose();
	}
}
