using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseInformation : IConfigurationItem
{
	public LicenseDetailFolder LicenseDetailFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LicenseDetailFolder")
				{
					return new LicenseDetailFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ICollection<LicenseFeatureChildItem> LicenseFeatureChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LicenseFeatureChildItem> list = new List<LicenseFeatureChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LicenseFeature")
				{
					list.Add(new LicenseFeatureChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public LicenseInstalledProductFolder LicenseInstalledProductFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LicenseInstalledProductFolder")
				{
					return new LicenseInstalledProductFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public LicenseOverviewAllFolder LicenseOverviewAllFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LicenseOverviewAllFolder")
				{
					return new LicenseOverviewAllFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Slc => base.Wrapper.GetPropertyValue("Slc");

	public string Sku => base.Wrapper.GetPropertyValue("Sku");

	public string CareLevel => base.Wrapper.GetPropertyValue("CareLevel");

	public string CareId => base.Wrapper.GetPropertyValue("CareId");

	public bool ActivationAutomatic
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ActivationAutomatic");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public LicenseInformation(ServerId serverId, ConfigurationItem licenseInformation)
		: base(serverId, licenseInformation)
	{
	}

	public LicenseInformation(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseInformation(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ActivateLicense(string userName, SecureString password, bool activationAutomatic)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ActivateLicense");
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("ActivationAutomatic", activationAutomatic.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ActivateLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'ActivateLicense':" + ex.Message);
		}
		return null;
	}

	public ActivateLicenseServerTask ActivateLicense()
	{
		try
		{
			return new ActivateLicenseServerTask(base.Wrapper.ExecuteMethod("ActivateLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'ActivateLicense':" + ex.Message);
		}
		return null;
	}

	public ServerTask RequestLicense()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("RequestLicense").ExecuteMethod("RequestLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'RequestLicense':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangeLicense(string license)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeLicense");
			configurationApiItemWrapper.SetProperty("License", license);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'ChangeLicense':" + ex.Message);
		}
		return null;
	}

	public ChangeLicenseServerTask ChangeLicense()
	{
		try
		{
			return new ChangeLicenseServerTask(base.Wrapper.ExecuteMethod("ChangeLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'ChangeLicense':" + ex.Message);
		}
		return null;
	}

	public ServerTask UpdateLicense(string license)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("UpdateLicense");
			configurationApiItemWrapper.SetProperty("License", license);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("UpdateLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'UpdateLicense':" + ex.Message);
		}
		return null;
	}

	public UpdateLicenseServerTask UpdateLicense()
	{
		try
		{
			return new UpdateLicenseServerTask(base.Wrapper.ExecuteMethod("UpdateLicense"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LicenseInformation", "Unable to call method 'UpdateLicense':" + ex.Message);
		}
		return null;
	}

	public ServerTask StopAutomaticLicenseActivation()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("StopAutomaticLicenseActivation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StopAutomaticLicenseActivation", "Unable to call method 'StopAutomaticLicenseActivation':" + ex.Message);
			return null;
		}
	}
}
