using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public class SiteSynchronizationManager
{
	public static void SynchronizeHomeSite(IGeminiRequestContext requestContext, Site homeSite)
	{
		if (homeSite == null || homeSite.SiteType != SiteType.Home)
		{
			return;
		}
		IVmoClient vmoClient = null;
		SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
		try
		{
			RemoteSite site = new RemoteSite(homeSite.Id, homeSite.Addresses, new ServerVersion(homeSite.Version));
			ISiteConnector siteConnector = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create();
			homeSite.LastStatusHandshake = DateTime.UtcNow;
			homeSite.SynchronizationStatus = 0;
			ISiteConnectionResult siteConnection = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
			if (siteConnection.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to home site '{0}','{1}' from the home site ifself using the addresses configured for the home site. Exception='{2}'", homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "SynchronizeHomeSite: {0}", text);
				homeSite.SynchronizationStatus = 101;
				UpdateSiteHandshakeAndSyncStatus(requestContext, homeSite);
				if (siteConnection.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection.HighPriorityException.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			Uri serverAddress = siteConnection.ServerAddress;
			vmoClient = siteConnection.VmoClient;
			Version serverVersionFromSite = SitesUtil.GetServerVersionFromSite(serverAddress);
			if (serverVersionFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from home site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeHomeSite: {0}", text);
				homeSite.SynchronizationStatus = 102;
				UpdateSiteHandshakeAndSyncStatus(requestContext, homeSite);
				throw new InvalidServerVersionException(text);
			}
			homeSite.Version = serverVersionFromSite.ToString(4);
			UpdateSiteVersion(requestContext, homeSite);
			ServerInformationOld serverInformationFromSite = SitesUtil.GetServerInformationFromSite(serverAddress);
			if (serverInformationFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from home site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeHomeSite: {0}", text);
				homeSite.SynchronizationStatus = 103;
				UpdateSiteHandshakeAndSyncStatus(requestContext, homeSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			string text2 = null;
			string serviceAccount = serverInformationFromSite.ServiceAccount;
			try
			{
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(serverInformationFromSite.ServiceAccount);
				homeSite.ServiceAccount = serverInformationFromSite.ServiceAccount;
				if (securityIdentifier.IsWellKnown(WellKnownSidType.NetworkServiceSid))
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
				homeSite.ServiceAccount = serviceAccount;
			}
			UpdateSiteServiceAccount(requestContext, homeSite);
			using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
			siteComponent.UpdateHomeSite(homeSite.Name, homeSite.Description, homeSite.Addresses, homeSite.Version, homeSite.ServiceAccount, homeSite.SynchronizationStatus, homeSite.CustomProperties);
		}
		finally
		{
			vmoClient?.Dispose();
			siteTokenCacheStore.Dispose();
		}
	}

	public static void SynchronizeSlaveSite(IGeminiRequestContext requestContext, Site slaveSite, Site homeSite)
	{
		if (slaveSite == null || homeSite == null || slaveSite.MasterSiteId != homeSite.Id || slaveSite.SiteType != SiteType.Slave)
		{
			return;
		}
		IVmoClient vmoClient = null;
		IVmoClient vmoClient2 = null;
		SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
		try
		{
			RemoteSite site = new RemoteSite(homeSite.Id, homeSite.Addresses, new ServerVersion(homeSite.Version));
			RemoteSite site2 = new RemoteSite(slaveSite.Id, slaveSite.Addresses, new ServerVersion(slaveSite.Version));
			ISiteConnector siteConnector = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create();
			slaveSite.LastStatusHandshake = DateTime.UtcNow;
			slaveSite.SynchronizationStatus = 0;
			ISiteConnectionResult siteConnection = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site2, allowUnsecure: true, forceLegacy: true));
			if (siteConnection.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to slave site '{0}','{1}' from home site '{2}','{3}'using the addresses configured for the slave site. Exception='{4}'", slaveSite.Id, slaveSite.Name, homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 201;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				if (siteConnection.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection.HighPriorityException.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			Uri serverAddress = siteConnection.ServerAddress;
			vmoClient2 = siteConnection.VmoClient;
			ISiteConnectionResult siteConnection2 = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
			if (siteConnection2.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to home site '{0}','{1}' from the home site ifself using the addresses configured for the home site. Exception='{2}'", homeSite.Id, homeSite.Name, siteConnection2.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection2.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 202;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				if (siteConnection2.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection2.HighPriorityException.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			Uri serverAddress2 = siteConnection2.ServerAddress;
			vmoClient = siteConnection2.VmoClient;
			if (SitesUtil.GetServerVersionFromSite(serverAddress2) == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 203;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				throw new InvalidServerVersionException(text);
			}
			Version serverVersionFromSite = SitesUtil.GetServerVersionFromSite(serverAddress);
			if (serverVersionFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from slave site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 204;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				throw new InvalidServerVersionException(text);
			}
			slaveSite.Version = serverVersionFromSite.ToString(4);
			UpdateSiteVersion(requestContext, slaveSite);
			ServerInformationOld serverInformationFromSite = SitesUtil.GetServerInformationFromSite(serverAddress2);
			if (serverInformationFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 206;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			ServerInformationOld serverInformationFromSite2 = SitesUtil.GetServerInformationFromSite(serverAddress);
			if (serverInformationFromSite2 == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from slave site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				slaveSite.SynchronizationStatus = 207;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
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
					if (string.Compare(text3, serverInformationFromSite.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
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
				slaveSite.ServiceAccount = serverInformationFromSite2.ServiceAccount;
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
					if (string.Compare(text6, serverInformationFromSite2.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						text7 = array2[1];
					}
				}
				slaveSite.ServiceAccount = text7;
			}
			UpdateSiteServiceAccount(requestContext, slaveSite);
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress2, text7))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave IIS service account '{0}' is not administrator on home site '{1}'", text7, serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                      slaveSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite2.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                      slaveSiteServerInformation.DomainName '{0}'", serverInformationFromSite2.DomainName);
				GeminiTrace.Warning(TraceType.General, "                      slaveSiteServerInformation.ComputerName '{0}'", serverInformationFromSite2.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                      slaveSiteIdent '{0}'", securityIdentifier2);
				GeminiTrace.Warning(TraceType.General, "                      slaveSiteMachineNSAccount '{0}'", text5);
				GeminiTrace.Warning(TraceType.General, "                      slaveServiceAccountDomain '{0}'", text6);
				GeminiTrace.Warning(TraceType.General, "                      isWellKnownSlaveAccount '{0}'", flag2);
				slaveSite.SynchronizationStatus = 208;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress, text4))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Home IIS service account '{0}' is not administrator on slave site '{1}'", text4, serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                      homeSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                      homeSiteServerInformation.DomainName '{0}'", serverInformationFromSite.DomainName);
				GeminiTrace.Warning(TraceType.General, "                      homeSiteServerInformation.ComputerName '{0}'", serverInformationFromSite.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                      homeSiteIdent '{0}'", securityIdentifier);
				GeminiTrace.Warning(TraceType.General, "                      homeSiteMachineNSAccount '{0}'", text2);
				GeminiTrace.Warning(TraceType.General, "                      homeServiceAccountDomain '{0}'", text3);
				GeminiTrace.Warning(TraceType.General, "                      isWellKnownHomeAccount '{0}'", flag);
				slaveSite.SynchronizationStatus = 209;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			if (vmoClient2.ManagementServer.MasterSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}' does not have home site '{1}' as master, slave ConnectionState='{2}'. Creating home site on slave", slaveSite.Name, homeSite.Name, slaveSite.ConnectionState);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				MasterNotExistOnRemoteServer(requestContext, slaveSite, homeSite, vmoClient2);
				slaveSite.SynchronizationStatus = 210;
				UpdateSiteHandshakeAndSyncStatus(requestContext, slaveSite);
				return;
			}
			if (string.Compare(vmoClient2.ManagementServer.MasterSite.Id.ToString(), homeSite.Id, StringComparison.OrdinalIgnoreCase) != 0)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}' does have another site '{1}','{2}' as master than the home site '{3}','{4}'. Slave site is deleted.", slaveSite.Name, vmoClient2.ManagementServer.MasterSite.Id, vmoClient2.ManagementServer.MasterSite.Name, homeSite.Id, homeSite.Name);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
				siteComponent.DeleteSite(slaveSite.Id);
				return;
			}
			switch (slaveSite.ConnectionState)
			{
			case SiteState.Attached:
			{
				if (vmoClient2.ManagementServer.MasterSite.ConnectionState != ConnectionState.Unauthorized)
				{
					SyncSlaveSites(requestContext, slaveSite, vmoClient2);
					break;
				}
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}' is deleted due to invalid master ConnectionState. Slave site ConnectionState='{1}', MasterSite ConnectionState='{2}'", slaveSite.Name, slaveSite.ConnectionState, vmoClient2.ManagementServer.MasterSite.ConnectionState);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				using ISiteComponent siteComponent6 = requestContext.CreateSiteComponent();
				siteComponent6.DeleteSite(slaveSite.Id);
				break;
			}
			case SiteState.AttachPending:
			{
				if (vmoClient2.ManagementServer.MasterSite.ConnectionState == ConnectionState.Unauthorized)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}':  Correcting master site '{1}' ConnectionState from Unauthorized to WaitRemoteAuthorize", slaveSite.Name, vmoClient2.ManagementServer.MasterSite.Name);
					GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
					using ISiteComponent siteComponent4 = requestContext.CreateSiteComponent();
					siteComponent4.UpdateSiteState(slaveSite.Id, SiteState.WaitRemoteAuthorize);
					break;
				}
				SyncSlaveSites(requestContext, slaveSite, vmoClient2);
				using ISiteComponent siteComponent5 = requestContext.CreateSiteComponent();
				siteComponent5.UpdateSiteState(slaveSite.Id, SiteState.Attached);
				break;
			}
			case SiteState.Unauthorized:
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Slave site '{0}' is deleted due to invalid slave site ConnectionState=Unauthorized.", slaveSite.Name);
				GeminiTrace.Warning(TraceType.General, "SynchronizeSlaveSite: {0}", text);
				using ISiteComponent siteComponent7 = requestContext.CreateSiteComponent();
				siteComponent7.DeleteSite(slaveSite.Id);
				break;
			}
			case SiteState.WaitRemoteAuthorize:
			{
				if (vmoClient2.ManagementServer.MasterSite.ConnectionState != ConnectionState.Unauthorized)
				{
					SyncSlaveSites(requestContext, slaveSite, vmoClient2);
					using ISiteComponent siteComponent2 = requestContext.CreateSiteComponent();
					siteComponent2.UpdateSiteState(slaveSite.Id, SiteState.Attached);
					break;
				}
				using ISiteComponent siteComponent3 = requestContext.CreateSiteComponent();
				siteComponent3.UpdateSiteState(slaveSite.Id, SiteState.WaitRemoteAuthorize);
				break;
			}
			}
		}
		finally
		{
			vmoClient?.Dispose();
			vmoClient2?.Dispose();
			siteTokenCacheStore.Dispose();
		}
	}

	private static void MasterNotExistOnRemoteServer(IGeminiRequestContext requestContext, Site site, Site homeSite, IVmoClient remoteSite)
	{
		switch (site.ConnectionState)
		{
		case SiteState.AttachPending:
		{
			AddressCollection addressCollection = new AddressCollection();
			ServerAddress[] addresses = homeSite.Addresses;
			foreach (ServerAddress serverAddress in addresses)
			{
				addressCollection.Add(new Address(new Uri(serverAddress.URL), (AddressType)serverAddress.Type));
			}
			try
			{
				remoteSite.ManagementServer.AttachMasterSite(new Guid(homeSite.Id), addressCollection, homeSite.Version);
			}
			catch (VideoOS.Management.VmoClient.FeatureNotLicensedException)
			{
				GeminiTrace.Warning(TraceType.General, "Attach Master Site on remote site: {0}. Server is not licensed to have a master. Deleting site from system.", remoteSite.ManagementServer.Name);
				using ISiteComponent siteComponent2 = requestContext.CreateSiteComponent();
				siteComponent2.DeleteSite(site.Id);
				break;
			}
			using ISiteComponent siteComponent3 = requestContext.CreateSiteComponent();
			siteComponent3.UpdateSiteState(site.Id, SiteState.WaitRemoteAuthorize);
			break;
		}
		case SiteState.Attached:
		case SiteState.Unauthorized:
		case SiteState.WaitRemoteAuthorize:
		{
			using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
			siteComponent.DeleteSite(site.Id);
			break;
		}
		}
	}

	private static void SyncSlaveSites(IGeminiRequestContext requestContext, Site slaveSite, IVmoClient remoteSite)
	{
		if (!IsInSync(requestContext, slaveSite, remoteSite))
		{
			using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
			siteComponent.SyncronizeSlaveTree(slaveSite, remoteSite);
		}
		using ISiteComponent siteComponent2 = requestContext.CreateSiteComponent();
		siteComponent2.UpdateSiteHandshakeAndSyncStatus(slaveSite);
	}

	private static bool IsInSync(IGeminiRequestContext requestContext, Site localSlaveSite, IVmoClient remoteSite)
	{
		Dictionary<string, Site> dictionary = new Dictionary<string, Site>();
		Site[] array;
		using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
		{
			array = siteComponent.QuerySlaveSitesComplete(localSlaveSite.Id);
		}
		Site[] array2 = array;
		foreach (Site site in array2)
		{
			dictionary.Add(site.Id.ToUpper(CultureInfo.InvariantCulture), site);
		}
		return ValidateSites(remoteSite, dictionary);
	}

	private static bool ValidateSites(IVmoClient remoteSite, Dictionary<string, Site> sitesCol)
	{
		if (!sitesCol.TryGetValue(remoteSite.ManagementServer.Id.ToString().ToUpper(CultureInfo.InvariantCulture), out var value))
		{
			return false;
		}
		int num = 0;
		DateTime dateTime = new DateTime(remoteSite.ManagementServer.LastModified.Year, remoteSite.ManagementServer.LastModified.Month, remoteSite.ManagementServer.LastModified.Day, remoteSite.ManagementServer.LastModified.Hour, remoteSite.ManagementServer.LastModified.Minute, remoteSite.ManagementServer.LastModified.Second);
		DateTime value2 = new DateTime(value.LastModified.Year, value.LastModified.Month, value.LastModified.Day, value.LastModified.Hour, value.LastModified.Minute, value.LastModified.Second);
		if (dateTime.CompareTo(value2) != 0)
		{
			return false;
		}
		num++;
		if (!ValidateSlaveSites(remoteSite.ManagementServer.ChildSites, sitesCol, ref num))
		{
			return false;
		}
		return num == sitesCol.Count;
	}

	private static bool ValidateSlaveSites(ChildSiteCollection remoteSlaveSites, Dictionary<string, Site> sitesCol, ref int validatedSites)
	{
		if (remoteSlaveSites == null)
		{
			return true;
		}
		foreach (ChildSite remoteSlaveSite in remoteSlaveSites)
		{
			if (!sitesCol.TryGetValue(remoteSlaveSite.Id.ToString().ToUpper(CultureInfo.InvariantCulture), out var value))
			{
				return false;
			}
			if (!IsSiteInSync(remoteSlaveSite, value))
			{
				return false;
			}
			if (!ValidateSlaveSites(remoteSlaveSite.ChildSites, sitesCol, ref validatedSites))
			{
				return false;
			}
			validatedSites++;
		}
		return true;
	}

	private static bool IsSiteInSync(ChildSite remoteChildSite, Site localSite)
	{
		DateTime dateTime = new DateTime(remoteChildSite.LastModified.Year, remoteChildSite.LastModified.Month, remoteChildSite.LastModified.Day, remoteChildSite.LastModified.Hour, remoteChildSite.LastModified.Minute, remoteChildSite.LastModified.Second);
		DateTime value = new DateTime(localSite.LastModified.Year, localSite.LastModified.Month, localSite.LastModified.Day, localSite.LastModified.Hour, localSite.LastModified.Minute, localSite.LastModified.Second);
		string text = null;
		text = ((remoteChildSite.ParentSite == null) ? remoteChildSite.ManagementServer.Id.ToString() : remoteChildSite.ParentSite.Id.ToString());
		if (remoteChildSite == null || localSite == null || string.Compare(remoteChildSite.Id.ToString(), localSite.Id, StringComparison.OrdinalIgnoreCase) != 0 || dateTime.CompareTo(value) != 0 || string.Compare(text, localSite.MasterSiteId, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(remoteChildSite.Version.ToString(4), localSite.Version, StringComparison.OrdinalIgnoreCase) != 0 || string.Compare(remoteChildSite.ServiceAccount, localSite.ServiceAccount, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return false;
		}
		return true;
	}

	private static void UpdateSiteVersion(IGeminiRequestContext requestContext, Site site)
	{
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		siteComponent.UpdateSiteVersion(site);
	}

	private static void UpdateSiteServiceAccount(IGeminiRequestContext requestContext, Site site)
	{
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		siteComponent.UpdateSiteServiceAccount(site);
	}

	private static void UpdateSiteHandshakeAndSyncStatus(IGeminiRequestContext requestContext, Site site)
	{
		using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
		siteComponent.UpdateSiteHandshakeAndSyncStatus(site);
	}

	public static void SynchronizeMasterSite(IGeminiRequestContext requestContext, Site masterSite, Site homeSite)
	{
		if (masterSite == null || homeSite == null || homeSite.MasterSiteId != masterSite.Id || masterSite.SiteType != SiteType.Master)
		{
			return;
		}
		IVmoClient vmoClient = null;
		IVmoClient vmoClient2 = null;
		SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
		try
		{
			RemoteSite site = new RemoteSite(homeSite.Id, homeSite.Addresses, new ServerVersion(homeSite.Version));
			RemoteSite site2 = new RemoteSite(masterSite.Id, masterSite.Addresses, new ServerVersion(masterSite.Version));
			ISiteConnector siteConnector = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create();
			masterSite.LastStatusHandshake = DateTime.UtcNow;
			masterSite.SynchronizationStatus = 0;
			ISiteConnectionResult siteConnection = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site2, allowUnsecure: true, forceLegacy: true));
			if (siteConnection.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to master site '{0}','{1}' from home site '{2}','{3}'using the addresses configured for the master site. Exception='{4}'", masterSite.Id, masterSite.Name, homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 301;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				if (siteConnection.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection.HighPriorityException.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			Uri serverAddress = siteConnection.ServerAddress;
			vmoClient2 = siteConnection.VmoClient;
			ISiteConnectionResult siteConnection2 = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
			if (siteConnection2.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to home site '{0}','{1}' from the home site ifself using the addresses configured for the home site. Exception='{2}'", homeSite.Id, homeSite.Name, siteConnection2.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection2.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 302;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				if (siteConnection2.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection2.HighPriorityException.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			Uri serverAddress2 = siteConnection2.ServerAddress;
			vmoClient = siteConnection2.VmoClient;
			if (SitesUtil.GetServerVersionFromSite(serverAddress2) == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 303;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				throw new InvalidServerVersionException(string.Format(CultureInfo.InvariantCulture, "Cannot get server version from home site '{0}'", serverAddress2.OriginalString));
			}
			Version serverVersionFromSite = SitesUtil.GetServerVersionFromSite(serverAddress);
			if (serverVersionFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server version from master site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 304;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				throw new InvalidServerVersionException(text);
			}
			masterSite.Version = serverVersionFromSite.ToString(4);
			UpdateSiteVersion(requestContext, masterSite);
			ServerInformationOld serverInformationFromSite = SitesUtil.GetServerInformationFromSite(serverAddress2);
			if (serverInformationFromSite == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from home site '{0}'", serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 306;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			ServerInformationOld serverInformationFromSite2 = SitesUtil.GetServerInformationFromSite(serverAddress);
			if (serverInformationFromSite2 == null)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Cannot get server information from master site '{0}'", serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				masterSite.SynchronizationStatus = 307;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
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
					if (string.Compare(text3, serverInformationFromSite.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
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
				masterSite.ServiceAccount = serverInformationFromSite2.ServiceAccount;
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
					if (string.Compare(text6, serverInformationFromSite2.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						text7 = array2[1];
					}
				}
				masterSite.ServiceAccount = text7;
			}
			UpdateSiteServiceAccount(requestContext, masterSite);
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress2, text7))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Master IIS service account '{0}' is not administrator on home site '{1}'", text7, serverAddress2.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                       masterSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite2.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                       masterSiteServerInformation.DomainName '{0}'", serverInformationFromSite2.DomainName);
				GeminiTrace.Warning(TraceType.General, "                       masterSiteServerInformation.ComputerName '{0}'", serverInformationFromSite2.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                       masterSiteIdent '{0}'", securityIdentifier2);
				GeminiTrace.Warning(TraceType.General, "                       masterSiteMachineNSAccount '{0}'", text5);
				GeminiTrace.Warning(TraceType.General, "                       masterServiceAccountDomain '{0}'", text6);
				GeminiTrace.Warning(TraceType.General, "                       isWellKnownMasterAccount '{0}'", flag2);
				masterSite.SynchronizationStatus = 308;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			if (!SitesUtil.IsIISServerAccountAdminOnSite(serverAddress, text4))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Home IIS service account '{0}' is not administrator on master site '{1}'", text4, serverAddress.OriginalString);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				GeminiTrace.Warning(TraceType.General, "                       homeSiteServerInformation.ServiceAccount '{0}'", serverInformationFromSite.ServiceAccount);
				GeminiTrace.Warning(TraceType.General, "                       homeSiteServerInformation.DomainName '{0}'", serverInformationFromSite.DomainName);
				GeminiTrace.Warning(TraceType.General, "                       homeSiteServerInformation.ComputerName '{0}'", serverInformationFromSite.ComputerName);
				GeminiTrace.Warning(TraceType.General, "                       homeSiteIdent '{0}'", securityIdentifier);
				GeminiTrace.Warning(TraceType.General, "                       homeSiteMachineNSAccount '{0}'", text2);
				GeminiTrace.Warning(TraceType.General, "                       homeServiceAccountDomain '{0}'", text3);
				GeminiTrace.Warning(TraceType.General, "                       isWellKnownHomeAccount '{0}'", flag);
				masterSite.SynchronizationStatus = 309;
				UpdateSiteHandshakeAndSyncStatus(requestContext, masterSite);
				throw new VmoUnauthorizedAccessException(text);
			}
			bool flag3 = false;
			if (vmoClient2.ManagementServer.ChildSites[new Guid(homeSite.Id)] != null)
			{
				flag3 = true;
			}
			if (!flag3)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Master site '{0}','{1}' does not have home site '{2}','{3}' as slave. Master site is deleted.", masterSite.Id, masterSite.Name, homeSite.Id, homeSite.Name);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				using ISiteComponent siteComponent = requestContext.CreateSiteComponent();
				siteComponent.DeleteSite(masterSite.Id);
				return;
			}
			switch (masterSite.ConnectionState)
			{
			case SiteState.Attached:
			case SiteState.Unauthorized:
			{
				using ISiteComponent siteComponent3 = requestContext.CreateSiteComponent();
				Site site3 = SiteComponent.SerializeVmoClientSite(vmoClient2.ManagementServer, (int)masterSite.ConnectionState, masterSite.MasterSiteId, masterSite.LastStatusHandshake, 2, masterSite.Version, masterSite.ServiceAccount, masterSite.SynchronizationStatus);
				siteComponent3.SyncSite(site3);
				break;
			}
			case SiteState.AttachPending:
			case SiteState.WaitRemoteAuthorize:
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Master site '{0}' is deleted due to invalid ConnectionState='{1}'.", masterSite.Name, masterSite.ConnectionState);
				GeminiTrace.Warning(TraceType.General, "SynchronizeMasterSite: {0}", text);
				using ISiteComponent siteComponent2 = requestContext.CreateSiteComponent();
				siteComponent2.DeleteSite(masterSite.Id);
				break;
			}
			}
		}
		finally
		{
			vmoClient2?.Dispose();
			vmoClient?.Dispose();
			siteTokenCacheStore.Dispose();
		}
	}

	public static void StartSynchronizeOnMasterSite(Site masterSite, Site homeSite)
	{
		if (masterSite == null || homeSite == null || homeSite.MasterSiteId != masterSite.Id || masterSite.SiteType != SiteType.Master || (masterSite.ConnectionState != SiteState.Unauthorized && masterSite.ConnectionState != SiteState.Attached))
		{
			return;
		}
		IVmoClient vmoClient = null;
		try
		{
			RemoteSite site = new RemoteSite(masterSite.Id, masterSite.Addresses, new ServerVersion(masterSite.Version));
			using SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
			ISiteConnectionResult siteConnection = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create().GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
			if (siteConnection.IsFail)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "Server-to-Server connection error. Failed to connect to master site '{0}','{1}' from home site '{2}','{3}'using the addresses configured for the master site. Exception='{4}'", masterSite.Id, masterSite.Name, homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
				GeminiTrace.Warning(TraceType.General, "StartSynchronizeOnMasterSite: {0}", text);
				if (siteConnection.HighPriorityException?.ExceptionFound != null)
				{
					throw siteConnection.HighPriorityException?.ExceptionFound;
				}
				throw new SiteToSiteConnectionFailureException(text);
			}
			vmoClient = siteConnection.VmoClient;
			bool flag = false;
			ChildSite childSite = vmoClient.ManagementServer.ChildSites[new Guid(homeSite.Id)];
			if (childSite != null && ((childSite.ConnectionState == ConnectionState.Attached || childSite.ConnectionState == ConnectionState.WaitRemoteAuthorize) ? true : false))
			{
				childSite.Synchronize();
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			vmoClient?.Dispose();
		}
	}
}
