using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;
using VideoOS.Platform.SDK;
using VideoOS.Platform.SDK.Export;
using VideoOS.Platform.SDK.Log;
using VideoOS.Platform.SDK.Media;

namespace VideoOS.Mobile.LoginThread;

public class MipSdkWrapper : IMipSdkWraper
{
	private readonly IMipConfig _mipSdkConfig;

	public ILoginData LoginData { get; private set; }

	public ILoginConfiguration LoginConfiguration { get; private set; }

	public ILoginConfigSpecific LoginConfigSpecific { get; private set; }

	public MipSdkWrapper(ILoginData data, ILoginConfiguration configuration, ILoginConfigSpecific configSpecific, IMipConfig mipSdkSpecificConfig = null)
	{
		LoginData = data;
		LoginConfiguration = configuration;
		LoginConfigSpecific = configSpecific;
		_mipSdkConfig = mipSdkSpecificConfig;
	}

	public bool InitializeMipMultiUser()
	{
		Log.TraceBegin("InitializeMipMultiUser", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 41);
		try
		{
			MultiUserEnvironment.InitializeUsingUserContext(LoginData.IsSecured, LoginData.ServerUri, LoginData.Username, LoginData.Password, LoginData.AdUser, _mipSdkConfig.MipSdkLogFolderPath);
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkWrapper.InitializeMipMultiUser - " + ex.GetType(), ex.Message);
		}
		Log.TraceEnd("InitializeMipMultiUser", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 59);
		return MultiUserEnvironment.InitializeLoggedIn;
	}

	public void UnInitializeMipMultiUser()
	{
		Log.TraceBegin("UnInitializeMipMultiUser", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 66);
		try
		{
			MultiUserEnvironment.UnInitialize();
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkWrapper.UnInitializeMipMultiUser - " + ex.GetType(), ex.Message);
		}
		Log.TraceEnd("UnInitializeMipMultiUser", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 78);
	}

	public void InitializeMipSdk()
	{
		Log.TraceBegin("InitializeMipSdk", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 83);
		try
		{
			VideoOS.Platform.SDK.Media.Environment.Initialize();
			VideoOS.Platform.SDK.Export.Environment.Initialize();
			VideoOS.Platform.SDK.Log.Environment.Initialize();
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkWrapper.InitializeMipSdk - " + ex.GetType(), ex.Message);
		}
		Log.TraceEnd("InitializeMipSdk", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\MipSdkWrapper.cs", 96);
	}

	public Item GetSiteItem()
	{
		Item result = null;
		try
		{
			VideoOS.Platform.Configuration.Instance.GetItems();
			if ((object)EnvironmentManager.Instance.MasterSite != null)
			{
				result = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
			}
		}
		catch (Exception ex)
		{
			Log.Error("MipAskWrapper.GetSiteItem()", ex.Message);
		}
		return result;
	}

	public List<Item> GetConfigurations(Guid pluginId, Guid itemKind)
	{
		List<Item> result = null;
		try
		{
			VideoOS.Platform.Configuration.Instance.RefreshConfiguration(pluginId);
			result = VideoOS.Platform.Configuration.Instance.GetItemConfigurations(pluginId, null, itemKind);
		}
		catch (Exception ex)
		{
			Log.Error("MipAskWrapper.GetConfigurations()", ex.Message);
		}
		return result;
	}

	public List<Item> GetConfigurations()
	{
		return GetConfigurations(LoginConfigSpecific.ServerPluginId, LoginConfigSpecific.ServerItemType);
	}

	public Item GetCurrentServerConfiguration()
	{
		List<Item> configurations = GetConfigurations();
		Item item = null;
		CheckSettings(configurations);
		try
		{
			item = configurations.FirstOrDefault(delegate(Item item2)
			{
				try
				{
					return new Guid(item2.Properties["ServerId"]) == LoginConfiguration.ServerIdentificationId;
				}
				catch
				{
				}
				return false;
			});
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkWrapper.GetCurrentServerConfiguration()", ex.Message);
		}
		if (item == null)
		{
			Log.Warning("MipSdkWrapper.GetCurrentServerConfiguration()", "Settings not found. Default settings will be used.");
		}
		else
		{
			Log.Info("MipSdkWrapper.GetCurrentServerConfiguration()", "Settings has been found.");
		}
		return item;
	}

	private void CheckSettings(IEnumerable<Item> settingsList)
	{
		try
		{
			Log.Info("MipSdkWrapper.CheckSettings()", "Number of read configurations: " + (settingsList?.Count() ?? 0));
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public void SaveUpdatedItemConfiguration(Item item, bool updateConfigurationId)
	{
		MipSdkWrapperExt.SaveUpdatedItemConfiguration(Definitions.MobilePluginId, item, updateConfigurationId);
	}
}
