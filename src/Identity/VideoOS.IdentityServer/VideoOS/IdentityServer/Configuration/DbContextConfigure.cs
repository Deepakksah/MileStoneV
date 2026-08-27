using System;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Configuration.Interfaces;

namespace VideoOS.IdentityServer.Configuration;

public class DbContextConfigure : IDbContextConfigure
{
	public Action<DbContextOptionsBuilder> ContextOptionsBuilder { get; set; }
}
