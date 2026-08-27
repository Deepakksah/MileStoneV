using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public sealed class Site
{
	private static bool _childSitesForExternalUsers = bool.Parse(Config.Instance.GetAppSetting("ChildSitesForExternalUsers", "false"));

	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime LastModified { get; set; }

	public DateTime LastStatusHandshake { get; set; }

	public SiteType SiteType { get; set; }

	public SiteState ConnectionState { get; set; }

	public ServerAddress[] Addresses { get; set; }

	public DateTime MarkedInconsistent { get; set; }

	public string MasterSiteId { get; set; }

	public string Version { get; set; }

	public string ServiceAccount { get; set; }

	public int SynchronizationStatus { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string[] RelatedSlaveSiteIds { get; set; }

	public static Site[] QuerySlaveSites(IGeminiRequestContext requestContext)
	{
		if (requestContext.IsBasicUser || (requestContext.IsExternalVmsUser && !_childSitesForExternalUsers))
		{
			return new Site[0];
		}
		Site[] sitesArray = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			sitesArray = siteComponent.QuerySlaveSites();
		}
		return FilterSitesBasedOnPermissions(requestContext, sitesArray);
	}

	public static Site[] QuerySlaveSites(IGeminiRequestContext requestContext, string siteId)
	{
		if (requestContext.IsBasicUser || (requestContext.IsExternalVmsUser && !_childSitesForExternalUsers))
		{
			return new Site[0];
		}
		return QuerySlaveSitesInternal(requestContext, siteId);
	}

	private static Site[] QuerySlaveSitesInternal(IGeminiRequestContext requestContext, string siteId)
	{
		Site[] sitesArray = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			sitesArray = siteComponent.QuerySlaveSites(siteId);
		}
		return FilterSitesBasedOnPermissions(requestContext, sitesArray);
	}

	public static Site[] QuerySites(IGeminiRequestContext requestContext, string[] ids)
	{
		if (requestContext.IsBasicUser || (requestContext.IsExternalVmsUser && !_childSitesForExternalUsers))
		{
			return new Site[0];
		}
		return QuerySitesInternal(requestContext, ids);
	}

	public static Site[] QuerySitesLegacy(IGeminiRequestContext requestContext, string[] ids)
	{
		if (requestContext.IsBasicUser || requestContext.IsExternalVmsUser)
		{
			return new Site[0];
		}
		return QuerySitesInternal(requestContext, ids);
	}

	private static Site[] QuerySitesInternal(IGeminiRequestContext requestContext, string[] ids)
	{
		Site[] sitesArray = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			sitesArray = siteComponent.QuerySlaveSitesByIds(ids);
		}
		return FilterSitesBasedOnPermissions(requestContext, sitesArray);
	}

	public static Site QueryHomeSite(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "CONNECT");
		return QueryHomeSiteNoSecurity(requestContext);
	}

	public static Site QueryHomeSiteNoSecurity(IGeminiRequestContext requestContext)
	{
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		return siteComponent.QueryHomeSite();
	}

	public static string[] QuerySlaveSiteIds(IGeminiRequestContext requestContext, string siteId)
	{
		if (requestContext.IsBasicUser || (requestContext.IsExternalVmsUser && !_childSitesForExternalUsers))
		{
			return new string[0];
		}
		return QuerySlaveSiteIdsInternal(requestContext, siteId);
	}

	public static string[] QuerySlaveSiteIdsLegacy(IGeminiRequestContext requestContext, string siteId)
	{
		if (requestContext.IsBasicUser || requestContext.IsExternalVmsUser)
		{
			return new string[0];
		}
		return QuerySlaveSiteIdsInternal(requestContext, siteId);
	}

	public static string[] QuerySlaveSiteIdsInternal(IGeminiRequestContext requestContext, string siteId)
	{
		string[] siteIdsArray = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			siteIdsArray = siteComponent.QuerySlaveSiteIds(siteId);
		}
		return FilterSiteIdsBasedOnPermissions(requestContext, siteIdsArray);
	}

	public static Site QueryMasterSite(IGeminiRequestContext requestContext)
	{
		Site site;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			site = siteComponent.QueryMasterSite();
		}
		if (site != null)
		{
			IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetSecurityManagerService().GetSecurityNamespaceFromToken(requestContext, site.Id);
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "GENERIC_READ", site.Id);
		}
		return site;
	}

	public static Site QuerySite(IGeminiRequestContext requestContext, string siteId)
	{
		IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetSecurityManagerService().GetSecurityNamespaceFromToken(requestContext, siteId);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "GENERIC_READ", siteId);
		Validation.CheckObjectId(ref siteId, "siteId", allowNullOrEmpty: false);
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		return siteComponent.QuerySite(siteId);
	}

	public static Site AttachMasterSite(IGeminiRequestContext requestContext, string masterSiteId, ServerAddress[] masterSiteAddresses, string masterSiteVersion)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "AddToFederatedHierarchy"))
		{
			throw new FeatureNotLicensedException();
		}
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		IVmoClient vmoClient = null;
		Site site = null;
		try
		{
			Site homeSite = null;
			using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
			{
				homeSite = FindHomeSite(requestContext, siteComponent);
			}
			using (ISiteComponent siteComponent2 = requestContext.CreateSiteComponent())
			{
				site = FindSite(requestContext, siteComponent2, masterSiteId);
			}
			if (site != null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Master site '{0}'' already added to home site", masterSiteId);
				GeminiTrace.Warning(TraceType.General, "AttachMasterSite: {0}", text);
				throw new SiteAlreadyAttachedException(text);
			}
			ServerVersion siteVersion = (string.IsNullOrEmpty(masterSiteVersion) ? new ServerVersion() : new ServerVersion(masterSiteVersion));
			RemoteSite remoteSite = new RemoteSite(masterSiteId, masterSiteAddresses, siteVersion);
			ISiteConnectionResult siteConnectionResult = ConnectToSite(homeSite, remoteSite);
			Uri serverAddress = siteConnectionResult.ServerAddress;
			vmoClient = siteConnectionResult.VmoClient;
			Version serverVersionFromSite = SitesUtil.GetServerVersionFromSite(serverAddress);
			if (serverVersionFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from master site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachMasterSite: {0}", text);
				throw new InvalidServerVersionException(text);
			}
			ServerInformationOld serverInformationFromSite = SitesUtil.GetServerInformationFromSite(serverAddress);
			if (serverInformationFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from master site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachMasterSite: {0}", text);
				throw new VmoUnauthorizedAccessException(text);
			}
			string text2 = null;
			string serviceAccount = serverInformationFromSite.ServiceAccount;
			try
			{
				if (new SecurityIdentifier(serverInformationFromSite.ServiceAccount).IsWellKnown(WellKnownSidType.NetworkServiceSid))
				{
					text2 = ((!string.IsNullOrEmpty(serverInformationFromSite.DomainName)) ? string.Format(CultureInfo.InvariantCulture, "{0}\\{1}$", serverInformationFromSite.DomainName, serverInformationFromSite.ComputerName) : string.Format(CultureInfo.InvariantCulture, "{0}$", serverInformationFromSite.ComputerName));
					serviceAccount = text2;
				}
			}
			catch (ArgumentException)
			{
				string[] array = serverInformationFromSite.ServiceAccount.Split('\\');
				if (array.Length == 2 && string.Compare(array[0], serverInformationFromSite.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					serviceAccount = array[1];
				}
			}
			site = new Site
			{
				Id = vmoClient.ManagementServer.Id.ToString().ToUpper(CultureInfo.InvariantCulture),
				Name = vmoClient.ManagementServer.Name,
				Description = vmoClient.ManagementServer.Description,
				Version = serverVersionFromSite.ToString(4),
				ServiceAccount = serviceAccount,
				SynchronizationStatus = 0
			};
			List<ServerAddress> list = new List<ServerAddress>();
			foreach (Address address in vmoClient.ManagementServer.Addresses)
			{
				list.Add(new ServerAddress(address.ServerUrl.OriginalString, (ServerAddressType)address.AddressType));
			}
			site.Addresses = list.ToArray();
			using ISiteComponent siteComponent3 = requestContext.CreateSiteComponent();
			siteComponent3.AttachMasterSite(site);
			return FindMasterSite(requestContext, siteComponent3);
		}
		finally
		{
			vmoClient?.Dispose();
		}
	}

	public static Site AttachSlaveSite(IGeminiRequestContext requestContext, string slaveSiteId, ServerAddress[] slaveSiteAddresses, string slaveSiteVersion)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "FederatedSites");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		IVmoClient vmoClient = null;
		IVmoClient vmoClient2 = null;
		Site site = null;
		try
		{
			Site site2 = null;
			using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
			{
				site2 = FindHomeSite(requestContext, siteComponent);
			}
			ServerVersion siteVersion = (string.IsNullOrEmpty(slaveSiteVersion) ? new ServerVersion() : new ServerVersion(slaveSiteVersion));
			RemoteSite remoteSite = new RemoteSite(slaveSiteId, slaveSiteAddresses, siteVersion);
			RemoteSite remoteSite2 = new RemoteSite(site2.Id, site2.Addresses, new ServerVersion(site2.Version));
			ISiteConnectionResult siteConnectionResult = ConnectToSite(site2, remoteSite);
			Uri serverAddress = siteConnectionResult.ServerAddress;
			vmoClient2 = siteConnectionResult.VmoClient;
			if (vmoClient2.ServerConnection.ServerVersion < new ServerVersion(8, 0, 0))
			{
				if (vmoClient2.ManagementServer.IsFeatureLicensed("PreventAddToFederatedHierarchy"))
				{
					throw new FeatureNotLicensedException();
				}
			}
			else if (!vmoClient2.ManagementServer.IsFeatureLicensed("AddToFederatedHierarchy"))
			{
				throw new FeatureNotLicensedException();
			}
			ISiteConnectionResult siteConnectionResult2 = ConnectToSite(site2, remoteSite2);
			Uri serverAddress2 = siteConnectionResult2.ServerAddress;
			vmoClient = siteConnectionResult2.VmoClient;
			if (SitesUtil.GetServerVersionFromSite(serverAddress2) == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new InvalidServerVersionException(text);
			}
			Version serverVersionFromSite = SitesUtil.GetServerVersionFromSite(serverAddress);
			if (serverVersionFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from slave site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new InvalidServerVersionException(text);
			}
			ServerInformationOld serverInformationFromSite = SitesUtil.GetServerInformationFromSite(serverAddress2);
			if (serverInformationFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new VmoUnauthorizedAccessException(text);
			}
			ServerInformationOld serverInformationFromSite2 = SitesUtil.GetServerInformationFromSite(serverAddress);
			if (serverInformationFromSite2 == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from slave site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new VmoUnauthorizedAccessException(text);
			}
			bool flag = false;
			SecurityIdentifier securityIdentifier = null;
			string text2 = null;
			string text3 = null;
			string text4 = serverInformationFromSite.ServiceAccount;
			try
			{
				securityIdentifier = new SecurityIdentifier(serverInformationFromSite.ServiceAccount);
				if (securityIdentifier.IsWellKnown(WellKnownSidType.NetworkServiceSid))
				{
					flag = true;
					text2 = ((!string.IsNullOrEmpty(serverInformationFromSite.DomainName)) ? string.Format(CultureInfo.InvariantCulture, "{0}\\{1}$", serverInformationFromSite.DomainName, serverInformationFromSite.ComputerName) : string.Format(CultureInfo.InvariantCulture, "{0}$", serverInformationFromSite.ComputerName));
					text4 = text2;
				}
			}
			catch (ArgumentException)
			{
				string[] array = serverInformationFromSite.ServiceAccount.Split('\\');
				if (array.Length == 2)
				{
					text3 = array[0];
					if (string.Equals(text3, serverInformationFromSite.ComputerName, StringComparison.OrdinalIgnoreCase))
					{
						text4 = array[1];
					}
				}
			}
			bool flag2 = false;
			SecurityIdentifier securityIdentifier2 = null;
			string text5 = null;
			string text6 = null;
			string text7 = serverInformationFromSite2.ServiceAccount;
			try
			{
				securityIdentifier2 = new SecurityIdentifier(serverInformationFromSite2.ServiceAccount);
				if (securityIdentifier2.IsWellKnown(WellKnownSidType.NetworkServiceSid))
				{
					flag2 = true;
					text5 = ((!string.IsNullOrEmpty(serverInformationFromSite2.DomainName)) ? string.Format(CultureInfo.InvariantCulture, "{0}\\{1}$", serverInformationFromSite2.DomainName, serverInformationFromSite2.ComputerName) : string.Format(CultureInfo.InvariantCulture, "{0}$", serverInformationFromSite2.ComputerName));
					text7 = text5;
				}
			}
			catch (ArgumentException)
			{
				string[] array2 = serverInformationFromSite2.ServiceAccount.Split('\\');
				if (array2.Length == 2)
				{
					text6 = array2[0];
					if (string.Equals(text6, serverInformationFromSite2.ComputerName, StringComparison.OrdinalIgnoreCase))
					{
						text7 = array2[1];
					}
				}
			}
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress2, text7))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave IIS service account '{0}' is not administrator on home site '{1}'", text7, serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                 slaveSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite2.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                 slaveSiteServerInformation.DomainName '{0}'", serverInformationFromSite2.DomainName);
				GeminiTrace.Warning(TraceType.General, "                 slaveSiteServerInformation.ComputerName '{0}'", serverInformationFromSite2.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                 slaveSiteIdent '{0}'", securityIdentifier2);
				GeminiTrace.Warning(TraceType.General, "                 slaveSiteMachineNSAccount '{0}'", text5);
				GeminiTrace.Warning(TraceType.General, "                 slaveServiceAccountDomain '{0}'", text6);
				GeminiTrace.Warning(TraceType.General, "                 isWellKnownSlaveAccount '{0}'", flag2);
				throw new VmoUnauthorizedAccessException(text);
			}
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress, text4))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Home IIS service account '{0}' is not administrator on slave site '{1}'", text4, serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                 homeSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                 homeSiteServerInformation.DomainName '{0}'", serverInformationFromSite.DomainName);
				GeminiTrace.Warning(TraceType.General, "                 homeSiteServerInformation.ComputerName '{0}'", serverInformationFromSite.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                 homeSiteIdent '{0}'", securityIdentifier);
				GeminiTrace.Warning(TraceType.General, "                 homeSiteMachineNSAccount '{0}'", text2);
				GeminiTrace.Warning(TraceType.General, "                 homeServiceAccountDomain '{0}'", text3);
				GeminiTrace.Warning(TraceType.General, "                 isWellKnownHomeAccount '{0}'", flag);
				throw new VmoUnauthorizedAccessException(text);
			}
			if (vmoClient2.ManagementServer.MasterSite != null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site already has the master site '{0}','{1}'", vmoClient2.ManagementServer.MasterSite.Id, vmoClient2.ManagementServer.MasterSite.Name);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new SiteRemoteSiteHasMasterException(text);
			}
			ChildSite childSite = null;
			try
			{
				childSite = vmoClient2.Repositories.GetObjectById<ChildSite>(new Guid(site2.Id), refreshCachedObjects: false);
			}
			catch (VideoOS.Management.VmoClient.SiteNotFoundException)
			{
			}
			if (childSite != null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}' already has the home site '{1}' in its hierarchy", vmoClient2.ManagementServer.Name, site2.Name);
				GeminiTrace.Warning(TraceType.General, "AttachSlaveSite: {0}", text);
				throw new SiteRemoteSiteHasSiteAsChildException(text);
			}
			site = new Site
			{
				Id = vmoClient2.ManagementServer.Id.ToString().ToUpper(CultureInfo.InvariantCulture),
				Name = vmoClient2.ManagementServer.Name,
				Description = vmoClient2.ManagementServer.Description,
				Version = serverVersionFromSite.ToString(4),
				ServiceAccount = text7,
				SynchronizationStatus = 0
			};
			List<ServerAddress> list = new List<ServerAddress>();
			foreach (Address address in vmoClient2.ManagementServer.Addresses)
			{
				list.Add(new ServerAddress(address.ServerUrl.OriginalString, (ServerAddressType)address.AddressType));
			}
			site.Addresses = list.ToArray();
			using (ISiteComponent siteComponent2 = requestContext.CreateSiteComponent())
			{
				siteComponent2.AttachSlaveSite(site);
				site = FindSite(requestContext, siteComponent2, site.Id);
			}
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogEntryAuditLogger.AuditSitesSlaveSiteCreated(vmoClient2.ManagementServer.Id, vmoClient2.ManagementServer.Name, serverAddress.OriginalString, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			Integration.FireConfigurationChanged(requestContext, "Site", vmoClient2.ManagementServer.Id.ToString(), EventAction.Added, vmoClient2.ManagementServer.Name);
			GeminiApplicationCore.SiteManager.RunNowSync(requestContext, site);
			return site;
		}
		finally
		{
			vmoClient?.Dispose();
			vmoClient2?.Dispose();
		}
	}

	private static ISiteConnectionResult ConnectToSite(Site homeSite, ISite remoteSite)
	{
		using SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
		ISiteConnectionResult siteConnection = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create().GetSiteConnection(new SiteConnectionInfoWindows(remoteSite, allowUnsecure: true, forceLegacy: true));
		if (siteConnection.IsFail)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to attach to site '{0}' from home site '{1}','{2}' using the target's configured addresses . Exception='{3}'", remoteSite.Id, homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
			GeminiTrace.Warning(TraceType.General, "ConnectToSite: {0}", text);
			if (siteConnection.HighPriorityException?.ExceptionFound != null)
			{
				throw siteConnection.HighPriorityException?.ExceptionFound;
			}
			throw new SiteToSiteConnectionFailureException(text);
		}
		return siteConnection;
	}

	public static Site UpdateHomeSite(IGeminiRequestContext requestContext, string name, string description, ServerAddress[] addresses, CustomProperty[] customProperties)
	{
		UpdateHomeSiteValidation(requestContext, name, description, addresses);
		Site site = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			siteComponent.UpdateHomeSite(name, description, addresses, null, null, null, customProperties);
			site = FindHomeSite(requestContext, siteComponent);
			Site masterSite = FindMasterSite(requestContext, siteComponent);
			GeminiApplicationCore.SiteManager.NotifyMaster(requestContext, masterSite, site);
		}
		if (string.Compare(name, site.Name, StringComparison.Ordinal) != 0)
		{
			using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
			vmoLogManager.LogEntryAuditLogger.AuditSitesRenamed(new Guid(site.Id), name, site.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "System", site.Id, EventAction.Updated, site.Name);
		return site;
	}

	public static void UpdateHomeSiteValidation(IGeminiRequestContext requestContext, string name, string description, ServerAddress[] addresses)
	{
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		Validation.CheckServerAddressArray(addresses, "addresses", allowNullOrEmpty: true);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
	}

	public static void DeleteSite(IGeminiRequestContext requestContext, string siteId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		Validation.CheckObjectId(ref siteId, "siteId", allowNullOrEmpty: false);
		Site site = null;
		Site site2 = null;
		Site site3 = null;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			site = FindMasterSite(requestContext, siteComponent);
			site2 = FindHomeSite(requestContext, siteComponent);
			site3 = FindSite(requestContext, siteComponent, siteId);
			siteComponent.DeleteSite(siteId);
			GeminiApplicationCore.SiteManager.NotifyMaster(requestContext, site, site2);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			if (site != null && siteId == site.Id)
			{
				vmoLogManager.LogEntryAuditLogger.AuditSitesMasterSiteDeleted(new Guid(siteId), site3.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			else
			{
				vmoLogManager.LogEntryAuditLogger.AuditSitesSlaveSiteDeleted(new Guid(siteId), site3.Name, GetServerAddress(site3.Addresses).URL, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
		}
		Integration.FireConfigurationChanged(requestContext, "Site", siteId, EventAction.Deleted, site3.Name);
	}

	public static void MarkInconsistent(IGeminiRequestContext requestContext, string siteId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		Validation.CheckObjectId(ref siteId, "siteId", allowNullOrEmpty: false);
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		siteComponent.MarkInconsistent(siteId);
	}

	public static Site AuthorizeMaster(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		Site site;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			site = FindMasterSite(requestContext, siteComponent);
			Site homeSite = FindHomeSite(requestContext, siteComponent);
			SitesUtil.CheckIfMasterSiteHasHomeSiteAsSlave(site, homeSite);
			siteComponent.AuthorizeMaster();
			GeminiApplicationCore.SiteManager.NotifyMaster(requestContext, site, homeSite);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditSitesMasterSiteAuthorized(new Guid(site.Id), site.Name, GetServerAddress(site.Addresses).URL, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return site;
	}

	public static void SyncronizeSite(IGeminiRequestContext requestContext, string siteIdStr)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_FEDERATED_HIERARCHY");
		if (Guid.TryParse(siteIdStr, out var _))
		{
			Site site = FindSite(requestContext, null, siteIdStr);
			if (site != null)
			{
				GeminiApplicationCore.SiteManager.RunNow(requestContext, site);
			}
		}
	}

	public static Site FindSite(IGeminiRequestContext requestContext, ISiteComponent siteComponent, string siteId)
	{
		bool flag = false;
		try
		{
			if (siteComponent == null)
			{
				siteComponent = requestContext.CreateSiteComponent();
				flag = true;
			}
			return siteComponent.QuerySite(siteId);
		}
		catch
		{
			return null;
		}
		finally
		{
			if (flag)
			{
				siteComponent.Dispose();
			}
		}
	}

	public static Site FindHomeSite(IGeminiRequestContext requestContext, ISiteComponent siteComponent)
	{
		bool flag = false;
		try
		{
			if (siteComponent == null)
			{
				siteComponent = requestContext.CreateSiteComponent();
				flag = true;
			}
			return siteComponent.QueryHomeSite();
		}
		finally
		{
			if (flag)
			{
				siteComponent.Dispose();
			}
		}
	}

	public static Site FindMasterSite(IGeminiRequestContext requestContext, ISiteComponent siteComponent)
	{
		bool flag = false;
		try
		{
			if (siteComponent == null)
			{
				siteComponent = requestContext.CreateSiteComponent();
				flag = true;
			}
			return siteComponent.QueryMasterSite();
		}
		finally
		{
			if (flag)
			{
				siteComponent.Dispose();
			}
		}
	}

	private static ServerAddress GetServerAddress(ServerAddress[] addresses)
	{
		if (addresses == null || addresses.Length == 0)
		{
			return null;
		}
		foreach (ServerAddress serverAddress in addresses)
		{
			if (serverAddress.Type == ServerAddressType.Server)
			{
				return serverAddress;
			}
		}
		return addresses[0];
	}

	public static void TriggerFullSiteSynchronization(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		GeminiApplicationCore.SiteManager.SynchronizeAllSites(requestContext, waitForComplete: false);
	}

	private static Site[] FilterSitesBasedOnPermissions(IGeminiRequestContext requestContext, Site[] sitesArray)
	{
		List<Site> list = new List<Site>();
		foreach (Site site in sitesArray)
		{
			IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetSecurityManagerService().GetSecurityNamespaceFromToken(requestContext, site.Id);
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "GENERIC_READ", site.Id))
			{
				list.Add(site);
			}
		}
		return list.ToArray();
	}

	private static string[] FilterSiteIdsBasedOnPermissions(IGeminiRequestContext requestContext, string[] siteIdsArray)
	{
		List<string> list = new List<string>();
		foreach (string text in siteIdsArray)
		{
			IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetSecurityManagerService().GetSecurityNamespaceFromToken(requestContext, text);
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "GENERIC_READ", text))
			{
				list.Add(text);
			}
		}
		return list.ToArray();
	}
}
