using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ScanResults
{
	public ScanErrorCode ErrorCode { get; set; }

	public string Result { get; set; }

	public ScanResults()
	{
		ErrorCode = ScanErrorCode.NoError;
		Result = string.Empty;
	}

	public ScanResults(ScanErrorCode errorCode, string result)
	{
		ErrorCode = errorCode;
		Result = result;
	}

	public static string GetErrorMessageFromCode(ScanErrorCode errorCode)
	{
		return errorCode switch
		{
			ScanErrorCode.Timeout => Resources.ScanPageTimeOutError, 
			ScanErrorCode.Generic => Resources.ScanPageGenericError, 
			ScanErrorCode.NoHardwareFound => Resources.HardwareSelectionNoHardwareMessage, 
			ScanErrorCode.AlreadyRunning => Resources.ScanAlreadyRunning, 
			_ => string.Empty, 
		};
	}
}
