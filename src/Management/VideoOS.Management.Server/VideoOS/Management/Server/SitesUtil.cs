using System;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public sealed class SitesUtil
{
	public static Version GetServerVersionFromSite(Uri siteUri)
	{
		Version result = null;
		ChannelFactory<IServerService> channelFactory = null;
		try
		{
			if (siteUri == null)
			{
				return result;
			}
			WSHttpBinding wSHttpBinding = new WSHttpBinding();
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			NetworkCredential clientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			Uri uri = new UriBuilder(new Uri(siteUri, "ManagementServer/ServerService.svc").OriginalString).Uri;
			Uri uri2 = new Uri("urn:videoos:management:server:serverservice");
			string spn = SpnFactory.GetSpn(uri);
			Exception ex = null;
			try
			{
				EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));
				channelFactory = new ChannelFactory<IServerService>(wSHttpBinding, remoteAddress);
				channelFactory.Credentials.Windows.ClientCredential = clientCredential;
				channelFactory.Credentials.SupportInteractive = false;
				result = channelFactory.CreateChannel().GetServerVersion();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex is EndpointNotFoundException)
			{
				ex = null;
				try
				{
					EndpointAddress remoteAddress2 = new EndpointAddress(uri2, EndpointIdentity.CreateSpnIdentity(spn));
					channelFactory = new ChannelFactory<IServerService>(wSHttpBinding, remoteAddress2);
					channelFactory.Endpoint.Behaviors.Add(new ClientViaBehavior(uri));
					channelFactory.Credentials.Windows.ClientCredential = clientCredential;
					channelFactory.Credentials.SupportInteractive = false;
					result = channelFactory.CreateChannel().GetServerVersion();
				}
				catch (Exception)
				{
				}
			}
		}
		finally
		{
			if (channelFactory != null)
			{
				try
				{
					channelFactory.Close();
				}
				catch (Exception)
				{
					channelFactory.Abort();
				}
			}
		}
		return result;
	}

	public static ServerInformationOld GetServerInformationFromSite(Uri siteUri)
	{
		ServerInformationOld result = null;
		ChannelFactory<IServerService> channelFactory = null;
		try
		{
			if (siteUri == null)
			{
				return result;
			}
			WSHttpBinding wSHttpBinding = new WSHttpBinding();
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			NetworkCredential clientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			Uri uri = new UriBuilder(new Uri(siteUri, "ManagementServer/ServerService.svc").OriginalString).Uri;
			Uri uri2 = new Uri("urn:videoos:management:server:serverservice");
			string spn = SpnFactory.GetSpn(uri);
			Exception ex = null;
			try
			{
				EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));
				channelFactory = new ChannelFactory<IServerService>(wSHttpBinding, remoteAddress);
				channelFactory.Credentials.Windows.ClientCredential = clientCredential;
				channelFactory.Credentials.SupportInteractive = false;
				result = channelFactory.CreateChannel().GetServerInformation();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex is EndpointNotFoundException)
			{
				ex = null;
				try
				{
					EndpointAddress remoteAddress2 = new EndpointAddress(uri2, EndpointIdentity.CreateSpnIdentity(spn));
					channelFactory = new ChannelFactory<IServerService>(wSHttpBinding, remoteAddress2);
					channelFactory.Endpoint.Behaviors.Add(new ClientViaBehavior(uri));
					channelFactory.Credentials.Windows.ClientCredential = clientCredential;
					channelFactory.Credentials.SupportInteractive = false;
					result = channelFactory.CreateChannel().GetServerInformation();
				}
				catch (Exception)
				{
				}
			}
		}
		finally
		{
			if (channelFactory != null)
			{
				try
				{
					channelFactory.Close();
				}
				catch (Exception)
				{
					channelFactory.Abort();
				}
			}
		}
		return result;
	}

	private static bool CheckIfIdentitiesAreAdmin(Identity[] identities)
	{
		bool result = false;
		foreach (Identity identity in identities)
		{
			if (identity == null)
			{
				continue;
			}
			string[] memberOf = identity.MemberOf;
			foreach (string text in memberOf)
			{
				if (Guid.TryParse(text, out var result2))
				{
					if (result2 == SecurityManager.AdministratorRoleId)
					{
						result = true;
						break;
					}
					continue;
				}
				try
				{
					if (new SecurityIdentifier(text).IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
					{
						result = true;
						break;
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		return result;
	}

	public static void CheckIfMasterSiteHasHomeSiteAsSlave(Site masterSite, Site homeSite)
	{
		IVmoClient vmoClient = null;
		try
		{
			string empty = string.Empty;
			RemoteSite site = new RemoteSite(masterSite.Id, masterSite.Addresses, new ServerVersion(masterSite.Version));
			using SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
			ISiteConnectionResult siteConnection = ((ISiteConnectorFactory)new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"))).Create().GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
			if (siteConnection.IsFail)
			{
				empty = string.Format(CultureInfo.InvariantCulture, "CheckIfMasterHasHomeSite: Server-to-Server connection error. Failed to connect to master site '{0}','{1}' from home site '{2}','{3}' using the addresses configured for the master site. Exception='{4}'", masterSite.Id, masterSite.Name, homeSite.Id, homeSite.Name, siteConnection.HighPriorityException?.ExceptionFound?.ToString() ?? siteConnection.FormatExceptions());
				if (siteConnection.HighPriorityException?.ExceptionFound != null)
				{
					throw new SiteToSiteConnectionFailureException(empty, siteConnection.HighPriorityException.ExceptionFound);
				}
				throw new SiteToSiteConnectionFailureException(empty);
			}
			vmoClient = siteConnection.VmoClient;
			if (vmoClient.ManagementServer.ChildSites[new Guid(homeSite.Id)] == null)
			{
				empty = string.Format(CultureInfo.InvariantCulture, "CheckIfMasterHasHomeSite: Master site '{0}' does not have home site '{1}' as its slave site", masterSite.Name, homeSite.Name);
				throw new SiteMasterSiteDoesNotHaveHomeSiteAsChildException(empty);
			}
		}
		finally
		{
			vmoClient?.Dispose();
		}
	}

	public static bool IsIISServerAccountAdminOnSite(Uri siteUri, string username)
	{
		bool result = false;
		ChannelFactory<ISecurityService> channelFactory = null;
		try
		{
			if (siteUri == null || string.IsNullOrEmpty(username))
			{
				return result;
			}
			WSHttpBinding wSHttpBinding = new WSHttpBinding();
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			NetworkCredential clientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			Uri uri = new UriBuilder(new Uri(siteUri, "ManagementServer/SecurityService.svc").OriginalString).Uri;
			Uri uri2 = new Uri("urn:videoos:management:server:securityservice");
			string spn = SpnFactory.GetSpn(uri);
			try
			{
				EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));
				channelFactory = new ChannelFactory<ISecurityService>(wSHttpBinding, remoteAddress);
				channelFactory.Credentials.Windows.ClientCredential = clientCredential;
				channelFactory.Credentials.SupportInteractive = false;
				result = CheckIfIdentitiesAreAdmin(channelFactory.CreateChannel().ReadIdentities(SearchFactor.AccountName, new string[1] { username }, QueryMembership.Expanded));
			}
			catch (EndpointNotFoundException)
			{
				try
				{
					EndpointAddress remoteAddress2 = new EndpointAddress(uri2, EndpointIdentity.CreateSpnIdentity(spn));
					channelFactory = new ChannelFactory<ISecurityService>(wSHttpBinding, remoteAddress2);
					channelFactory.Endpoint.Behaviors.Add(new ClientViaBehavior(uri));
					channelFactory.Credentials.Windows.ClientCredential = clientCredential;
					channelFactory.Credentials.SupportInteractive = false;
					result = CheckIfIdentitiesAreAdmin(channelFactory.CreateChannel().ReadIdentities(SearchFactor.AccountName, new string[1] { username }, QueryMembership.Expanded));
				}
				catch (Exception ex2)
				{
					ex2.ToString();
				}
			}
			catch (Exception ex3)
			{
				ex3.ToString();
			}
		}
		finally
		{
			if (channelFactory != null)
			{
				try
				{
					channelFactory.Close();
				}
				catch (Exception)
				{
					channelFactory.Abort();
				}
			}
		}
		return result;
	}
}
