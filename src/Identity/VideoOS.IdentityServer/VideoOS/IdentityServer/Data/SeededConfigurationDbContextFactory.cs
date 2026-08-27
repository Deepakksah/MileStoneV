using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using VideoOS.IdentityServer.Configuration;

namespace VideoOS.IdentityServer.Data;

public class SeededConfigurationDbContextFactory : IDesignTimeDbContextFactory<SeededConfigurationDbContext>
{
	public SeededConfigurationDbContext CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<SeededConfigurationDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<SeededConfigurationDbContext>();
		dbContextOptionsBuilder.UseSqlServer(new DatabaseConfiguration().ConnectionString);
		ServiceCollection services = new ServiceCollection();
		services.AddScoped<ConfigurationStoreOptions>();
		ServiceProvider serviceProvider = services.BuildServiceProvider();
		dbContextOptionsBuilder.UseApplicationServiceProvider(serviceProvider);
		return new SeededConfigurationDbContext(dbContextOptionsBuilder.Options);
	}
}
