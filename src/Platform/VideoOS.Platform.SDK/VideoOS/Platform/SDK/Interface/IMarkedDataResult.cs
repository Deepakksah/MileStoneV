namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataResult
{
	IFaultDevice[] FaultDevices { get; set; }

	IMarkedData MarkedData { get; set; }

	IResultStatus Status { get; set; }
}
