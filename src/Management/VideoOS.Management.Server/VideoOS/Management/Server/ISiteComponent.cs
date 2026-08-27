using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public interface ISiteComponent : IDisposable
{
	void AttachMasterSite(Site site);

	void AttachSlaveSite(Site site);

	void AuthorizeMaster();

	void DeleteSite(string siteId);

	void MarkInconsistent(string siteId);

	Site QueryHomeSite();

	Site QueryMasterSite();

	Site QuerySite(string siteId);

	string[] QuerySlaveSiteIds(string siteId);

	Site[] QuerySlaveSites();

	Site[] QuerySlaveSites(string siteId);

	Site[] QuerySlaveSitesByIds(string[] siteIds);

	Site[] QuerySlaveSitesComplete(string siteId);

	void SyncronizeSlaveTree(Site slaveSite, IVmoClient slaveServer);

	void SyncSite(Site site);

	void UpdateHomeSite(string name, string description, ServerAddress[] addresses, string version, string serviceAccount, int? synchronizationStatus, CustomProperty[] customProperties);

	void UpdateSiteHandshakeAndSyncStatus(Site site);

	void UpdateSiteServiceAccount(Site site);

	void UpdateSiteState(string siteId, SiteState siteState);

	void UpdateSiteSyncStatus(Site site);

	void UpdateSiteVersion(Site site);
}
