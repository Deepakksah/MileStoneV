namespace VideoOS.Management.VmoClient;

public abstract class RepositoryBase<T> : RepositoryBaseRoot where T : VmoObject
{
	public GenericRepository<T> GenericRepository { get; private set; }

	public RepositoryBase(IVmoRepositories repositories)
		: base(repositories)
	{
		GenericRepository = base.Repository.GetGenericRepository<T>();
	}
}
