using System;
using System.IO;

namespace VideoOS.LPR.Common.OEM;

public static class OemResources
{
	private static OemResourcePackageHelper _resourcePackage = new OemResourcePackageHelper(useGlobalResourceFile: false);

	public static string CompanyNameShort => _resourcePackage.GetResourceString("CompanyNameShort");

	public static string LPRProductNameShort => _resourcePackage.GetResourceString("LPRProductNameShort");

	public static string DataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyNameShort, LPRProductNameShort);

	public static string LPRProductName => _resourcePackage.GetResourceString("LPRProductName");

	public static string LPRProductNameFull => _resourcePackage.GetResourceString("LPRProductNameFull");

	public static string LPRServiceName => _resourcePackage.GetResourceString("LPRServiceName");

	public static void SetCustomOemResourcePackageHelper(OemResourcePackageHelper oemResourcePackageHelper)
	{
		_resourcePackage = oemResourcePackageHelper;
	}
}
