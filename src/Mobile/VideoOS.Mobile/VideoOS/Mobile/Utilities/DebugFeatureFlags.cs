using System;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Utilities;

public class DebugFeatureFlags
{
	public static DebugFeatureFlags Instance = new DebugFeatureFlags();

	public bool AbsEnabled { get; private set; }

	public bool MobileEnabled { get; private set; }

	public bool TwoStepsVerificationEnabled { get; private set; }

	public bool PushEnabled { get; private set; }

	public bool FootagesEnabled { get; private set; }

	public bool SegmentationEnabled { get; private set; }

	public bool DirectStreamingEnabled { get; set; }

	public bool AdaptiveStreamingEnabled { get; set; }

	public bool SmartMapEnabled { get; set; }

	public bool AllowGetRegisteredDevices { get; set; }

	private DebugFeatureFlags()
	{
		AbsEnabled = false;
		MobileEnabled = false;
		TwoStepsVerificationEnabled = false;
		FootagesEnabled = false;
		PushEnabled = false;
		SegmentationEnabled = false;
		DirectStreamingEnabled = false;
		AdaptiveStreamingEnabled = false;
		SmartMapEnabled = false;
		AllowGetRegisteredDevices = false;
	}

	private void ReadRegistry()
	{
		try
		{
			using ModifyRegistry modifyRegistry = RegistryHelper.Instance.GetExplicitRegistryEntry(RegistryHelper.Instance.SubKeyName);
			string value = modifyRegistry.Read("AbsEnabled");
			AbsEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("MobileEnabled");
			MobileEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("TwoStepsVerificationEnabled");
			TwoStepsVerificationEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("FootagesEnabled");
			FootagesEnabled = !string.IsNullOrEmpty(value);
			value = modifyRegistry.Read("PushEnabled");
			PushEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("SegmentationEnabled");
			SegmentationEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("DirectStreamingEnabled");
			DirectStreamingEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("AdaptiveStreamingEnabled");
			AdaptiveStreamingEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("SmartMapEnabled");
			SmartMapEnabled = !string.IsNullOrWhiteSpace(value);
			value = modifyRegistry.Read("AllowGetRegisteredDevices");
			AllowGetRegisteredDevices = !string.IsNullOrWhiteSpace(value);
		}
		catch (Exception)
		{
		}
	}
}
