using System;
using Microsoft.EntityFrameworkCore;

namespace VideoOS.IdentityServer.Configuration.Interfaces;

public interface IDbContextConfigure
{
	Action<DbContextOptionsBuilder> ContextOptionsBuilder { get; }
}
