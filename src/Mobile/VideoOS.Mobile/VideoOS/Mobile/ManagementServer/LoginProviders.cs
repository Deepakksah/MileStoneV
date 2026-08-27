using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Mobile.ManagementServer;

public class LoginProviders
{
	public static ICollection<LoginProvider> GetLoginProviders()
	{
		try
		{
			return new VideoOS.Platform.ConfigurationItems.ManagementServer(EnvironmentManager.Instance.MasterSite).LoginProviderFolder.LoginProviders;
		}
		catch (Exception ex)
		{
			Log.Error("LoginProviders.GetLoginProviders", ex.ToString());
			return null;
		}
	}
}
