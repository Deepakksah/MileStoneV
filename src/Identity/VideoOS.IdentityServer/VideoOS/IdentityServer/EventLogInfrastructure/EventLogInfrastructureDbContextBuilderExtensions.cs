using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.EventLogs;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public static class EventLogInfrastructureDbContextBuilderExtensions
{
	public static DbContextBuilder<TContext> AddEventLogInterceptor<TContext>(this DbContextBuilder<TContext> builder) where TContext : DbContext
	{
		builder.Services.TryAddScoped<IEventLogFactory, EventLogFactory>();
		if (!builder.Services.Any((ServiceDescriptor s) => s.ServiceType == typeof(IAuditStrategyHandler) && s.ImplementationType == typeof(VmsUserAuditStrategy)))
		{
			builder.Services.AddScoped<IAuditStrategyHandler, VmsUserAuditStrategy>();
		}
		if (!builder.Services.Any((ServiceDescriptor s) => s.ServiceType == typeof(IAuditStrategyHandler) && s.ImplementationType == typeof(ClientAuditStrategy)))
		{
			builder.Services.AddScoped<IAuditStrategyHandler, ClientAuditStrategy>();
		}
		return builder.AddInterceptor<EventLogInterceptor>();
	}
}
