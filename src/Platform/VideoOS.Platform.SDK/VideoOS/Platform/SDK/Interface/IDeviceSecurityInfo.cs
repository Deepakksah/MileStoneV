namespace VideoOS.Platform.SDK.Interface;

public interface IDeviceSecurityInfo
{
	bool Live { get; set; }

	bool Browse { get; set; }

	bool GetSequences { get; set; }

	bool ExportDatabase { get; set; }

	bool BookmarkView { get; set; }

	bool BookmarkAdd { get; set; }

	bool BookmarkEdit { get; set; }

	bool BookmarkDelete { get; set; }

	bool RetrieveEdgeRecordings { get; set; }

	bool RetentionCreate { get; set; }

	bool RetentionRemove { get; set; }

	bool RetentionView { get; set; }

	bool StartRecording { get; set; }

	bool StopRecording { get; set; }
}
