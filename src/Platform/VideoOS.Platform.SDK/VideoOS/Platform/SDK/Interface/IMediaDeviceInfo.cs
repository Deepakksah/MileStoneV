namespace VideoOS.Platform.SDK.Interface;

public interface IMediaDeviceInfo : IDeviceInfo
{
	ITrackInfo[] Tracks { get; set; }

	bool MulticastEnabled { get; set; }

	bool EdgeStorageSupported { get; set; }

	bool EdgeStoragePlayback { get; set; }

	int StopManualRecordingSeconds { get; set; }
}
