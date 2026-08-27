namespace VideoOS.Management.Common;

public static class CameraSecurityAction
{
	public const string Read = "GENERIC_READ";

	public const string Edit = "GENERIC_WRITE";

	public const string ViewLive = "VIEW_LIVE";

	public const string ViewRestrictedLive = "VIEW_RESTRICTED_LIVE";

	public const string Playback = "PLAYBACK";

	public const string PlaybackRestrictedMedia = "PLAYBACK_RESTRICTED_RECORDINGS";

	public const string ReadRemoteRecordings = "READ_REMOTE_RECORDINGS";

	public const string ReadSequences = "READ_SEQUENCES";

	public const string SmartSearch = "SMARTSEARCH";

	public const string Export = "EXPORT";

	public const string CreateBookmarks = "CREATE_BOOKMARKS";

	public const string ReadBookmarks = "READ_BOOKMARKS";

	public const string EditBookmarks = "EDIT_BOOKMARKS";

	public const string DeleteBookmarks = "DELETE_BOOKMARKS";

	public const string CreateEvidenceLock = "CREATE_EVIDENCE_LOCK";

	public const string ReadEvidenceLock = "READ_EVIDENCE_LOCK";

	public const string DeleteEvidenceLock = "DELETE_EVIDENCE_LOCK";

	public const string CreateRestrictedMedia = "CREATE_RESTRICTIONS";

	public const string ReadRestrictedMedia = "READ_RESTRICTIONS";

	public const string DeleteRestrictedMedia = "DELETE_RESTRICTIONS";

	public const string AuxCommands = "AUX_COMMANDS";

	public const string PtzControl = "PTZ_CONTROL";

	public const string ActivatePtzPreset = "ACTIVATE_PTZ_PRESET";

	public const string ManagePresetAndPatrolling = "MANAGE_PTZ_PRESET";

	public const string ManageLockPreset = "MANAGE_LOCK_PTZ_PRESET";

	public const string ReservePtzSession = "RESERVE_PTZ_SESSION";

	public const string ReleasePtzSession = "RELEASE_PTZ_SESSION";

	public const string StartManualRecording = "START_MANUAL_RECORDING";

	public const string StopManualRecording = "STOP_MANUAL_RECORDING";

	public const string DeleteRecordings = "DELETE_RECORDINGS";

	public const string AdministrateSecurity = "ADMIN_SECURITY";

	public const string LiftPrivacyMasking = "LIFT_PRIVACY_MASKING";
}
