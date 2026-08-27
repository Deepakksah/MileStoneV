using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using VideoOS.IdentityServer.Configuration;
using VideoOS.IdentityServer.Configuration.Interfaces;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Services;

public class ProductionDbContextConfigure(IDatabaseConfiguration databaseConfiguration) : IDbContextConfigure
{
	public Action<DbContextOptionsBuilder> ContextOptionsBuilder
	{
		get
		{
			IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
			IConfigurationSection section = configurationRoot.GetSection("ConnectionStrings");
			string idpConnectionString = section.GetValue(typeof(string), "ModuleTest")?.ToString();
			if (string.IsNullOrEmpty(idpConnectionString))
			{
				idpConnectionString = databaseConfiguration.ConnectionString;
			}
			return delegate(DbContextOptionsBuilder builder)
			{
				builder.UseSqlServer(idpConnectionString, delegate(SqlServerDbContextOptionsBuilder sql)
				{
					string name = typeof(ApplicationDbContext).Assembly.GetName().Name;
					sql.MigrationsAssembly(name);
					sql.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30.0), SqlErrorCodesToRetryOn.ErrorCodes);
				});
			};
		}
	}
}
