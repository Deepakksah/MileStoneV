using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MasterSite")]
public class MasterSite : Site
{
	public MasterSite(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Authorize()
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.AuthorizeMasterSite(base.Repositories, this);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Synchronize()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.SynchronizeSite(base.InternalId);
		Refresh();
	}
}
