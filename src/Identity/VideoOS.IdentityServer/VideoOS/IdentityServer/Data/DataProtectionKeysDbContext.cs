using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VideoOS.IdentityServer.Data;

public class DataProtectionKeysDbContext : DbContext, IDataProtectionKeyContext
{
	public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

	public DataProtectionKeysDbContext(DbContextOptions<DataProtectionKeysDbContext> options)
		: base(options)
	{
	}
}
