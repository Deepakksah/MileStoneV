using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Manager;

public class OSInformationReader : IOSInformationReader
{
	public OSInformation Read()
	{
		string text = string.Empty;
		uint num = 0u;
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
		{
			using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			try
			{
				foreach (ManagementObject item in managementObjectCollection)
				{
					text = (string)item.GetPropertyValue("Version");
					num = (uint)item.GetPropertyValue("ProductType");
				}
				if (managementObjectCollection.Count > 1)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Multiple OS Information collected, count {0}. Using last result, Version:{1}, ProductType: {2}", managementObjectCollection.Count, text, num), TraceEventType.Warning);
				}
			}
			catch (ManagementException ex)
			{
				LogManager.Write("OSInformationReader failed while querying the operating system", ex.StackTrace, TraceEventType.Error);
				return null;
			}
		}
		if (Version.TryParse(text, out var result) && num != 0)
		{
			WinVersionTypeVersionsToExcludeProductType productType = ((num == 1) ? WinVersionTypeVersionsToExcludeProductType.Workstation : WinVersionTypeVersionsToExcludeProductType.Server);
			return new OSInformation
			{
				OSVersion = result,
				ProductType = productType
			};
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "OSInformationReader failed while parsing version OS information, versionValue: {0}, productType: {1}", text, num), TraceEventType.Error);
		return null;
	}
}
