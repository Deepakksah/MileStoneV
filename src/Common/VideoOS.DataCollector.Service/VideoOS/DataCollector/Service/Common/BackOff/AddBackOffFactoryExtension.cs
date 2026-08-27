using System;
using Microsoft.Extensions.DependencyInjection;

namespace VideoOS.DataCollector.Service.Common.BackOff;

public static class AddBackOffFactoryExtension
{
	public static void AddExponentialBackOffFactory(this IServiceCollection services)
	{
		services.AddTransient<IBackOffProvider, ExponentialBackOff>();
		services.AddSingleton((Func<IServiceProvider, Func<IBackOffProvider>>)((IServiceProvider x) => () => x.GetService<IBackOffProvider>()));
		services.AddSingleton<IBackOffFactory, BackOffFactory>();
	}
}
