using System;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.DataCollector.Service.Configuration;

namespace VideoOS.DataCollector.Service.Common.ExtensionMethods;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection ConfigureAndValidateOptions(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, "services");
		services.AddOptions<PerformanceCounterOptions>().BindConfiguration("PerformanceCounterSettings").ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddOptions<RuleOptions>().BindConfiguration("RuleSettings").ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddOptions<ClusterOptions>().BindConfiguration("ClusterSettings").ValidateDataAnnotations()
			.ValidateOnStart();
		return services;
	}
}
