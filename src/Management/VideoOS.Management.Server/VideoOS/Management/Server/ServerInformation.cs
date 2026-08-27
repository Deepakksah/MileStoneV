using System;
using System.Globalization;
using System.Net.NetworkInformation;
using VideoOS.Management.Common;
using VideoOS.Server.Webservice.Shared;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public sealed class ServerInformation
{
	private const string _area = "ManagementServer";

	private const string _layer = "ServerInformation";

	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Platform { get; set; }

	public int Processors { get; set; }

	public int PhysicalMemory { get; set; }

	public string ComputerName { get; set; }

	public string ServiceAccount { get; set; }

	public Version Version { get; set; }

	public ServerAddress[] Addresses { get; set; }

	public string MasterSite { get; set; }

	public DateTime LastModified { get; set; }

	public string DomainName { get; set; }

	public DateTime LastStatusHandshake { get; set; }

	public int SynchronizationStatus { get; set; }

	public string TimeZone { get; set; }

	public string PrivateViewGroupId { get; set; }

	public static ServerInformation GetServerInformation(IGeminiRequestContext requestContext)
	{
		requestContext.RequestTracer.TraceEnter(0, "ManagementServer", "ServerInformation", "GetServerInformationNoSecurity");
		try
		{
			ServerInformation serverInformation = null;
			bool privateViewGroupCreated;
			using (IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent())
			{
				serverInformation = managementServerComponent.GetServerInformation(requestContext.AuthenticatedUserSid, out privateViewGroupCreated);
			}
			if (privateViewGroupCreated)
			{
				PrivateViewGroupTokenCache.Clear();
				IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, serverInformation.PrivateViewGroupId);
				requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid).ForceNamespaceReload(requestContext);
			}
			serverInformation.Platform = "[Not Available]";
			serverInformation.ComputerName = Environment.MachineName;
			serverInformation.DomainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
			string accountName = string.Empty;
			IISHelper.IdentityType identityType = IISHelper.IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSERVICE;
			IISHelper.GetServiceAccountName(ref accountName, ref identityType);
			serverInformation.ServiceAccount = accountName;
			serverInformation.Processors = 0;
			serverInformation.PhysicalMemory = 0;
			serverInformation.TimeZone = TimeZoneInfo.Local.Id;
			return serverInformation;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "ManagementServer", "ServerInformation", "GetServerInformationNoSecurity");
		}
	}

	public static ServerInformation UpdateServerInformation(IGeminiRequestContext requestContext, string name, string description, ServerAddress[] addresses, CustomProperty[] customProperties)
	{
		Site.UpdateHomeSite(requestContext, name, description, addresses, customProperties);
		GeminiApplicationCore.SiteManager.SynchronizeAllSites(requestContext, waitForComplete: false);
		return GetServerInformation(requestContext);
	}

	public static void UpdateServerInformationValidation(IGeminiRequestContext requestContext, string name, string description, ServerAddress[] addresses)
	{
		Site.UpdateHomeSiteValidation(requestContext, name, description, addresses);
	}

	public static string GetSystemSetting(IGeminiRequestContext requestContext, string setting)
	{
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		return managementServerComponent.GetSystemSetting(setting);
	}

	public static string GetSystemSetting(IGeminiRequestContext requestContext, SettingList setting)
	{
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		return managementServerComponent.GetSystemSetting(setting);
	}

	public static bool SetSystemSetting(IGeminiRequestContext requestContext, string setting, string value)
	{
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		return managementServerComponent.SetSystemSetting(setting, value);
	}

	public static int GetServerStatus()
	{
		using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
		if (!string.IsNullOrEmpty(serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetVmoServerName(), null, (Exception ex, Action ac) => false)))
		{
			return 1;
		}
		return 2;
	}
}
