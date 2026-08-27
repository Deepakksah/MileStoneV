using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Extensions;

public static class TaskExtensions
{
	public static ConfiguredTaskAwaitable IgnoreContext(this Task task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: false);
	}

	public static ConfiguredTaskAwaitable<TResult> IgnoreContext<TResult>(this Task<TResult> task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: false);
	}

	public static ConfiguredTaskAwaitable<int> IgnoreContext<TResult>(this Task<int> task)
	{
		return task.ConfigureAwait(continueOnCapturedContext: false);
	}
}
