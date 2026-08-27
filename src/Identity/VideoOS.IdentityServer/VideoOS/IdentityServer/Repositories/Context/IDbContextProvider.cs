using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace VideoOS.IdentityServer.Repositories.Context;

public interface IDbContextProvider<T> where T : DbContext
{
	T Context { get; set; }

	IExecutionStrategy CreateExecutionStrategy();
}
