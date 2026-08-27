using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ChildSite")]
public class ChildSite : Site
{
	public DateTime MarkedInconsistent => GetPropertyValue<DateTime>("MarkedInconsistent");

	public bool IsInconsistent => base.LastConfirmedHandshake < MarkedInconsistent;

	public ChildSiteCollection ChildSites { get; private set; }

	public string ParentSiteId => GetPropertyValue<string>("MasterSiteId");

	public ChildSite ParentSite
	{
		get
		{
			if (string.IsNullOrEmpty(ParentSiteId))
			{
				return null;
			}
			return GetObjectById<ChildSite>(ParentSiteId);
		}
	}

	public ManagementServer ManagementServer => GetObjectById<ManagementServer>("909749AE-88C4-4780-A381-383D691E98FA");

	public ChildSite(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		ChildSites = new ChildSiteCollection(LoadChildSites, this, isInitialized);
	}

	public void SetMarkInconsistent()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.MarkChildSiteInconsistent(base.InternalId);
		Refresh();
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

	private ChildSite[] LoadChildSites(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ChildSite, ChildSite>(base.Id, refreshCachedObjects);
	}
}
