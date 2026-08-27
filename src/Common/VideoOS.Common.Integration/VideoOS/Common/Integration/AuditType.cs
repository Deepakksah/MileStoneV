using System.CodeDom.Compiler;

namespace VideoOS.Common.Integration;

[GeneratedCode("CreateLogUserEnumerations build task", "0.0.0.0")]
public enum AuditType
{
	Unknown,
	Login,
	Logout,
	ResourceAccess,
	DatabaseAccess,
	Security,
	Management,
	LiveVideo,
	LiveAudio,
	Ptz,
	IOAndEvents,
	Export,
	Alerts,
	RecordingSequences,
	PlaybackVideo,
	SmartWallControl,
	MapViewing,
	PlaybackAudio,
	Bookmark,
	LiveMetadata,
	PlaybackMetadata,
	ManualRecording,
	SaveSearches,
	MetadataSearch
}
