using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class InitializeExternalProvidersService : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	private readonly ILogger _logger;

	public InitializeExternalProvidersService(IServiceProvider serviceProvider, ILogger<InitializeExternalProvidersService> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("External provider initialization started...");
		using IServiceScope serviceScope = _serviceProvider.CreateScope();
		ILoginProviderRepository service = serviceScope.ServiceProvider.GetService<ILoginProviderRepository>();
		IExternalProviderManager externalProviderManager = serviceScope.ServiceProvider.GetService<IExternalProviderManager>();
		IEnumerable<LoginProvider> enumerable;
		try
		{
			enumerable = await service.GetLoginProviders();
		}
		catch (Exception ex) when (ex is RetryLimitExceededException || ex is SqlException || ex is FormatException)
		{
			throw new ConfigurationErrorsException("Failed to load external IDP configuration from database.", ex);
		}
		foreach (LoginProvider item in enumerable)
		{
			try
			{
				externalProviderManager.AddProvider(item);
			}
			catch (ScopesFormatInvalidException)
			{
				throw new ConfigurationErrorsException("The scopes for provider " + item.DisplayName + " has an invalid format.");
			}
		}
		_logger.LogInformation("External provider initialization completed.");
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
