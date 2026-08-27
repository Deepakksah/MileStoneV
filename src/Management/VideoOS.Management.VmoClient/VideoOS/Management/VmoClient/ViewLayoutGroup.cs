using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ViewLayoutGroup")]
public class ViewLayoutGroup : ServerGroup
{
	public ViewLayoutCollection ViewLayouts { get; private set; }

	public ViewLayoutGroup(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer", "cannot be null");
		}
		SetPropertyValue("ParentServerGroupId", "430C2FE2-B233-42DA-B508-1A22EF03383D");
	}

	public ViewLayoutGroup(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public ViewLayoutGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		ViewLayouts = new ViewLayoutCollection(LoadViewLayouts, this, isInitialized);
	}

	private ViewLayout[] LoadViewLayouts(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ViewLayoutGroup, ViewLayout>(base.Id, refreshCachedObjects);
	}
}
