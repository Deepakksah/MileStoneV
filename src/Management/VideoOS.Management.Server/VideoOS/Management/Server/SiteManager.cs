using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Common.Integration.Threading;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class SiteManager : IDisposable
{
	private class SyncInfo
	{
		public IGeminiRequestContext RequestContext { get; set; }

		public Site Site { get; private set; }

		public Site HomeSite { get; private set; }

		public SyncInfo(IGeminiRequestContext requestContext, Site siteToSync, Site homeSite)
		{
			RequestContext = requestContext;
			Site = siteToSync;
			HomeSite = homeSite;
		}
	}

	private class SyncInfoSpecificSite : SyncInfo
	{
		public Site MasterSite { get; private set; }

		public SyncInfoSpecificSite(IGeminiRequestContext requestContext, Site siteToSync, Site homeSite, Site masterSite)
			: base(requestContext, siteToSync, homeSite)
		{
			MasterSite = masterSite;
		}
	}

	private Timer _timer;

	private readonly object factorySyncLock = new object();

	private int _syncAllCounter;

	private int _notifyMasterCounter;

	private Dictionary<Guid, object> syncLocker = new Dictionary<Guid, object>();

	private SmartThreadPool _singleSiteSyncThreadPool = new SmartThreadPool(60000, 30, 1)
	{
		Name = "SiteManager single site sync thread pool"
	};

	private SmartThreadPool _notifyMasterThreadPool = new SmartThreadPool(60000, 10, 1)
	{
		Name = "SiteManager notify master thread pool"
	};

	private SmartThreadPool _fullSyncThreadPool = new SmartThreadPool(60000, 30, 4)
	{
		Name = "SiteManager full sync thread pool"
	};

	private Guid _homeSiteId = Guid.Empty;

	private object _homeSiteLock = new object();

	public Guid HomeSiteId
	{
		get
		{
			if (_homeSiteId == Guid.Empty)
			{
				lock (_homeSiteLock)
				{
					if (_homeSiteId == Guid.Empty)
					{
						using GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
						using SiteComponent siteComponent = geminiRequestContext.CreateComponent<SiteComponent>();
						Site site = siteComponent.QueryHomeSite();
						_homeSiteId = new Guid(site.Id);
					}
				}
			}
			return _homeSiteId;
		}
	}

	private object GetSyncLock(Guid siteId)
	{
		object value;
		lock (factorySyncLock)
		{
			if (!syncLocker.TryGetValue(siteId, out value))
			{
				value = new object();
				syncLocker.Add(siteId, value);
			}
		}
		return value;
	}

	public SiteManager()
	{
		if (!GeminiApplicationCore.IsExecutingModuleTest)
		{
			TimeSpan dueTime = TimeSpan.FromMinutes(5.0);
			TimeSpan period = TimeSpan.FromHours(1.0);
			_timer = new Timer(OnTimer, null, dueTime, period);
		}
	}

	private void OnTimer(object state)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		SynchronizeAllSites(requestContext, waitForComplete: true);
	}

	public void Stop()
	{
		if (_timer != null)
		{
			_fullSyncThreadPool.Cancel(abortExecution: true);
			_fullSyncThreadPool.Dispose();
			_singleSiteSyncThreadPool.Cancel(abortExecution: true);
			_singleSiteSyncThreadPool.Dispose();
			_notifyMasterThreadPool.Cancel(abortExecution: true);
			_notifyMasterThreadPool.Dispose();
			_timer.Dispose();
			_timer = null;
		}
	}

	public void RunNow(IGeminiRequestContext requestContext, Site site)
	{
		if (GeminiApplicationCore.IsExecutingModuleTest)
		{
			return;
		}
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		Site homeSite = Site.FindHomeSite(requestContext, siteComponent);
		Site masterSite = Site.FindMasterSite(requestContext, siteComponent);
		_singleSiteSyncThreadPool.QueueWorkItem((WorkItemCallback)SynchronizeSpecifySite, (object)new SyncInfoSpecificSite(requestContext, site, homeSite, masterSite));
	}

	public void RunNowSync(IGeminiRequestContext requestContext, Site site)
	{
		if (GeminiApplicationCore.IsExecutingModuleTest)
		{
			return;
		}
		try
		{
			if (site != null)
			{
				Site site2;
				Site masterSite;
				using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
				{
					site2 = Site.FindHomeSite(requestContext, siteComponent);
					_homeSiteId = new Guid(site2.Id);
					masterSite = Site.FindMasterSite(requestContext, siteComponent);
				}
				SynchronizeSpecifySite(new SyncInfoSpecificSite(requestContext, site, site2, masterSite));
			}
		}
		catch
		{
		}
	}

	public void NotifyMaster(IGeminiRequestContext requestContext, Site masterSite, Site homeSite)
	{
		if (GeminiApplicationCore.IsExecutingModuleTest || Interlocked.Exchange(ref _notifyMasterCounter, 1) != 0)
		{
			return;
		}
		try
		{
			if (masterSite != null && homeSite != null)
			{
				GeminiTrace.Info(TraceType.Security, "Triggering notify master: masterSite='{0}', homeSite='{1}'", masterSite.Name, homeSite.Name);
				_notifyMasterThreadPool.QueueWorkItem((WorkItemCallback)StartSyncronizationOnMasterSite, (object)new SyncInfo(requestContext, masterSite, homeSite));
			}
		}
		finally
		{
			Interlocked.Exchange(ref _notifyMasterCounter, 0);
		}
	}

	public void SynchronizeAllSites(IGeminiRequestContext requestContext, bool waitForComplete)
	{
		if (GeminiApplicationCore.IsExecutingModuleTest || !_fullSyncThreadPool.IsIdle)
		{
			return;
		}
		if (Interlocked.Exchange(ref _syncAllCounter, 1) == 0)
		{
			try
			{
				GeminiTrace.Info(TraceType.Security, "Synchronization of all sites started");
				using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
				{
					Site site = Site.FindHomeSite(requestContext, siteComponent);
					Site site2 = Site.FindMasterSite(requestContext, siteComponent);
					if (site != null)
					{
						GeminiTrace.Info(TraceType.Security, "  Synchronize home site '{0}'", site.Name);
						_fullSyncThreadPool.QueueWorkItem((WorkItemCallback)SynchronizeSite, (object)new SyncInfo(requestContext, site, site));
					}
					if (site2 != null)
					{
						GeminiTrace.Info(TraceType.Security, "  Synchronize master site '{0}'", site2.Name);
						_fullSyncThreadPool.QueueWorkItem((WorkItemCallback)SynchronizeSite, (object)new SyncInfo(requestContext, site2, site));
					}
					SynchronizeSlaveSites(requestContext, siteComponent, site);
					NotifyMaster(requestContext, site2, site);
				}
				GeminiTrace.Info(TraceType.Security, "  WaitForComplete: {0}", waitForComplete);
				if (waitForComplete)
				{
					_fullSyncThreadPool.WaitForIdle();
				}
				return;
			}
			catch (DatabaseException ex)
			{
				GeminiTrace.Error(TraceType.Security, "An SQL error occurred synchronizing all sites. Exception={0}", ex.ToString());
				return;
			}
			catch (Exception ex2)
			{
				GeminiTrace.Error(TraceType.Security, "Unknown exception in SiteManager: {0}", ex2.ToString());
				throw;
			}
			finally
			{
				Interlocked.Exchange(ref _syncAllCounter, 0);
				GeminiTrace.Info(TraceType.Security, "Synchronization of all sites stopped");
			}
		}
		GeminiTrace.Warning(TraceType.Security, "Synchronization of all sites denied - already running");
	}

	private void SynchronizeSlaveSites(IGeminiRequestContext requestContext, ISiteComponent siteComponent, Site homeSite)
	{
		try
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "FederatedSites");
		}
		catch (FeatureNotLicensedException)
		{
			GeminiTrace.Info(TraceType.Security, "Not synchronizing slave sites. MFA feature is not licensed.");
			return;
		}
		Site[] array = siteComponent.QuerySlaveSites(homeSite.Id);
		foreach (Site site in array)
		{
			GeminiTrace.Info(TraceType.Security, "  Synchronize slave site '{0}'", site.Name);
			_fullSyncThreadPool.QueueWorkItem((WorkItemCallback)SynchronizeSite, (object)new SyncInfo(requestContext, site, homeSite));
		}
	}

	private object SynchronizeSpecifySite(object syncInformation)
	{
		try
		{
			SynchronizeSite(syncInformation);
			SyncInfoSpecificSite syncInfoSpecificSite = syncInformation as SyncInfoSpecificSite;
			SiteSynchronizationManager.StartSynchronizeOnMasterSite(syncInfoSpecificSite.Site, syncInfoSpecificSite.HomeSite);
			NotifyMaster(syncInfoSpecificSite.RequestContext, syncInfoSpecificSite.MasterSite, syncInfoSpecificSite.HomeSite);
		}
		catch (Exception ex)
		{
			GeminiTrace.Info(TraceType.Security, "Unknown exception in SiteManager: {0}", ex.ToString());
		}
		return null;
	}

	private object SynchronizeSite(object syncInformation)
	{
		SyncInfo syncInfo = null;
		try
		{
			syncInfo = syncInformation as SyncInfo;
			if (syncInfo.Site == null)
			{
				return null;
			}
			lock (GetSyncLock(new Guid(syncInfo.Site.Id)))
			{
				if (syncInfo.Site.SiteType == SiteType.Home)
				{
					SiteSynchronizationManager.SynchronizeHomeSite(syncInfo.RequestContext, syncInfo.Site);
				}
				if (syncInfo.Site.SiteType == SiteType.Master)
				{
					SiteSynchronizationManager.SynchronizeMasterSite(syncInfo.RequestContext, syncInfo.Site, syncInfo.HomeSite);
				}
				if (syncInfo.Site.SiteType == SiteType.Slave)
				{
					SiteSynchronizationManager.SynchronizeSlaveSite(syncInfo.RequestContext, syncInfo.Site, syncInfo.HomeSite);
				}
			}
		}
		catch (Exception ex)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (syncInfo != null)
			{
				text = syncInfo.HomeSite.Name;
				text2 = syncInfo.Site.Name;
			}
			GeminiTrace.Warning(TraceType.Security, "SynchronizeSite {0} from {1}, Exception = {2}", text2, text, ex.ToString());
		}
		return null;
	}

	private object StartSyncronizationOnMasterSite(object syncInformation)
	{
		try
		{
			SyncInfo syncInfo = syncInformation as SyncInfo;
			if (syncInfo.Site == null)
			{
				return null;
			}
			if (syncInfo.Site.SiteType == SiteType.Master)
			{
				SiteSynchronizationManager.StartSynchronizeOnMasterSite(syncInfo.Site, syncInfo.HomeSite);
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Info(TraceType.Security, "Unknown exception in SiteManager: {0}", ex.ToString());
		}
		return null;
	}

	public void Dispose()
	{
		Stop();
	}
}
