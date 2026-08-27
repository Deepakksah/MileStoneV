using System;

namespace VideoOS.IdentityServer.Services;

public interface IPostMigratorHostedServiceHandler
{
	void Run(IServiceProvider serviceProvider);
}
