using System;

namespace VideoOS.Common.Interfaces;

public interface IServiceContainer : IServiceProvider, IDisposable
{
	TService GetRequiredService<TService>();

	TService GetService<TService>() where TService : class;
}
