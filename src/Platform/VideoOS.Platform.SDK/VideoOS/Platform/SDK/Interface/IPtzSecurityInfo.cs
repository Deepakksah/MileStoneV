namespace VideoOS.Platform.SDK.Interface;

public interface IPtzSecurityInfo
{
	bool ManualControl { get; set; }

	bool PresetControl { get; set; }

	bool ReserveControl { get; set; }
}
