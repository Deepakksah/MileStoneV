using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using VideoOS.Common.Integration;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class FreeLicenseDownloader : IFreeLicenseDownloader
{
	public const string PrimaryRegistrySubKey = "SOFTWARE\\VideoOS\\Installation\\Server";

	public async Task<LicenseJsonResponse> DownloadAsync(CancellationToken token)
	{
		string managerDownloadFreeLicenseUrl = OemResources.Instance.App.ManagerDownloadFreeLicenseUrl;
		string text = AssemblyProductInfo.Major + AssemblyProductInfo.Minor;
		string vendorCode = OemResources.Instance.App.VendorCode;
		string text2 = "XProtectEssentialPlusUser";
		string text3 = "M0hrG!xk75ia4";
		string text4 = string.Format(CultureInfo.InvariantCulture, "Essential/FreeLicenseApi.svc/GetFreeLicense/{0}/{1}/{2}/{3}", text2, text3, text, vendorCode);
		ManufacturerInfo manufacturerInfo = InstallerManager.Instance.ManufacturerInfo;
		if (manufacturerInfo != null)
		{
			text4 += string.Format(CultureInfo.InvariantCulture, "/{0}/{1}/{2}", Uri.EscapeDataString(manufacturerInfo.ManufacturerName), Uri.EscapeDataString(manufacturerInfo.Sku), manufacturerInfo.PartnerGuid);
		}
		managerDownloadFreeLicenseUrl = GetHostNameFromRegistry(managerDownloadFreeLicenseUrl);
		Uri uri = new UriBuilder(Uri.UriSchemeHttps, managerDownloadFreeLicenseUrl, 443, text4).Uri;
		try
		{
			HttpClient client = new HttpClient();
			try
			{
				using Stream stream = await (await client.GetAsync(uri, token)).Content.ReadAsStreamAsync();
				using StreamReader streamReader = new StreamReader(stream);
				JsonTextReader val = new JsonTextReader((TextReader)streamReader);
				try
				{
					LicenseJsonResponse licenseJsonResponse = new JsonSerializer().Deserialize<LicenseJsonResponse>((JsonReader)(object)val);
					ValidateData(licenseJsonResponse);
					return licenseJsonResponse;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.InnerException != null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "FreeLicenseDownloader: Failed to download license file. Exception = {0}, InnerException = {1} ", ex2.Message, ex2.InnerException.Message), TraceEventType.Error);
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "FreeLicenseDownloader: Failed to download license file. Exception = {0}, ", ex2.Message), TraceEventType.Error);
			}
			throw new DownloadingLicenseException("Downloading exception failed", ex2);
		}
	}

	private static string GetHostNameFromRegistry(string hostName)
	{
		using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\VideoOS\\Installation\\Server", writable: false);
			if (registryKey2 != null && registryKey2.GetValue("ManagerDownloadLicenseUrl") != null)
			{
				hostName = registryKey2.GetValue("ManagerDownloadLicenseUrl").ToString();
			}
		}
		return hostName;
	}

	private static void ValidateData(LicenseJsonResponse data)
	{
		if (data == null || (data.IsSuccessful && data.Data == null))
		{
			throw new DownloadingLicenseException("Downloading license failed. Invalid data");
		}
	}
}
