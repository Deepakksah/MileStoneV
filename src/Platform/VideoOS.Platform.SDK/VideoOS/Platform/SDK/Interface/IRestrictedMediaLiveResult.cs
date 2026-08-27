namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveResult
{
	IRestrictedMediaLive[] RestrictedMedia { get; set; }

	IFaultDevice[] FaultDevices { get; set; }

	RestrictedMediaResultStatus Status { get; set; }

	IWarningDevice[] WarningDevices { get; set; }
}
