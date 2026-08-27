using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public static class DomainEventInfrastructureServiceExtensions
{
	public static DbContextBuilder<TContext> AddDbContextWithInterceptor<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> builderConfigure) where TContext : DbContext
	{
		return new DbContextBuilder<TContext>(services, builderConfigure);
	}

	public static IServiceCollection RegisterDomainEvent<TEntity, TDomainEvent>(this IServiceCollection services, EntityState entityState, Func<TEntity, TDomainEvent> factory) where TEntity : class where TDomainEvent : DomainEvent
	{
		services.AddSingleton((IDomainEventRegistration)new DomainEventRegistration<TEntity, TDomainEvent>(entityState, factory));
		return services;
	}

	public static IServiceCollection AddDomainEventInfrastructure(this IServiceCollection services)
	{
		services.AddOptions<OutboxProcessorOptions>().Configure(delegate(OutboxProcessorOptions settings, IConfiguration configuration)
		{
			configuration.GetSection("OutboxProcessorSettings").Bind(settings);
		});
		services.AddSingleton<DomainEventsRegistry>();
		services.AddScoped<DomainEventsInterceptor>();
		services.AddSingleton<IOutboxMetricsTracker, OutboxMetricsTracker>();
		services.AddSingleton<ITransientExceptionDetector, TransientExceptionDetector>();
		services.AddTransient<IValidator<OutboxMessage>, OutboxMessageValidator>();
		services.AddScoped<IOutboxStore, OutboxStore>();
		services.AddHostedService<OutboxProcessor>();
		services.AddHostedService<OutboxMessageCleanupService>();
		return services;
	}
}
