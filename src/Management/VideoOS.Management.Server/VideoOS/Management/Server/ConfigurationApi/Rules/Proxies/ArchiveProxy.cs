namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class ArchiveProxy : IArchiveProxy
{
	public ArchiveStorageDataContract QueryArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		return ArchiveStorage.QueryArchiveStorage(requestContext, archiveStorageId);
	}
}
