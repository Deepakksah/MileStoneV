using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace VideoOS.IdentityServer.Repositories.Context;

public class EfContextProvider<T> : IDbContextProvider<T> where T : DbContext
{
	public T Context { get; set; }

	public EfContextProvider(T context)
	{
		Context = context;
	}

	public IExecutionStrategy CreateExecutionStrategy()
	{
		return Context.Database.CreateExecutionStrategy();
	}
}
