using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.DomainEventInfrastructure;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Security;

namespace VideoOS.IdentityServer.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly IDataProtector _dataProtector;

	private readonly ILogger _logger;

	public DbSet<AdminIdentity> AdminIdentities { get; set; }

	public DbSet<EventLog> EventLogs { get; set; }

	public DbSet<LoginProvider> LoginProviders { get; set; }

	public DbSet<LoginProviderClient> LoginProviderClients { get; set; }

	public DbSet<TrustedIssuer> TrustedIssuers { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<IApplicationDbContext> logger, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
		: base(options)
	{
		_logger = logger;
		_dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.LoginProviderPurposeString);
	}

	private string Encrypt(string plainText)
	{
		return _dataProtector.Protect(plainText);
	}

	private string Decrypt(string protectedData)
	{
		try
		{
			return _dataProtector.Unprotect(protectedData);
		}
		catch (CryptographicException exception)
		{
			_logger.LogError(exception, "Unable to decrypt login provider client secret, setting client secret to empty string");
			return string.Empty;
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.HasDefaultSchema("dbo");
		modelBuilder.Entity<AdminIdentity>().HasIndex((AdminIdentity t) => t.Identity).IsUnique();
		modelBuilder.Entity<LoginProvider>().HasIndex((LoginProvider t) => new { t.Authority }).IsUnique();
		modelBuilder.Entity<LoginProvider>().HasIndex((LoginProvider t) => new { t.DisplayName }).IsUnique();
		modelBuilder.Entity<LoginProvider>().Property((LoginProvider x) => x.ClientSecret).HasConversion((string x) => Encrypt(x), (string x) => Decrypt(x));
		modelBuilder.Entity<TrustedIssuer>().HasKey((TrustedIssuer t) => t.Id);
		modelBuilder.Entity<TrustedIssuer>().Property((TrustedIssuer t) => t.Address).HasColumnType("varchar(450)")
			.HasMaxLength(450)
			.IsRequired();
		modelBuilder.Entity<TrustedIssuer>().HasIndex((TrustedIssuer t) => new { t.Address }).IsUnique();
		modelBuilder.Entity<TrustedIssuer>().Property((TrustedIssuer t) => t.Issuer).HasColumnType("varchar(450)")
			.HasMaxLength(450)
			.IsRequired();
		modelBuilder.Entity<TrustedIssuer>().HasIndex((TrustedIssuer t) => new { t.Issuer }).IsUnique();
		modelBuilder.Entity<TrustedIssuer>().Property((TrustedIssuer x) => x.Address).HasConversion((Uri uri) => uri.ToString(), (string str) => new Uri(str));
		modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration(includeInMigrations: true));
		modelBuilder.ApplyConfiguration(new EventLogsConfiguration(includeInMigrations: true));
	}
}
