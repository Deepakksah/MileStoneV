namespace VideoOS.Platform.SDK.Config;

public interface IDeviceSecurity
{
	bool Live { get; }

	bool Browse { get; }

	bool GetSequences { get; }

	bool ExportDatabase { get; }

	bool BookmarkAdd { get; }

	bool BookmarkDelete { get; }

	bool BookmarkEdit { get; }

	bool BookmarkView { get; }

	bool RetentionCreate { get; }

	bool RetentionRemove { get; }

	bool RetentionView { get; }

	bool StartRecording { get; }

	bool RetrieveEdgeRecordings { get; }
}
