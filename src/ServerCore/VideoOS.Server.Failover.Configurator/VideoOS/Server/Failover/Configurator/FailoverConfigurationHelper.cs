using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Diagnostics.Logging;
using VideoOS.Server.Failover.Configurator.Properties;

namespace VideoOS.Server.Failover.Configurator;

public static class FailoverConfigurationHelper
{
	public const string ScriptsFolderName = "scripts";

	public const string SafekitFolderName = "safekit";

	public const string HelpTopicId = "fms_managementserverfailoverexplained";

	public const string ModuleName = "videoos";

	public const string ModuleNameNoSql = "videoosnosql";

	public const string IdpSettingsFileName = "appsettings.json";

	public const string RelativeModuleSettingsPath = "safekit\\modules\\{0}\\conf\\userconfig.xml";

	public const string RelativeModulePath = "safekit\\modules\\{0}\\";

	public const string PrimaryNodeName = "node1";

	public const string SecondaryNodeName = "node2";

	public const string DesktopShortcutIconName = "MsFailoverWebConsole.ico";

	public const string MsFailoverLogName = "ManagementServerFailover";

	public const string AppsettingsJsonFileName = "appsettings.json";

	public const string SqlDataFolderName = "DATA";

	public const string SqlLogFolderName = "Log";

	public static Uri IdpUri
	{
		get
		{
			if (!Uri.TryCreate(IdpConfiguration.Load(Path.Combine(ProductLocationHelper.IdpDirectory, "appsettings.json")).IdentityProviderSettings.Authority, UriKind.Absolute, out var result))
			{
				return null;
			}
			return result;
		}
	}

	public static string LocalFqdn
	{
		get
		{
			string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
			string text = Dns.GetHostName();
			if (!string.IsNullOrWhiteSpace(domainName) && !text.EndsWith(domainName, StringComparison.Ordinal))
			{
				text = text + "." + domainName;
			}
			return text;
		}
	}

	public static string ClusterIPAddress => SafekitModuleConfiguration.Load(Path.Combine(ProductLocationHelper.FailoverServerDirectory, string.Format(CultureInfo.InvariantCulture, "safekit\\modules\\{0}\\conf\\userconfig.xml", IsExternalSqlSetup() ? "videoosnosql" : "videoos"))).VirtualAddress;

	public static async Task<string> ValidateCertificateAuthorityConnection(ILogger logger, string machineFqdn, string securityCode)
	{
		HttpWebResponse httpWebResponse = await CheckHttpsConnectionToCertificateAuthorityAsync(machineFqdn, securityCode);
		switch (httpWebResponse?.StatusCode)
		{
		case HttpStatusCode.OK:
			return string.Empty;
		case HttpStatusCode.Unauthorized:
			logger.LogError("Connection with Certificate Authority on {MachineFqdn} failed - wrong security code.", machineFqdn);
			return Resources.FailoverWizardForm_SecurityCodeValidation_Error;
		case null:
			logger.LogError("Connection with Certificate Authority on {MachineFqdn} failed. Unknown error", machineFqdn);
			return Resources.FailoverWizardForm_NoConnection_Error;
		default:
			logger.LogError("Connection with Certificate Authority on {MachineFqdn} failed. Status code {StatusCode} ({StatusDescription}).", machineFqdn, httpWebResponse.StatusCode, httpWebResponse.StatusDescription);
			return Resources.FailoverWizardForm_NoConnection_Error;
		}
	}

	public static async Task<HttpWebResponse> CheckHttpsConnectionToCertificateAuthorityAsync(string machineFqdn, string securityCode)
	{
		return await Task.Factory.StartNew(delegate
		{
			HttpWebRequest httpWebRequest = WebRequest.CreateHttp(new UriBuilder("https", machineFqdn, 9001).ToString());
			httpWebRequest.Credentials = new NetworkCredential("CA_admin", securityCode);
			httpWebRequest.Method = "GET";
			httpWebRequest.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpWebRequest.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			try
			{
				return httpWebRequest.GetResponse() as HttpWebResponse;
			}
			catch (WebException ex)
			{
				return ex.Response as HttpWebResponse;
			}
		});
	}

	public static bool CheckIfFeatureIsLicensed(IServerAccess serverAccess)
	{
		return LicenseChecker.CheckIfFailoverFeatureIsLicensed(serverAccess, delegate
		{
		}, delegate
		{
		}, delegate
		{
		});
	}

	public static async Task<bool> CheckIfFeatureIsLicensedAsync(IServerAccess serverAccess)
	{
		return await Task.Factory.StartNew(() => CheckIfFeatureIsLicensed(serverAccess));
	}

	public static bool CheckIfFailoverIsInstalled()
	{
		return Directory.Exists(Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"));
	}

	public static async Task<bool> CheckIfFailoverIsInstalledAsync()
	{
		return await Task.Factory.StartNew(() => CheckIfFailoverIsInstalled());
	}

	public static string ConstructIdpAddress(Uri idpUri, string targetHostName)
	{
		string arg = idpUri?.Scheme;
		int? num = idpUri?.Port;
		return new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}", arg, targetHostName, num)).ToString();
	}

	public static void LogManagerOnModifyLogParameter(object sender, LogManager.LogInitializeParameterEventArgs e)
	{
		if (e != null && !(e.LogType != "File") && (!(e.Key != "Filename") || !(e.Key != "Archive")))
		{
			string text = (string)e.Value2;
			text = text.Replace("~", "").Replace("/", "\\");
			if (Path.IsPathRooted(text))
			{
				text = text.TrimStart('\\');
			}
			e.Value = Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, text);
		}
	}

	public static bool IsExternalSqlSetup()
	{
		return Directory.Exists(Path.Combine(ProductLocationHelper.FailoverServerDirectory, string.Format(CultureInfo.InvariantCulture, "safekit\\modules\\{0}\\", "videoosnosql")));
	}

	public static Guid GuidFromString(string input)
	{
		Guid empty = Guid.Empty;
		using SHA256 sHA = SHA256.Create();
		byte[] b = sHA.ComputeHash(Encoding.Default.GetBytes(input)).Take(16).ToArray();
		return new Guid(b);
	}

	public static void RestartWindowsServiceIfRunning(string serviceName)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		if (serviceController.Status == ServiceControllerStatus.Running)
		{
			TimeSpan timeout = TimeSpan.FromSeconds(10.0);
			serviceController.Stop();
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			timeout = TimeSpan.FromSeconds(10.0);
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}
	}

	public static async Task<bool> PingHostAsync(string deviceAddress, TimeSpan timeout)
	{
		return await Task.Factory.StartNew(delegate
		{
			using Ping ping = new Ping();
			PingOptions options = new PingOptions
			{
				DontFragment = true
			};
			string s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			try
			{
				return ping.Send(deviceAddress, (int)timeout.TotalMilliseconds, bytes, options).Status == IPStatus.Success;
			}
			catch (Exception)
			{
				return false;
			}
		});
	}
}
