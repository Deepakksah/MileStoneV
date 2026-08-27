namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class SettingsHelper
{
	public enum SettingsMask : uint
	{
		Nothing = 0u,
		Notify = 1u,
		NotifyAlarms = 2u,
		MyAlarmsOnly = 4u,
		NotifyACRequests = 8u,
		AppDenyInApp = 0x10u,
		AppDenyPush = 0x20u
	}

	public static void SetFlag(ref uint flags, SettingsMask setting)
	{
		flags |= (uint)setting;
	}

	public static bool IsSet(uint flags, SettingsMask settings)
	{
		return (flags & (uint)settings) != 0;
	}
}
