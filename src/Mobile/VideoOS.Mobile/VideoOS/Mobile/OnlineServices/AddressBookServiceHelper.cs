using System;
using System.Collections.Generic;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.SDK.Portable.OnlineService;

namespace VideoOS.Mobile.OnlineServices;

public static class AddressBookServiceHelper
{
	public static void InitFromConfigSettings(this IAddressBookService addressBookService, Dictionary<string, string> settings)
	{
		Log.TraceBegin("InitFromConfigSettings", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\OnlineServices\\AddressBookServiceHelper.cs", 13);
		try
		{
			if (settings != null && settings.ContainsKey(Definitions.SmartConnectEnabledName))
			{
				addressBookService.Enabled = settings[Definitions.SmartConnectEnabledName] == "True";
			}
			else
			{
				addressBookService.Enabled = true;
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		Log.TraceEnd("InitFromConfigSettings", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\OnlineServices\\AddressBookServiceHelper.cs", 31);
	}
}
