using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ViewGroup")]
public abstract class ViewGroup : ServerGroup
{
	public ViewGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
