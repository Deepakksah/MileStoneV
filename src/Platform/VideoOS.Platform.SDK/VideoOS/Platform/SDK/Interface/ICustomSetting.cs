namespace VideoOS.Platform.SDK.Interface;

public interface ICustomSetting
{
	ICustomSettingInfo CustomSettingInfo { get; set; }

	ICustomSettingData CustomSettingData { get; set; }
}
