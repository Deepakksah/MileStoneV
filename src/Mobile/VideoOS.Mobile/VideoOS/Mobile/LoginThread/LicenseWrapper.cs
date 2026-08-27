using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Utilities;
using VideoOS.Platform;

namespace VideoOS.Mobile.LoginThread;

public class LicenseWrapper
{
	public const string TwoStepVerification = "TwoStepVerification";

	public const string ManageAlarms = "ManageAlarms";

	public const string PreventMobileLogin = "PreventMobileLogin";

	public const string MobileLogin = "MobileLogin";

	public const string Online = "Online";

	public const string OnlineExpiration = "OnlineExpiration";

	public const string DirectStreaming = "DirectStreaming";

	public const string AdaptiveStreaming = "AdaptiveStreaming";

	public const string Bookmarking = "Bookmarking";

	public const string SmartMap = "SmartMap";

	public static readonly LicenseWrapper Instance = new LicenseWrapper();

	private LicenseWrapper()
	{
	}

	public bool CheckPreventMobileLogin()
	{
		bool result = EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("PreventMobileLogin");
		Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "PreventMobileLogin is " + result);
		return result;
	}

	public bool CheckMobileLogin()
	{
		bool flag = EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("MobileLogin");
		Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "Login is " + flag);
		return flag | DebugFeatureFlags.Instance.MobileEnabled;
	}

	public bool CheckBookmarks()
	{
		bool result = EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("Bookmarking");
		Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "CheckBookmarks is " + result);
		return result;
	}

	public bool CheckTwoStepVerification(bool log = true)
	{
		bool result = EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("TwoStepVerification");
		if (log)
		{
			Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "CheckTwoStepVerification is " + result);
		}
		return result;
	}

	public bool CheckAlarms(bool log = true)
	{
		bool result = EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("ManageAlarms");
		if (log)
		{
			Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "ManageAlarms is " + result);
		}
		return result;
	}

	private bool LicenseNotExpired()
	{
		bool flag = false;
		Dictionary<string, DateTime> expirationDateTimes = EnvironmentManager.Instance.SystemLicense.ExpirationDateTimes;
		if (expirationDateTimes != null && expirationDateTimes.ContainsKey("OnlineExpiration"))
		{
			flag |= expirationDateTimes["OnlineExpiration"] > DateTime.UtcNow;
		}
		Logger.Instance.Log(LogContent.Info, "LicenseWrapper", "OnlineExpiration is " + flag);
		return flag;
	}

	public bool SmartConnectEnabled()
	{
		return LicenseNotExpired() | DebugFeatureFlags.Instance.AbsEnabled;
	}

	public bool PushNotificationsEnabled()
	{
		return LicenseNotExpired() | DebugFeatureFlags.Instance.PushEnabled;
	}

	public bool DirectStreamingEnabled()
	{
		if (!EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("DirectStreaming"))
		{
			return DebugFeatureFlags.Instance.DirectStreamingEnabled;
		}
		return true;
	}

	public bool AdaptiveStreamingEnabled()
	{
		if (!EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("AdaptiveStreaming"))
		{
			return DebugFeatureFlags.Instance.AdaptiveStreamingEnabled;
		}
		return true;
	}

	public bool SmartMapEnabled()
	{
		if (!EnvironmentManager.Instance.SystemLicense.FeatureFlags.Contains("SmartMap"))
		{
			return DebugFeatureFlags.Instance.SmartMapEnabled;
		}
		return true;
	}
}
