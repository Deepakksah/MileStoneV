using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common;

public static class GlobalData
{
	public static class Constants
	{
		public const string ExistingDatabaseName = "Surveillance";
	}

	public static class InstallationRegistryPath
	{
		private const string _unformattedInstallationRegPath = "SOFTWARE\\{0}\\Installation\\{1}";

		private static string _installationRegirstryPath;

		private static string _productId;

		public static string InstallationRegirstryPath => _installationRegirstryPath;

		public static string ProductId => _productId;

		public static void SetInstallationRegirstryPath(string id)
		{
			_productId = id;
			_installationRegirstryPath = string.Format(CultureInfo.CurrentCulture, "SOFTWARE\\{0}\\Installation\\{1}", OemHelper.CompanyName, id);
		}
	}

	private static Lazy<OemHelper> _oemHelper;

	public static List<IPackageParameter> Settings { get; set; }

	public static OemHelper OemHelper => _oemHelper.Value;

	public static string PublisherKey => Path.Combine("SOFTWARE", OemHelper.CompanyName);

	public static string ManufacturerInfoReg => Path.Combine(PublisherKey, "Installation\\ManufacturerInfo");

	static GlobalData()
	{
		_oemHelper = new Lazy<OemHelper>(() => new OemHelper());
		Settings = new List<IPackageParameter>();
	}

	public static Dictionary<string, string> ToDict()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (IPackageParameter setting in Settings)
		{
			dictionary[setting.Target] = setting.Value;
		}
		return dictionary;
	}

	public static bool HasParameter(string paramName)
	{
		return Settings.Any((IPackageParameter p) => p.Target.Equals(paramName, StringComparison.OrdinalIgnoreCase));
	}

	public static bool TryGetValue(string paramName, out string value)
	{
		IPackageParameter packageParameter = Settings.FirstOrDefault((IPackageParameter p) => p.Target.Equals(paramName, StringComparison.OrdinalIgnoreCase));
		value = packageParameter?.Value ?? string.Empty;
		return packageParameter != null;
	}

	public static bool GetValueAsBoolean(string paramName)
	{
		IPackageParameter packageParameter = Settings.FirstOrDefault((IPackageParameter p) => p.Target.Equals(paramName, StringComparison.OrdinalIgnoreCase));
		bool result = false;
		if (!bool.TryParse(packageParameter?.Value, out result))
		{
			return false;
		}
		return result;
	}

	public static bool? GetValueAsNullableBoolean(string paramName)
	{
		IPackageParameter packageParameter = Settings.FirstOrDefault((IPackageParameter p) => p.Target.Equals(paramName, StringComparison.OrdinalIgnoreCase));
		if (packageParameter == null || string.IsNullOrEmpty(packageParameter.Value))
		{
			return null;
		}
		bool result;
		return bool.TryParse(packageParameter?.Value, out result) && result;
	}

	public static void SetValue(string paramName, string value, string location)
	{
		string text = ((paramName.ToLower().Contains("password") || paramName.ToLower().Contains("secret") || paramName.Equals("ENRYPTED_SLC")) ? "****" : value);
		text = (new string[4] { "ScannedHardware", "SelectedHardware", "AddedHardware", "ConfiguredHardware" }.Any((string c) => paramName.Contains(c)) ? "" : text);
		foreach (IPackageParameter setting in Settings)
		{
			if (setting.Target.Equals(paramName, StringComparison.OrdinalIgnoreCase))
			{
				setting.Value = value;
				LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Parameter '{0}' was changed to '{1}' from '{2}' ", paramName, text, location), TraceEventType.Information);
				return;
			}
		}
		LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Parameter '{0}' was added with value '{1}' from '{2}' ", paramName, text, location), TraceEventType.Information);
		Settings.Add(new PackageParameter(paramName, value));
	}

	public static void RemoveValue(string paramName)
	{
		foreach (IPackageParameter setting in Settings)
		{
			if (setting.Target == paramName)
			{
				Settings.Remove(setting);
				break;
			}
		}
	}
}
