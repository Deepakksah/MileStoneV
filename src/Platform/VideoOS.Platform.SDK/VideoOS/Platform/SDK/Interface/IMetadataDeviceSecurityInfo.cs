namespace VideoOS.Platform.SDK.Interface;

public interface IMetadataDeviceSecurityInfo
{
	bool Browse { get; set; }

	bool ExportDatabase { get; set; }

	bool GetSequences { get; set; }

	bool Live { get; set; }

	bool RetrieveEdgeRecordings { get; set; }

	bool RetentionCreate { get; set; }

	bool RetentionRemove { get; set; }

	bool RetentionView { get; set; }

	bool StartRecording { get; set; }

	bool StopRecording { get; set; }
}
