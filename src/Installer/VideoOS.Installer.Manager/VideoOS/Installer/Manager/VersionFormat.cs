using System;
using VideoOS.Installer.Configuration;

namespace VideoOS.Installer.Manager;

public static class VersionFormat
{
	private static string _productKey = "default";

	private static string _displayName;

	private static string _caption;

	private static Version _version;

	private const string UninstalKey = "uninstall";

	public static string GenerateProductNameWithVersionUninstallMode()
	{
		_productKey = "uninstall";
		if (_version == null)
		{
			_version = GetVersion(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Visible);
		}
		if (_displayName == null)
		{
			GenerateDisplayName(InstallerManager.Instance.InstallerConfig.InstallerInfo.DisplayName);
		}
		return _displayName;
	}

	public static string GenerateProductNameWithVersion()
	{
		if (AllowedGetProductSku())
		{
			string text = InstallerManager.Instance.Parameters["PRODUCT-SKU"];
			if (text == _productKey)
			{
				return _displayName;
			}
			UpdateProductInfo(text);
		}
		if (_version == null)
		{
			_version = GetVersion(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Visible);
		}
		if (_displayName == null)
		{
			GenerateDisplayName(InstallerManager.Instance.InstallerConfig.InstallerInfo.DisplayName);
		}
		return _displayName;
	}

	private static bool AllowedGetProductSku()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("PRODUCT-SKU"))
		{
			return !_productKey.Equals("uninstall");
		}
		return false;
	}

	private static void GenerateDisplayName(string displayName)
	{
		if (_displayName == null)
		{
			if (_version.Major == 0)
			{
				_displayName = displayName;
			}
			else
			{
				_displayName = displayName + " " + $"{_version.Major}.{_version.Minor}{GetRevisionLetter(_version.Revision)}";
			}
		}
	}

	public static string GetNeutralProductName()
	{
		return InstallerManager.Instance.InstallerConfig.InstallerInfo.DisplayName;
	}

	public static string GetProductName()
	{
		return InstallerManager.Instance.InstallerConfig.InstallerInfo.DisplayName.ToString();
	}

	public static string GetFormCaption()
	{
		if (AllowedGetProductSku())
		{
			string text = InstallerManager.Instance.Parameters["PRODUCT-SKU"];
			if (text == _productKey)
			{
				return _caption;
			}
			UpdateProductInfo(text);
		}
		if (_caption == null)
		{
			_caption = InstallerManager.Instance.InstallerConfig.InstallerCaption;
		}
		return _caption;
	}

	private static void UpdateProductInfo(string needProdKey)
	{
		if (InstallerManager.Instance.InstallerConfig.LicenseProvider == null || InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseCustomisations == null || InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom == null || InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom.Length == 0)
		{
			return;
		}
		LicenseCustomisationsTypeLicenseCustom[] licenseCustom = InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom;
		foreach (LicenseCustomisationsTypeLicenseCustom licenseCustomisationsTypeLicenseCustom in licenseCustom)
		{
			if (licenseCustomisationsTypeLicenseCustom.ProductKey == needProdKey)
			{
				_productKey = needProdKey;
				if (licenseCustomisationsTypeLicenseCustom.DisplayVersion.IndexOf('.') == -1)
				{
					_version = new Version(TryParseToInt(licenseCustomisationsTypeLicenseCustom.DisplayVersion), 0);
				}
				else
				{
					_version = new Version(TryParseToInt(licenseCustomisationsTypeLicenseCustom.DisplayVersion.Split('.')[0]), TryParseToInt(licenseCustomisationsTypeLicenseCustom.DisplayVersion.Split('.')[1]));
				}
				_displayName = null;
				GenerateDisplayName(licenseCustomisationsTypeLicenseCustom.DisplayName);
				_caption = licenseCustomisationsTypeLicenseCustom.Caption;
			}
		}
	}

	public static Version GetProductVersion()
	{
		if (AllowedGetProductSku())
		{
			string text = InstallerManager.Instance.Parameters["PRODUCT-SKU"];
			if (text == _productKey)
			{
				return _version;
			}
			UpdateProductInfo(text);
		}
		if (_version == null)
		{
			_version = GetVersion(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Visible);
		}
		return _version;
	}

	public static Version GetVersion(bool visible)
	{
		if (!visible)
		{
			return new Version("0.0.0.0");
		}
		return new Version(TryParseToInt(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Major), TryParseToInt(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Minor), TryParseToInt(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Build), TryParseToInt(InstallerManager.Instance.InstallerConfig.InstallerInfo.Version.Revision));
	}

	private static int TryParseToInt(string number)
	{
		int result = -1;
		int.TryParse(number, out result);
		if (result != -1)
		{
			return result;
		}
		return 0;
	}

	public static string GetARPDisplayVersion()
	{
		Version productVersion = GetProductVersion();
		return $"{productVersion.Major}.{productVersion.Minor}.{productVersion.Revision}.{productVersion.Build}";
	}

	private static string GetRevisionLetter(int revision)
	{
		_ = string.Empty;
		if (revision > 0 && revision <= 25)
		{
			return Convert.ToChar(revision + 96).ToString();
		}
		return "";
	}

	private static int GetRevisionNumber(char revLetter)
	{
		return Convert.ToInt32(revLetter) - 96;
	}

	public static string GetValidVersionString(string myVersion)
	{
		int num = -1;
		int result = -1;
		int num2 = -1;
		int num3 = -1;
		if (myVersion.IndexOf(".") != -1)
		{
			string[] array = myVersion.Split('.');
			if (array.Length != 0)
			{
				num = TryParseToInt(array[0]);
			}
			if (array.Length > 1 && !int.TryParse(array[1], out result) && array[1].Length > 1)
			{
				char[] array2 = array[1].ToCharArray();
				result = TryParseToInt(array2[0].ToString());
				num2 = GetRevisionNumber(array2[1]);
			}
			if (array.Length > 2)
			{
				num3 = TryParseToInt(array[2]);
			}
			if (array.Length > 3)
			{
				num2 = TryParseToInt(array[3]);
			}
		}
		string text = ((num != -1) ? num.ToString() : myVersion);
		if (result != -1)
		{
			text = $"{text}.{result}";
		}
		if (num3 != -1)
		{
			text = $"{text}.{num3}";
		}
		if (num2 != -1)
		{
			text = $"{text}.{num2}";
		}
		return text;
	}
}
