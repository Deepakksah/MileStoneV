using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public abstract class RepositoryBaseRoot
{
	public IVmoRepositories Repository { get; private set; }

	public IServer ServerCommunication
	{
		[DebuggerStepThrough]
		get
		{
			return Repository.ServerCommunication;
		}
	}

	public RepositoryBaseRoot(IVmoRepositories repository)
	{
		Repository = repository;
	}
}
