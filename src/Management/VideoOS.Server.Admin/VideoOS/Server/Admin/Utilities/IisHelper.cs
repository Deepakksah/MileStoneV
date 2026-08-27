using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Management.Common;
using VideoOS.Server.Admin.Proxy;

namespace VideoOS.Server.Admin.Utilities;

public class IisHelper
{
	private enum InstallSslBindingActions
	{
		NothingToDo,
		CertificateMisconfigured,
		Install
	}

	private IIisInternal iisImpl;

	public const string IisLocalHostPath = "IIS://localhost/";

	public const string IisLocalWebsitePath = "IIS://localhost/W3SVC";

	public const string IisAppPoolPath = "IIS://Localhost/W3SVC/AppPools";

	private int SERVICE_DOWN_MAX_RETRY_COUNT = 20;

	private int SERVICE_UP_MAX_RETRY_COUNT = 30;

	private int SET_SSL_CERTIFICATE_MAX_RETRY_COUNT = 3;

	public const string webServerSchema = "IIsWebServer";

	public ILogger LogComponent { get; }

	public static string ConfiguredWebSiteId => GetConfiguredWebsiteId(new RegistryService());

	public IisHelper(ILogger logComponent, IIisInternal iis)
	{
		LogComponent = logComponent;
		iisImpl = iis;
	}

	public IisHelper(ILogger logComponent)
	{
		LogComponent = logComponent;
		iisImpl = new IisActual(LogComponent);
	}

	private static string SystemAccountName(ProcessModelIdentityType identityType)
	{
		return identityType switch
		{
			ProcessModelIdentityType.NetworkService => new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null).Translate(typeof(NTAccount)).Value, 
			ProcessModelIdentityType.LocalService => new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null).Translate(typeof(NTAccount)).Value, 
			ProcessModelIdentityType.LocalSystem => new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).Translate(typeof(NTAccount)).Value, 
			_ => null, 
		};
	}

	public void Control(IisAction action)
	{
		iisImpl.Control(action);
	}

	public static string GetConfiguredWebsiteId(IRegistryService registry)
	{
		if (registry == null)
		{
			throw new ArgumentNullException("registry");
		}
		string text = registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Installation\\Server", "WebSiteId", "1") as string;
		if (string.IsNullOrEmpty(text))
		{
			return "1";
		}
		return text;
	}

	public string GetPhysicalPath(string websiteId, string path)
	{
		return iisImpl.GetPhysicalPath(websiteId, path);
	}

	public static string GetAppPoolUserAccount(string appPoolName)
	{
		using ServerManager serverManager = new ServerManager();
		ApplicationPool applicationPool = serverManager.ApplicationPools.FirstOrDefault((ApplicationPool x) => x.Name.Equals(appPoolName, StringComparison.OrdinalIgnoreCase));
		if (applicationPool == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Apppool with the name {appPoolName} was not found"));
		}
		ProcessModelIdentityType identityType = applicationPool.ProcessModel.IdentityType;
		switch (identityType)
		{
		case ProcessModelIdentityType.ApplicationPoolIdentity:
			return applicationPool.Name;
		case ProcessModelIdentityType.SpecificUser:
		{
			string text = applicationPool.ProcessModel.UserName;
			if (text.StartsWith(".", StringComparison.Ordinal))
			{
				text = Environment.MachineName + text.Substring(1);
			}
			return text;
		}
		default:
			return SystemAccountName(identityType);
		}
	}

	public static void StartWebsite(string websiteId)
	{
		Site site = GetSite(websiteId);
		ObjectState state = site.State;
		if ((uint)state > 1u && (uint)(state - 2) <= 2u)
		{
			site.Start();
		}
	}

	private static Site GetSite(string websiteIdString)
	{
		using ServerManager iisManager = new ServerManager();
		return GetSite(websiteIdString, iisManager);
	}

	public static Site GetSite(string websiteIdString, ServerManager iisManager)
	{
		if (!long.TryParse(websiteIdString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var websiteId))
		{
			throw new ArgumentOutOfRangeException(FormattableString.Invariant(FormattableStringFactory.Create("{0} must be a number", "websiteIdString")));
		}
		Site site = iisManager.Sites.FirstOrDefault((Site x) => x.Id == websiteId);
		if (site == null)
		{
			throw new ServerAdminException(IISHelper_Resources.Unable_to_find_the_web_site + FormattableString.Invariant($" Id = {websiteId}"));
		}
		return site;
	}

	public static void StopWebsite()
	{
	}

	public static void DisableIisLogging(string websiteId)
	{
		using ServerManager serverManager = new ServerManager();
		Site site = GetSite(websiteId, serverManager);
		ConfigurationSection section = serverManager.GetApplicationHostConfiguration().GetSection("system.webServer/httpLogging", site.Name);
		section["selectiveLogging"] = "LogAll";
		section["dontLog"] = true;
		serverManager.CommitChanges();
	}

	public static int GetIisVersion()
	{
		return Convert.ToInt32((RegistryEx.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\InetStp", writable: false, access64bitKey: true) ?? throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Could_not_find_the_IIS_registry, "SOFTWARE\\Microsoft\\InetStp"))).GetValue("MajorVersion", 0), CultureInfo.InvariantCulture);
	}

	public static void GrantAccessToIis(string userName)
	{
		Process process = Process.Start(new ProcessStartInfo(Path.Combine(DotNetFrameworkHelper.GetFramework40Path, "aspnet_regiis.exe"))
		{
			Arguments = string.Format(CultureInfo.InvariantCulture, "-ga \"{0}\"", userName),
			UseShellExecute = false,
			RedirectStandardOutput = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			CreateNoWindow = true
		});
		string arg = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		if (process.ExitCode != 0)
		{
			throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Failed_to_grant_access_for_user, userName, process.ExitCode, arg));
		}
	}

	public static ChannelFactory<IServerService> CreateChannelFactory(string serverAddress, int serverPort, bool isServiceAccount, string accountName, string password, ILogger logger)
	{
		WSHttpBinding binding = new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			}
		};
		string text = string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}/ManagementServer/ServerService.svc", serverAddress, serverPort);
		LoggerExtensions.LogDebug(logger, string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Management_server_address, text), Array.Empty<object>());
		UriBuilder uriBuilder = new UriBuilder(text);
		EndpointAddress remoteAddress = new EndpointAddress(identity: EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(uriBuilder.Uri)), uri: uriBuilder.Uri, addressHeaders: Array.Empty<AddressHeader>());
		ChannelFactory<IServerService> channelFactory = new ChannelFactory<IServerService>(binding, remoteAddress);
		if (!isServiceAccount)
		{
			NetworkCredential clientCredential = new NetworkCredential(accountName, password);
			channelFactory.Credentials.Windows.ClientCredential = clientCredential;
			channelFactory.Credentials.SupportInteractive = false;
		}
		return channelFactory;
	}

	public void ValidateWebService(AccountInfo account, string atServer, int atServerPort, string websiteId)
	{
		if (account == null)
		{
			throw new ArgumentNullException("account");
		}
		Control(IisAction.Restart);
		StartWebsite(websiteId);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		string arg = "UNKNOWN";
		int num = 0;
		do
		{
			flag3 = false;
			flag2 = false;
			try
			{
				using ChannelFactory<IServerService> channelFactory = CreateChannelFactory(atServer, atServerPort, account.IsServiceAccount, account.AccountName, account.Password, LogComponent);
				try
				{
					arg = channelFactory.CreateChannel().CheckAuthentication();
					flag = true;
				}
				catch (FaultException<ExceptionDetail> ex)
				{
					channelFactory.Abort();
					flag3 = true;
					if (ex.Detail.Type.Equals("System.UnauthorizedAccessException"))
					{
						LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Service_up_but_not_yet_synchronized, Array.Empty<object>());
					}
					else
					{
						LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Service_up_but_not_yet_synchronized_Error_occurred_while_trying_to_check_authentication, Array.Empty<object>());
					}
					LoggerExtensions.LogDebug(LogComponent, ex.ToString(), Array.Empty<object>());
				}
				catch (TimeoutException)
				{
					channelFactory.Abort();
					LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Not_yet_synchronized_Check_authentication_timed_out, Array.Empty<object>());
				}
				catch (Exception ex3)
				{
					channelFactory.Abort();
					LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Not_yet_synchronized_Error_occurred_while_trying_to_check_authentication, Array.Empty<object>());
					LoggerExtensions.LogDebug(LogComponent, ex3.ToString(), Array.Empty<object>());
				}
			}
			catch (Exception ex4)
			{
				LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Not_yet_synchronized_Error_occurred_while_trying_to_check_authentication, Array.Empty<object>());
				LoggerExtensions.LogDebug(LogComponent, ex4.ToString(), Array.Empty<object>());
			}
			int num2 = 0;
			if (flag)
			{
				continue;
			}
			if (flag3)
			{
				if (num < SERVICE_UP_MAX_RETRY_COUNT)
				{
					flag2 = true;
					num2 = SERVICE_UP_MAX_RETRY_COUNT;
				}
			}
			else if (num < SERVICE_DOWN_MAX_RETRY_COUNT)
			{
				flag2 = true;
				num2 = SERVICE_DOWN_MAX_RETRY_COUNT;
			}
			if (flag2)
			{
				num++;
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Retrying_in_4_seconds, num, num2), Array.Empty<object>());
				Thread.Sleep(12000);
			}
			else
			{
				LoggerExtensions.LogDebug(LogComponent, IISHelper_Resources.Not_yet_synchronized_abort, Array.Empty<object>());
			}
		}
		while (flag2);
		if (flag)
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Synchronization_complete, arg), Array.Empty<object>());
			return;
		}
		throw new ServerAdminException(string.Format(CultureInfo.InvariantCulture, IISHelper_Resources.Error_authenticating_user_service, flag3 ? IISHelper_Resources.up_but_not_synchronized : "down"));
	}

	public bool CheckNewCertificate(int httpsPort, string certificateHash)
	{
		SslCertificateBindingInformation certificateBinding = iisImpl.GetCertificateBinding(httpsPort);
		if (certificateBinding.HasCertificateBinding)
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_ssl_port_in_certificate_ok, httpsPort), Array.Empty<object>());
			if (certificateBinding.CertificateExistsInStore)
			{
				if (string.Equals(certificateBinding.CertificateHash, certificateHash, StringComparison.OrdinalIgnoreCase))
				{
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_hash_ok, certificateHash), Array.Empty<object>());
					return true;
				}
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_hashes_not_equal), Array.Empty<object>());
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_store_hash_binding_hash, certificateHash, certificateBinding.CertificateHash), Array.Empty<object>());
			}
			else
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_binding_hash_not_in_store, certificateBinding.CertificateHash), Array.Empty<object>());
			}
		}
		else
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_ssl_port_in_binding_not_found), Array.Empty<object>());
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_expected_ssl_port_and_found_string, httpsPort, certificateBinding.FoundHttpsPortString), Array.Empty<object>());
		}
		return false;
	}

	public void AddSslCertificateBindingsWithRetry(int httpsPort, string certificateHash, int timeout)
	{
		if (CheckNewCertificate(httpsPort, certificateHash))
		{
			return;
		}
		int num = 0;
		do
		{
			if (num > 0)
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_retry, num), Array.Empty<object>());
			}
			num++;
			SslHelper.OnStatus(new SslStatusEventArgs(SslStatusStep.CreateSslCertificateConfiguration));
			iisImpl.AddSslCertificate(httpsPort, certificateHash);
			Thread.Sleep(timeout);
			iisImpl.IisControlledReset();
			if (CheckNewCertificate(httpsPort, certificateHash))
			{
				return;
			}
		}
		while (num < SET_SSL_CERTIFICATE_MAX_RETRY_COUNT);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Add_SSL_cert), Array.Empty<object>());
		throw new ServerAdminException(IISHelper_Resources.Error_Add_SSL_cert);
	}

	public static IisWebsite[] GetIisWebsites(ILogger logger)
	{
		List<IisWebsite> list = new List<IisWebsite>();
		using (ServerManager serverManager = new ServerManager())
		{
			SiteCollection sites = serverManager.Sites;
			if (sites != null)
			{
				foreach (Site item2 in sites)
				{
					string name = item2.Name;
					long id = item2.Id;
					IisWebsiteState state = (IisWebsiteState)Enum.Parse(typeof(IisWebsiteState), item2.State.ToString(), ignoreCase: true);
					IisWebsite item = new IisWebsite(name, id, state);
					list.Add(item);
				}
			}
		}
		IisHelper iisHelper = new IisHelper(logger);
		bool flag = false;
		foreach (IisWebsite item3 in list)
		{
			if (iisHelper.CheckSecureBindings(item3.Id.ToString(CultureInfo.InvariantCulture)))
			{
				flag = true;
				if (iisHelper.CheckSecureBindingsAndAssociatedCertificate(item3.Id.ToString(CultureInfo.InvariantCulture), ignoreExistingCertificate: true))
				{
					item3.Status = IisWebsiteStatus.Valid;
				}
				else
				{
					item3.Status = IisWebsiteStatus.HttpsNotConfiguredCorrectly;
				}
			}
		}
		foreach (IisWebsite item4 in list)
		{
			if (item4.Status == IisWebsiteStatus.Unknown)
			{
				if (flag)
				{
					item4.Status = IisWebsiteStatus.OtherSiteHttps;
				}
				else
				{
					item4.Status = IisWebsiteStatus.Valid;
				}
			}
		}
		return list.ToArray();
	}

	public bool CheckSecureBindingsAndAssociatedCertificate(string websiteId)
	{
		return CheckSecureBindingsAndAssociatedCertificate(websiteId, ignoreExistingCertificate: false);
	}

	public bool CheckSecureBindingsAndAssociatedCertificate(string websiteId, bool ignoreExistingCertificate)
	{
		HttpsBindingInformation httpsBindingInformation = iisImpl.CheckSecureBindingsAndGetHttpsPort(websiteId);
		if (httpsBindingInformation.HasBinding)
		{
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_secure_bindings_ok, httpsBindingInformation.Port), Array.Empty<object>());
			SslCertificateBindingInformation certificateBinding = iisImpl.GetCertificateBinding(httpsBindingInformation.Port);
			if (certificateBinding.HasCertificateBinding)
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_ssl_port_in_certificate_ok, httpsBindingInformation.Port), Array.Empty<object>());
				if (ignoreExistingCertificate)
				{
					return true;
				}
				if (certificateBinding.CertificateExistsInStore)
				{
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_hash_ok, certificateBinding.CertificateHash), Array.Empty<object>());
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_and_https_binding_ok), Array.Empty<object>());
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckSecureBindings(string websiteId)
	{
		return iisImpl.CheckSecureBindingsAndGetHttpsPort(websiteId).HasBinding;
	}

	public int GetHttpPort(string websiteId)
	{
		return iisImpl.GetHttpPort(websiteId).Port;
	}

	public int GetHttpsPort(string websiteId)
	{
		return iisImpl.CheckSecureBindingsAndGetHttpsPort(websiteId).Port;
	}

	public void CreateCertificateAndHttpsBindingIfNeeded(string websiteId)
	{
		CreateCertificateAndHttpsBindingIfNeeded(websiteId, "");
	}

	public void CreateCertificateAndHttpsBindingIfNeeded(string websiteId, string certificateHash)
	{
		CreateCertificateAndHttpsBindingIfNeeded(websiteId, certificateHash, ignoreExistingCertificate: false);
	}

	public void CreateCertificateAndHttpsBindingIfNeeded(string websiteId, string certificateHash, bool ignoreExistingCertificate)
	{
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_check_certificate_and_https_binding, websiteId), Array.Empty<object>());
		int iisVersion = GetIisVersion();
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_for_IIS_version, iisVersion), Array.Empty<object>());
		Tuple<int, InstallSslBindingActions> tuple = FigureWhatToDo(websiteId, certificateHash, ignoreExistingCertificate);
		int item = tuple.Item1;
		switch (tuple.Item2)
		{
		case InstallSslBindingActions.NothingToDo:
			break;
		case InstallSslBindingActions.CertificateMisconfigured:
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_or_https_binding_not_ok), Array.Empty<object>());
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.Error_Https_binding_on_website_and_port_has_no_certificate_associated, "localhost", item));
		default:
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_or_https_binding_not_ok), Array.Empty<object>());
			DeleteAndInstallSslBindings(item, websiteId, certificateHash);
			break;
		}
	}

	private Tuple<int, InstallSslBindingActions> FigureWhatToDo(string websiteId, string certificateHash, bool ignoreExistingCertificate)
	{
		HttpsBindingInformation httpsBindingInformation = iisImpl.CheckSecureBindingsAndGetHttpsPort(websiteId);
		if (!httpsBindingInformation.HasBinding)
		{
			return new Tuple<int, InstallSslBindingActions>(httpsBindingInformation.Port, InstallSslBindingActions.Install);
		}
		SslCertificateBindingInformation certificateBinding = iisImpl.GetCertificateBinding(httpsBindingInformation.Port);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_secure_bindings_ok, httpsBindingInformation.Port), Array.Empty<object>());
		if (!certificateBinding.HasCertificateBinding)
		{
			return new Tuple<int, InstallSslBindingActions>(0, InstallSslBindingActions.CertificateMisconfigured);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_ssl_port_in_certificate_ok, httpsBindingInformation.Port), Array.Empty<object>());
		if (!ignoreExistingCertificate && !certificateBinding.CertificateExistsInStore)
		{
			return new Tuple<int, InstallSslBindingActions>(0, InstallSslBindingActions.CertificateMisconfigured);
		}
		if (!string.IsNullOrWhiteSpace(certificateHash))
		{
			if (certificateHash == certificateBinding.CertificateHash)
			{
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_hash_ok, certificateBinding.CertificateHash), Array.Empty<object>());
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_and_https_binding_ok), Array.Empty<object>());
				return new Tuple<int, InstallSslBindingActions>(0, InstallSslBindingActions.NothingToDo);
			}
			return new Tuple<int, InstallSslBindingActions>(httpsBindingInformation.Port, InstallSslBindingActions.Install);
		}
		return new Tuple<int, InstallSslBindingActions>(httpsBindingInformation.Port, InstallSslBindingActions.Install);
	}

	private void DeleteAndInstallSslBindings(int httpsPort, string websiteId, string certificateHash)
	{
		SslHelper.OnStatus(new SslStatusEventArgs(SslStatusStep.DeleteSslCertificateConfiguration));
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_deleting_ssl_certificate_binding), Array.Empty<object>());
		iisImpl.Iis7DeleteSslCertificateBindings(httpsPort);
		SslHelper.OnStatus(new SslStatusEventArgs(SslStatusStep.DeleteSslBinding));
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_deleting_https_binding), Array.Empty<object>());
		iisImpl.DeleteSecureBindings(websiteId);
		if (string.IsNullOrWhiteSpace(certificateHash))
		{
			SslHelper.OnStatus(new SslStatusEventArgs(SslStatusStep.CreateSelfSignedCertificate));
			certificateHash = iisImpl.CreateSelfSignedCertificateIfNeeded();
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_creating_ssl_certificate_binding), Array.Empty<object>());
		AddSslCertificateBindingsWithRetry(httpsPort, certificateHash, 1000);
		SslHelper.OnStatus(new SslStatusEventArgs(SslStatusStep.CreateSslBinding));
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_creating_https_binding), Array.Empty<object>());
		iisImpl.SetSecureBindings(websiteId, httpsPort);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_certificate_and_https_binding_ok), Array.Empty<object>());
	}

	public static void SetPreloadEnabled(string appPath, bool enabled)
	{
		try
		{
			using ServerManager serverManager = new ServerManager();
			foreach (Site site in serverManager.Sites)
			{
				site.Applications.FirstOrDefault((Application app) => app.Path == appPath)?.SetAttributeValue("preloadEnabled", enabled);
			}
			serverManager.CommitChanges();
		}
		catch (COMException)
		{
		}
	}

	public AppPoolState RecycleAppPool(string appPoolName)
	{
		return iisImpl.RecycleAppPool(appPoolName);
	}

	public AppPoolState GetAppPoolState(string appPoolName)
	{
		return iisImpl.GetAppPoolState(appPoolName);
	}
}
