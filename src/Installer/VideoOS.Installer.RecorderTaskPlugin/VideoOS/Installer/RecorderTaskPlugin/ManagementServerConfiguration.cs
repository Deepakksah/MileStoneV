using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Communication;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.RecorderTaskPlugin;

public static class ManagementServerConfiguration
{
	public static void AddUserToAdminRole(Dictionary<string, string> installParameters)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		LogManager.Write("AddUserToAdminRole(): Starting AddUserToAdminRole");
		if (GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER"))
		{
			LogManager.Write("AddUserToAdminRole(): Not connected to Server, nothing to do.");
			return;
		}
		Uri uri = new Uri(installParameters["MANAGEMENTSERVERURI"]);
		Uri uri2 = new Uri(installParameters["IDPADDRESS"]);
		bool useHttps = uri.Scheme == Uri.UriSchemeHttps;
		Uri baseServerAddress = uri2;
		try
		{
			using IdentityServerWindowsTokenHandler tokenHandler = new IdentityServerWindowsTokenHandler((NetworkCredential)CredentialCache.DefaultCredentials, new IdentityServerClientSettings(baseServerAddress, "GrantValidatorClient", "windows_credentials"));
			using SimpleTokenCache tokenCache = new SimpleTokenCache(tokenHandler);
			using ServerConnection serverConnection = new ServerConnection(uri.Host, useHttps, tokenCache);
			serverConnection.ServerPort = uri.Port;
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRole(): Creating VMO client to Management Server '{0}'", uri.AbsoluteUri));
			using VmoClient client = new VmoClient(serverConnection);
			AddUserToAdminRoleThroughVMO(new VmoWrapper(client), installParameters);
		}
		catch (Exception innerException)
		{
			throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRole(): Connect to Management Server failed"), innerException);
		}
		LogManager.Write("AddUserToAdminRole(): Finished AddUserToAdminRole");
	}

	public static void AddUserToAdminRoleThroughVMO(IVmoWrapper server, Dictionary<string, string> installParameters)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		LogManager.Write("AddUserToAdminRoleThroughVMO(): Starting AddUserToAdminRoleThroughVMO");
		try
		{
			string text = null;
			text = installParameters["ACCOUNTNAME"];
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): Adding user '{0}' to 'Administrators' role on Management Server '{1}'", text, installParameters["MANAGEMENTSERVERURI"]));
			Role administratorRole = server.GetAdministratorRole();
			if (administratorRole == null)
			{
				throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): Failed, Role 'Administrators' does not exist"));
			}
			try
			{
				string accountNameToUse = GetAccountNameToUse(text);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): Account name to add: '{0}'", accountNameToUse));
				server.AddUserToAdminRole(administratorRole, accountNameToUse);
			}
			catch (AddMemberAlreadyMemberException)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): User already added to '{0}' role", administratorRole.Name));
				return;
			}
			catch (Exception innerException)
			{
				throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): Error adding user '{0}'", text), innerException);
			}
		}
		catch (Exception innerException2)
		{
			throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddUserToAdminRoleThroughVMO(): Failed"), innerException2);
		}
		LogManager.Write("AddUserToAdminRoleThroughVMO(): Done! ");
	}

	private static string GetAccountNameToUse(string accountName)
	{
		if (IsWorkgroupMember())
		{
			string[] array = accountName.Split('\\');
			accountName = array[array.Length - 1];
		}
		return accountName;
	}

	private static bool IsWorkgroupMember()
	{
		bool result = false;
		try
		{
			Domain.GetComputerDomain();
		}
		catch (ActiveDirectoryObjectNotFoundException)
		{
			result = true;
		}
		catch (Exception)
		{
			LogManager.Write("IsWorkgroupMember(): Could not determine if workstation is in a workgroup or domain.", TraceEventType.Warning);
			LogManager.Write("IsWorkgroupMember(): This could be because you are logged in with a local windows user on a workstation connected to a domain, or the trust relationship between this workstation and the primary domain failed", TraceEventType.Warning);
			LogManager.Write("IsWorkgroupMember(): Both of the above cases are not supported", TraceEventType.Warning);
			throw;
		}
		return result;
	}

	public static void AddRecorderToManagementServer(Dictionary<string, string> installParameters)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		LogManager.Write("AddRecorderToManagementServer(): Starting AddRecorderToManagementServer");
		if (GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER"))
		{
			LogManager.Write("AddRecorderToManagementServer(): Not connected to Server, nothing to do.");
			return;
		}
		Uri uri = new Uri(installParameters["MANAGEMENTSERVERURI"]);
		Uri uri2 = new Uri(installParameters["IDPADDRESS"]);
		bool useHttps = uri.Scheme == Uri.UriSchemeHttps;
		Uri baseServerAddress = uri2;
		try
		{
			using IdentityServerWindowsTokenHandler tokenHandler = new IdentityServerWindowsTokenHandler((NetworkCredential)CredentialCache.DefaultCredentials, new IdentityServerClientSettings(baseServerAddress, "GrantValidatorClient", "windows_credentials"));
			using SimpleTokenCache tokenCache = new SimpleTokenCache(tokenHandler);
			using ServerConnection serverConnection = new ServerConnection(uri.Host, useHttps, tokenCache);
			serverConnection.ServerPort = uri.Port;
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Creating VMO client to Management Server '{0}'", uri.AbsoluteUri));
			using VmoClient client = new VmoClient(serverConnection);
			AddRecorderToManagementServerThroughVMO(new VmoWrapper(client), new IPHelperWrapper(), installParameters);
		}
		catch (RecordingServerUnlicensedException innerException)
		{
			throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): The configuration contains more recording servers than your license allows."), innerException);
		}
		catch (Exception innerException2)
		{
			throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Connect to Management Server failed"), innerException2);
		}
		LogManager.Write("AddRecorderToManagementServer(): Stopping AddRecorderToManagementServer");
	}

	public static void AddRecorderToManagementServerThroughVMO(IVmoWrapper srv, IIPHelperWrapper IPHelper, Dictionary<string, string> installParameters)
	{
		Guid guid = Guid.Parse(installParameters["RECORDERGUID"]);
		bool flag = false;
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Looking up recording server with id {0}", guid.ToString()));
		try
		{
			flag = srv.RecorderExists(guid);
		}
		catch (Exception innerException)
		{
			throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Looking up recording server failed"), innerException);
		}
		if (!flag)
		{
			XmlDocument recorderConfig = GetRecorderConfig();
			int port = RecorderWebServicePort(recorderConfig);
			int defaultFailoverUdpPort = FailoverServerPort(recorderConfig);
			string hostName = RecorderWebServiceHostName(recorderConfig, IPHelper);
			try
			{
				string absoluteUri = WebServerUri(hostName, port).AbsoluteUri;
				string text = installParameters["RECORDERNAME"];
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Creating recorder '{0}' with id '{1}' and address '{2}'", text, guid, absoluteUri));
				int num = 24;
				int num2 = 60;
				srv.CreateRecorder(guid, text, hostName, absoluteUri, new RecordingServerCreateOptions
				{
					DefaultStorageRetainMinutes = int.Parse(installParameters["STORAGERETENTIONTIME"], CultureInfo.InvariantCulture) * num * num2,
					DefaultStoragePath = installParameters["MEDIADBPATH"],
					DefaultFailoverUdpPort = defaultFailoverUdpPort
				});
				return;
			}
			catch (RecordingServerUnlicensedException)
			{
				throw;
			}
			catch (Exception innerException2)
			{
				throw new ManagementServerConfigurationException(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Creating recording server failed"), innerException2);
			}
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AddRecorderToManagementServer(): Recorder with id '{0}' already exists", guid));
	}

	private static XmlDocument GetRecorderConfig()
	{
		string path = "RecorderConfig.xml";
		string filename = Path.Combine(OemResources.Instance.Recorder.CommonApplicationDataFolder, path);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(filename);
		return xmlDocument;
	}

	private static Uri WebServerUri(string hostName, int port)
	{
		return UriHandling.CreateHttpUri(hostName, port);
	}

	private static int RecorderWebServicePort(XmlDocument document)
	{
		return UriHandling.ConvertStringToPort(document.SelectSingleNode("/recorderconfig/webserver/port").InnerXml);
	}

	private static int FailoverServerPort(XmlDocument document)
	{
		return int.Parse(document.SelectSingleNode("/recorderconfig/failoverserver/port").InnerXml, CultureInfo.InvariantCulture);
	}

	private static string RecorderHostNameFromConfig(XmlDocument document)
	{
		return document.SelectSingleNode("/recorderconfig/webserver/host").InnerXml;
	}

	private static string RecorderWebServiceHostName(XmlDocument document, IIPHelperWrapper IPHelper)
	{
		string text = RecorderHostNameFromConfig(document);
		string result = "localhost";
		if (text != null && text.Length != 0)
		{
			result = text;
		}
		else
		{
			try
			{
				result = IPHelper.GetHostName();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return result;
	}
}
