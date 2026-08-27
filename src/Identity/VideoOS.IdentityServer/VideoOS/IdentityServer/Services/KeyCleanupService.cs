using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Data;

namespace VideoOS.IdentityServer.Services;

public class KeyCleanupService : IHostedService
{
	private readonly ILogger<KeyCleanupService> _logger;

	private readonly IServiceProvider _services;

	public KeyCleanupService(IServiceProvider services, ILogger<KeyCleanupService> logger)
	{
		_services = services;
		_logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = _services.CreateScope();
		DataProtectionKeysDbContext requiredService = scope.ServiceProvider.GetRequiredService<DataProtectionKeysDbContext>();
		IQueryable<DataProtectionKey> queryable = requiredService.DataProtectionKeys.Where((DataProtectionKey x) => !x.Xml.Contains("<X509Certificate>"));
		foreach (DataProtectionKey item in queryable)
		{
			_logger.LogInformation("Non certificate key found - {0}. Removing.", item.FriendlyName);
		}
		requiredService.DataProtectionKeys.RemoveRange(queryable);
		await requiredService.SaveChangesAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
