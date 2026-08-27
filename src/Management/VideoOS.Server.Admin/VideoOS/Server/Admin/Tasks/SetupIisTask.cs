using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using VideoOS.Server.Admin.Utilities;
using VideoOS.Utilities.Security;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Admin.Tasks;

public class SetupIisTask
{
	private const string ReportServerAppPoolName = "VideoOS ReportServer AppPool";

	private const string ClassicAppPoolName = "VideoOS Classic AppPool";

	private const string ManagementServerAppPoolName = "VideoOS Management Server AppPool";

	public const string IdentityProviderAppPoolName = "VideoOS IDP AppPool";

	public const string IncidentManagerAppPoolName = "VideoOS IM AppPool";

	private IisHelper iisHelper;

	public ILogger LogComponent { get; public set; }

	public SetupIisTask(ILogger logComponent)
	{
		LogComponent = logComponent;
		iisHelper = new IisHelper(logComponent);
	}

	public void Install(string virtualDirectoryParentFolder, string websiteId, int websitePort, AccountInfo serviceAccount, bool securedServer, string certificateHash, Action registerManagementServerAction)
	{
		if (serviceAccount == null)
		{
			throw new ArgumentException(SetupIISTask_Resources.Service_account_is_null, "serviceAccount");
		}
		if (string.IsNullOrEmpty(virtualDirectoryParentFolder))
		{
			throw new ArgumentException(SetupIISTask_Resources.Virtual_directory_parent_folder_is_null_or_empty, "virtualDirectoryParentFolder");
		}
		if (string.IsNullOrEmpty(websiteId))
		{
			throw new ArgumentException(SetupIISTask_Resources.Website_id_is_null_or_empty, "websiteId");
		}
		if (websitePort <= 0)
		{
			throw new ArgumentException(SetupIISTask_Resources.Website_port_must_be_higher_than_0, "websitePort");
		}
		LoggerExtensions.LogDebug(LogComponent, SetupIISTask_Resources.Starting_the_IIS_setup, Array.Empty<object>());
		if (!Directory.Exists(virtualDirectoryParentFolder))
		{
			throw new ArgumentException(SetupIISTask_Resources.The_directory_specified_for_the_virtual_directories_was_not_found, virtualDirectoryParentFolder);
		}
		Uninstall(websiteId);
		IisHelper.GrantAccessToIis(serviceAccount.AccountName);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupIISTask_Resources.Granted_access_to_the_IIS_User, serviceAccount.AccountName), Array.Empty<object>());
		iisHelper.CreateCertificateAndHttpsBindingIfNeeded(websiteId, certificateHash, ignoreExistingCertificate: true);
		SetupAppPools(serviceAccount);
		CreateVirtualDirectories(virtualDirectoryParentFolder, serviceAccount, websiteId);
		if (securedServer)
		{
			SslHelper.EnableSsl(websiteId, "IDP");
		}
		else
		{
			SslHelper.DisableSsl(websiteId, "IDP");
		}
		IisHelper.DisableIisLogging(websiteId);
		iisHelper.Control(IisAction.Start);
		registerManagementServerAction?.Invoke();
		UpdateManagementWebConfig(Path.Combine(virtualDirectoryParentFolder, SetupIISTask_Resources.ManagementServer), websitePort);
		int httpsPort = iisHelper.GetHttpsPort(websiteId);
		UpdateServerConfig(websitePort, httpsPort);
	}

	private void CreateVirtualDirectories(string virtualDirectoryPhysicalPath, AccountInfo serviceAccount, string webSiteId)
	{
		string fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\Httpdocs"));
		CreateVirtualDirectory(webSiteId, "Installation", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, enableDefaultDocument: true, "default.htm", fullPath, "VideoOS Classic AppPool");
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\Httpdocs\\Admin"));
		CreateVirtualDirectory(webSiteId, "Installation\\Admin", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, enableDefaultDocument: true, "default.htm", fullPath, "VideoOS Classic AppPool");
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\ManagementServer"));
		CreateVirtualDirectory(webSiteId, "ManagementServer", AuthorizationTypes.Anonymous, enableDefaultDocument: true, "default.aspx", fullPath, "VideoOS Management Server AppPool");
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\ServerConnector"));
		IEnumerable<HandlerMapping> scriptMapsToAdd = new HandlerMapping[1]
		{
			new HandlerMapping("XmlHandlerMapping", "*.xml", "GET,HEAD,POST,DEBUG", "IsapiModule", Path.Combine(DotNetFrameworkHelper.GetFramework40Path, "aspnet_isapi.dll"), AccessRights.Script)
		};
		CreateVirtualDirectory(webSiteId, "ServerConnector", AuthorizationTypes.Anonymous, enableDefaultDocument: false, "", fullPath, "VideoOS Classic AppPool", scriptMapsToAdd, null);
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\HtmlHelp"));
		CreateVirtualDirectory(webSiteId, "HtmlHelp", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, enableDefaultDocument: true, "help.htm", fullPath, "VideoOS Classic AppPool");
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\Reporting"));
		CreateVirtualDirectory(webSiteId, "Reporting", AuthorizationTypes.Anonymous, enableDefaultDocument: true, "default.aspx", fullPath, "VideoOS ReportServer AppPool");
		FileSystemAccessRule accessRule = new FileSystemAccessRule(serviceAccount.AccountName, FileSystemRights.FullControl, AccessControlType.Allow);
		Computer.ReplaceAllDescendantPermissions(new DirectoryInfo(fullPath), accessRule);
		IisHelper.SetPreloadEnabled("/Reporting", enabled: true);
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\IDP"));
		CreateVirtualDirectory(webSiteId, "IDP", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, enableDefaultDocument: false, "", fullPath, "VideoOS IDP AppPool");
		fullPath = Path.GetFullPath(Path.Combine(virtualDirectoryPhysicalPath, ".\\IncidentManager"));
		CreateVirtualDirectory(webSiteId, "IncidentManager", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, enableDefaultDocument: false, "", fullPath, "VideoOS IM AppPool");
	}

	private void CreateVirtualDirectory(string websiteId, string websiteName, AuthorizationTypes authorization, bool enableDefaultDocument, string defaultDocument, string physicalpath, string appPoolName)
	{
		CreateVirtualDirectory(websiteId, websiteName, authorization, enableDefaultDocument, defaultDocument, physicalpath, appPoolName, null, null);
	}

	private void CreateVirtualDirectory(string websiteId, string virtualApplicationPath, AuthorizationTypes authorization, bool enableDefaultDocument, string defaultDocument, string physicalPath, string appPoolName, IEnumerable<HandlerMapping> scriptMapsToAdd, IEnumerable<HandlerMapping> scriptMapsToRemove)
	{
		VideoOS.Server.Admin.Utilities.VirtualDirectory virtualDirectory = new VideoOS.Server.Admin.Utilities.VirtualDirectory(websiteId, appPoolName, virtualApplicationPath, physicalPath);
		virtualDirectory.Authorization = authorization;
		virtualDirectory.EnableDefaultDoc = enableDefaultDocument;
		virtualDirectory.DefaultDoc = defaultDocument;
		if (scriptMapsToAdd != null)
		{
			virtualDirectory.ScriptMapsToAdd = scriptMapsToAdd;
		}
		if (scriptMapsToRemove != null)
		{
			virtualDirectory.ScriptMapsToRemove = scriptMapsToRemove;
		}
		virtualDirectory.CommitChanges();
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupIISTask_Resources.Created_virtual_directory_Directory, virtualDirectory.VirtualLocation), Array.Empty<object>());
	}

	private void SetupAppPools(AccountInfo serviceAccount)
	{
		using (AppPoolHelper appPoolHelper = new AppPoolHelper(LogComponent))
		{
			appPoolHelper.CreateApplicationPool("VideoOS Classic AppPool", serviceAccount, classicManagedPipelineMode: true, AppPoolHelper.StartMode.OnDemand, "v4.0");
			appPoolHelper.CreateApplicationPool("VideoOS Management Server AppPool", serviceAccount, classicManagedPipelineMode: false, AppPoolHelper.StartMode.OnDemand, "v4.0");
			appPoolHelper.CreateApplicationPool("VideoOS ReportServer AppPool", serviceAccount, classicManagedPipelineMode: true, AppPoolHelper.StartMode.AlwaysRunning, "v4.0");
			appPoolHelper.CreateApplicationPool("VideoOS IDP AppPool", serviceAccount, classicManagedPipelineMode: false, AppPoolHelper.StartMode.AlwaysRunning, "", rapidFailProtection: false);
			appPoolHelper.CreateApplicationPool("VideoOS IM AppPool", serviceAccount, classicManagedPipelineMode: false, AppPoolHelper.StartMode.AlwaysRunning, "v4.0");
		}
		LoggerExtensions.LogDebug(LogComponent, SetupIISTask_Resources.Created_the_App_Pools, Array.Empty<object>());
	}

	public void Uninstall(string websiteId)
	{
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "Server");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "Installation");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "Installation/Admin");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "ManagementServer");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "SystemInformation");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "ServerConnector");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "HtmlHelp");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "Reporting");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "IDP");
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "IncidentManager");
		using AppPoolHelper appPoolHelper = new AppPoolHelper(LogComponent);
		DeleteAppPool(appPoolHelper, "VideoOS Classic AppPool");
		DeleteAppPool(appPoolHelper, "VideoOS Management Server AppPool");
		DeleteAppPool(appPoolHelper, "VideoOS ReportServer AppPool");
		DeleteAppPool(appPoolHelper, "VideoOS IDP AppPool");
		DeleteAppPool(appPoolHelper, "VideoOS IM AppPool");
	}

	private static void DeleteAppPool(AppPoolHelper appPoolHelper, string appPoolName)
	{
		try
		{
			ApplicationPool appPool = appPoolHelper.GetAppPool(appPoolName);
			if (appPool != null)
			{
				appPoolHelper.DeleteApplicationPool(appPool);
			}
		}
		catch (DirectoryNotFoundException)
		{
		}
		catch (DirectoryServicesCOMException)
		{
		}
	}

	private static void UpdateManagementWebConfig(string managementServerFolder, int webSitePort)
	{
		string text = Path.Combine(managementServerFolder, "Web.config");
		XmlDocument xmlDocument = null;
		if ((File.GetAttributes(text) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
		{
			File.SetAttributes(text, FileAttributes.Normal);
		}
		xmlDocument = new XmlDocument();
		xmlDocument.Load(text);
		XmlElement documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement.SelectNodes("system.serviceModel/services/service/endpoint"))
		{
			if (item.Attributes.GetNamedItem("listenUri") != null)
			{
				item.Attributes.GetNamedItem("listenUri").Value = item.Attributes.GetNamedItem("listenUri").Value.Replace("http://localhost/", "http://localhost:" + webSitePort.ToString(CultureInfo.InvariantCulture) + "/");
			}
			if (item.Attributes.GetNamedItem("address") != null)
			{
				item.Attributes.GetNamedItem("address").Value = item.Attributes.GetNamedItem("address").Value.Replace("http://localhost/", "http://localhost:" + webSitePort.ToString(CultureInfo.InvariantCulture) + "/");
			}
		}
		if (IisHelper.GetIisVersion() < 7)
		{
			XmlNode xmlNode2 = documentElement.SelectSingleNode("system.web");
			XmlElement xmlElement = xmlDocument.CreateElement("httpModules");
			XmlElement xmlElement2 = xmlDocument.CreateElement("add");
			xmlElement2.SetAttributeNode("name", string.Empty).Value = "GeminiAuthentication";
			xmlElement2.SetAttributeNode("type", string.Empty).Value = "VideoOS.Management.Server.GeminiAuthenticationModule, VideoOS.Management.Server";
			xmlElement.AppendChild(xmlElement2);
			xmlNode2.AppendChild(xmlElement);
		}
		xmlDocument.Save(text);
	}

	private static void UpdateServerConfig(int atPort, int atHttpsPort)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
		folderPath = Path.Combine(folderPath, OemResources.Instance.App.CompanyName);
		folderPath = Path.Combine(folderPath, OemResources.Instance.Server.DefaultInstallDir);
		folderPath = Path.Combine(folderPath, "ServerConfig.xml");
		if ((File.GetAttributes(folderPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
		{
			File.SetAttributes(folderPath, FileAttributes.Normal);
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(folderPath);
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlNode xmlNode = documentElement.SelectSingleNode("/server/VMOCommunication/Port");
		if (xmlNode != null)
		{
			xmlNode.InnerText = atPort.ToString(CultureInfo.InvariantCulture);
		}
		XmlNode xmlNode2 = documentElement.SelectSingleNode("/server/VMOCommunication/HttpsPort");
		if (xmlNode2 != null)
		{
			xmlNode2.InnerText = atHttpsPort.ToString(CultureInfo.InvariantCulture);
		}
		xmlDocument.Save(folderPath);
	}
}
