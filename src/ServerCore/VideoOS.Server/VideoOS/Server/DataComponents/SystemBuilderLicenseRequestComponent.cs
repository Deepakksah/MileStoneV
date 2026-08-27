using System;
using System.IO;
using Microsoft.Win32;
using VideoOS.Common.Integration;

namespace VideoOS.Server.DataComponents;

public sealed class SystemBuilderLicenseRequestComponent : ISystemBuilderLicenseRequestComponent, IDisposable
{
	private const string ManufacturerName = "ManufacturerName";

	private const string SKU = "SKU";

	private const string PartnerGuid = "PartnerGuid";

	private static string PublisherKey => Path.Combine("SOFTWARE", OemResources.Instance.App.CompanyName);

	private static string ManufacturerInfoReg => Path.Combine(PublisherKey, "Installation\\ManufacturerInfo");

	public void Dispose()
	{
	}

	public SystemBuilderLicenseRequest GetSystemBuilderLicenseRequest()
	{
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(ManufacturerInfoReg))
		{
			if (registryKey != null)
			{
				SystemBuilderLicenseRequest systemBuilderLicenseRequest = new SystemBuilderLicenseRequest();
				systemBuilderLicenseRequest.Name = registryKey.GetValue("ManufacturerName") as string;
				systemBuilderLicenseRequest.Sku = registryKey.GetValue("SKU") as string;
				string input = registryKey.GetValue("PartnerGuid") as string;
				if (Guid.TryParse(input, out var result))
				{
					systemBuilderLicenseRequest.PartnerGuid = result;
				}
				return systemBuilderLicenseRequest;
			}
		}
		return null;
	}
}
