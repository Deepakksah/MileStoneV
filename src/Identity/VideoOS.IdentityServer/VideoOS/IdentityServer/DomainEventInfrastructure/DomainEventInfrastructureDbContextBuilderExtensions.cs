using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Data;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public static class DomainEventInfrastructureDbContextBuilderExtensions
{
	private class OutboxMessagesVerificationFilter<TContext>(IServiceProvider serviceProvider) : IStartupFilter where TContext : DbContext
	{
		public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
		{
			return delegate(IApplicationBuilder app)
			{
				using IServiceScope serviceScope = serviceProvider.CreateScope();
				TContext requiredService = serviceScope.ServiceProvider.GetRequiredService<TContext>();
				VerifyOutboxMessagesTable(requiredService);
				next(app);
			};
		}

		private static void VerifyOutboxMessagesTable(TContext dbContext)
		{
			IEntityType entityType = dbContext.Model.GetEntityTypes().FirstOrDefault((IEntityType et) => et.GetTableName() == "OutboxMessages");
			if (entityType == null)
			{
				throw new InvalidOperationException("The 'OutboxMessages' table is not configured in the DbContext of type " + dbContext.GetType().FullName + ".");
			}
		}
	}

	public static DbContextBuilder<TContext> AddDomainEventInterceptor<TContext>(this DbContextBuilder<TContext> builder) where TContext : DbContext
	{
		builder.AddPostAction(delegate
		{
			builder.Services.AddTransient<IStartupFilter, OutboxMessagesVerificationFilter<TContext>>();
		});
		return builder.AddInterceptor<DomainEventsInterceptor>();
	}
}
