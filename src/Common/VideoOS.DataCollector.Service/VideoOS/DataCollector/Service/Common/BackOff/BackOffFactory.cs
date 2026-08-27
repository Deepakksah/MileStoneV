using System;

namespace VideoOS.DataCollector.Service.Common.BackOff;

public class BackOffFactory : IBackOffFactory
{
	private readonly Func<IBackOffProvider> _factory;

	public BackOffFactory(Func<IBackOffProvider> factory)
	{
		_factory = factory;
	}

	public IBackOffProvider Create(int delayMilliseconds, int maxDelayMilliseconds)
	{
		IBackOffProvider backOffProvider = _factory();
		backOffProvider.RetryDelayMilliseconds = delayMilliseconds;
		backOffProvider.MaxRetryDelayMilliseconds = maxDelayMilliseconds;
		return backOffProvider;
	}
}
