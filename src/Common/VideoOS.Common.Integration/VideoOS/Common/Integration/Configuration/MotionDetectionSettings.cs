using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class MotionDetectionSettings
{
	public const int PixCountRelativeMax = 100000;

	public const string MotionPixCountThresholdKey = "MotionPixCountThreshold";

	public const string MotionPixThresholdKey = "MotionPixThreshold";

	public const string PixelStepKey = "PixelStep";

	public const string KeyframesOnlyKey = "KeyFramesOnly";

	public const string ManualSensitivityKey = "ManualSensitivity";

	public const string GenerateSearchDataKey = "GenerateSearchData";

	public const string TimeThresholdKey = "TimeThresholdMs";

	public const string UseRegionsKey = "UseRegions";

	public const string EnabledKey = "Enabled";

	public const string RegionSettingsKey = "RegionSettings";

	public const string HardwareAccelerationKey = "HardwareAccelerationMode";

	public const string AllowNvidiaKey = "AllowNvidia";

	public const string ConfigSectionKey = "config";

	private bool xml2ConfigSettingsChanged;

	private Dictionary<string, Action<string>> LoadActions;

	public bool SettingsLoaded { get; set; }

	public Settings Settings { get; private set; }

	public FillableGridArea MotionExcludedRegions { get; set; }

	public bool Enabled { get; set; }

	public int TimeThresholdMs { get; set; }

	public int MotionPixThreshold { get; set; }

	public int MotionPixCountThreshold { get; set; }

	public int PixelStep { get; set; }

	public bool KeyframesOnly { get; set; }

	public bool ManualSensitivity { get; set; }

	public bool GenerateSearchData { get; set; }

	public bool UseRegions { get; set; }

	public bool AllowNvidiaDecoding { get; set; }

	public HardwareAccelerationMode.HardwareAccelerationModeType HardwareAcceleration { get; set; }

	public MotionDetectionSettings()
	{
		Settings = new Settings();
		MotionExcludedRegions = new FillableGridArea(0, 0);
		Initialize();
	}

	public void Initialize()
	{
		TimeThresholdMs = 500;
		KeyframesOnly = true;
		ManualSensitivity = true;
		GenerateSearchData = false;
		MotionPixThreshold = 100;
		MotionPixCountThreshold = 2000;
		PixelStep = 3;
		UseRegions = false;
		Enabled = true;
		HardwareAcceleration = HardwareAccelerationMode.HardwareAccelerationModeType.Automatic;
		AllowNvidiaDecoding = false;
		SettingsLoaded = false;
		Settings.Clear();
		LoadActions = new Dictionary<string, Action<string>>
		{
			{
				"MotionPixCountThreshold",
				delegate(string val)
				{
					MotionPixCountThreshold = Convert.ToInt32(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"MotionPixThreshold",
				delegate(string val)
				{
					MotionPixThreshold = Convert.ToInt32(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"PixelStep",
				delegate(string val)
				{
					PixelStep = Convert.ToInt32(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"TimeThresholdMs",
				delegate(string val)
				{
					TimeThresholdMs = Convert.ToInt32(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"KeyFramesOnly",
				delegate(string val)
				{
					KeyframesOnly = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"ManualSensitivity",
				delegate(string val)
				{
					ManualSensitivity = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"GenerateSearchData",
				delegate(string val)
				{
					GenerateSearchData = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"UseRegions",
				delegate(string val)
				{
					UseRegions = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"Enabled",
				delegate(string val)
				{
					Enabled = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"AllowNvidia",
				delegate(string val)
				{
					AllowNvidiaDecoding = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
				}
			},
			{
				"RegionSettings",
				delegate(string val)
				{
					MotionExcludedRegions.LoadGridAreaFromXml(val);
				}
			},
			{
				"HardwareAccelerationMode",
				delegate(string val)
				{
					HardwareAcceleration = StringToHardwareAccelerationMode(val);
				}
			}
		};
	}

	public bool Xml2Config(string xmlConfig)
	{
		xml2ConfigSettingsChanged = false;
		Settings.SettingsChanged += SettingsChanged;
		Settings.LoadXml(xmlConfig, SettingsLoaded ? SettingsLoadMethod.Update : SettingsLoadMethod.Reload);
		SettingsLoaded = true;
		Settings.SettingsChanged -= SettingsChanged;
		return xml2ConfigSettingsChanged;
	}

	private void SettingsChanged(object sender, SettingsEventArgs e)
	{
		for (int i = 0; i < e.SettingsEvents.Length; i++)
		{
			string name = e.SettingsEvents[i].Name;
			string valueAfter = e.SettingsEvents[i].ValueAfter;
			if (LoadActions.ContainsKey(name))
			{
				LoadActions[name](valueAfter);
			}
		}
		xml2ConfigSettingsChanged = true;
	}

	private static HardwareAccelerationMode.HardwareAccelerationModeType StringToHardwareAccelerationMode(string settingValue)
	{
		if (Enum.IsDefined(typeof(HardwareAccelerationMode.HardwareAccelerationModeType), settingValue))
		{
			return (HardwareAccelerationMode.HardwareAccelerationModeType)Enum.Parse(typeof(HardwareAccelerationMode.HardwareAccelerationModeType), settingValue);
		}
		return HardwareAccelerationMode.HardwareAccelerationModeType.Automatic;
	}

	public string Config2Xml()
	{
		Settings.Set("MotionPixCountThreshold", Convert.ToString(MotionPixCountThreshold, CultureInfo.InvariantCulture));
		Settings.Set("MotionPixThreshold", Convert.ToString(MotionPixThreshold, CultureInfo.InvariantCulture));
		Settings.Set("PixelStep", Convert.ToString(PixelStep, CultureInfo.InvariantCulture));
		Settings.Set("TimeThresholdMs", Convert.ToString(TimeThresholdMs, CultureInfo.InvariantCulture));
		Settings.Set("KeyFramesOnly", Convert.ToString(KeyframesOnly, CultureInfo.InvariantCulture));
		Settings.Set("ManualSensitivity", Convert.ToString(ManualSensitivity, CultureInfo.InvariantCulture));
		Settings.Set("GenerateSearchData", Convert.ToString(GenerateSearchData, CultureInfo.InvariantCulture));
		Settings.Set("UseRegions", Convert.ToString(UseRegions, CultureInfo.InvariantCulture));
		Settings.Set("Enabled", Convert.ToString(Enabled, CultureInfo.InvariantCulture));
		Settings.Set("RegionSettings", MotionExcludedRegions.GetGridAreaAsXml());
		Settings.Set("HardwareAccelerationMode", Convert.ToString(HardwareAcceleration, CultureInfo.InvariantCulture));
		Settings.Set("AllowNvidia", Convert.ToString(AllowNvidiaDecoding, CultureInfo.InvariantCulture));
		return Settings.SaveAsXml();
	}
}
