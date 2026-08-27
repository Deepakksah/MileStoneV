namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IArchiveProxy
{
	ArchiveStorageDataContract QueryArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId);
}
