using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("SmartClientProfile")]
public class SmartClientProfile : ClientProfile
{
	public SmartClientProfile(ManagementServer managementServer)
		: base(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), managementServer.Repositories, isInitialized: true)
	{
	}

	public SmartClientProfile(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: false)
	{
	}

	public SmartClientProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
