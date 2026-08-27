#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace VideoOS.Common.DriverResources;

public static class DriverResourcesLookup
{
	private static ResourceManager _driverResources = new ResourceManager($"{typeof(DriverResourcesLookup).Namespace}.DriversResources.drivers", Assembly.GetExecutingAssembly());

	public static ResourceManager getResourceManager()
	{
		return _driverResources;
	}

	public static string GetDisplayNameByGuid(string guid)
	{
		CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
		return GetDisplayNameByGuid(guid, currentUICulture);
	}

	public static string GetDisplayNameByGuid(string guid, string cultureName)
	{
		CultureInfo culture = null;
		if (!string.IsNullOrEmpty(cultureName))
		{
			try
			{
				culture = new CultureInfo(cultureName);
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"Culture name({cultureName}) is not supported. Error: {ex.Message}");
			}
		}
		return GetDisplayNameByGuid(guid, culture);
	}

	public static string GetDisplayNameByGuid(string guid, CultureInfo culture)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(guid))
		{
			try
			{
				result = _driverResources.GetString(guid, culture);
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"Failed to look up driver resource by guid({guid}). Error: {ex.Message} ");
			}
		}
		return result;
	}
}
