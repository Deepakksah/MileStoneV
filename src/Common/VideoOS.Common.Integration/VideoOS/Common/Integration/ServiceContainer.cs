using System;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.Common.Interfaces;

namespace VideoOS.Common.Integration;

public sealed class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
{
	private readonly ServiceProvider _serviceProvider;

	private static IServiceContainer _serviceContainer;

	private bool _isDisposed;

	public static IServiceContainer Instance
	{
		get
		{
			if (_serviceContainer == null)
			{
				throw new InvalidOperationException("Service provider has not been setup");
			}
			return _serviceContainer;
		}
	}

	private ServiceContainer(IServiceCollection serviceCollection)
	{
		if (serviceCollection == null)
		{
			throw new ArgumentNullException("serviceCollection");
		}
		ServiceCollectionServiceExtensions.AddSingleton<IServiceContainer>(serviceCollection, (IServiceContainer)this);
		_serviceProvider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(serviceCollection);
	}

	public static IServiceContainer CreateInstance(IServiceCollection serviceCollection)
	{
		if (_serviceContainer != null)
		{
			throw new InvalidOperationException("Setting static instance must be completed before using the service container.");
		}
		_serviceContainer = new ServiceContainer(serviceCollection);
		return _serviceContainer;
	}

	public void Dispose()
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			_serviceContainer = null;
			_serviceProvider.Dispose();
			GC.SuppressFinalize(this);
		}
	}

	public TService GetRequiredService<TService>()
	{
		return ((TService)_serviceProvider.GetService(typeof(TService))) ?? throw new InvalidOperationException("No registration for " + typeof(TService).FullName);
	}

	public TService GetService<TService>() where TService : class
	{
		return (TService)_serviceProvider.GetService(typeof(TService));
	}

	public object GetService(Type serviceType)
	{
		return _serviceProvider.GetService(serviceType);
	}
}
