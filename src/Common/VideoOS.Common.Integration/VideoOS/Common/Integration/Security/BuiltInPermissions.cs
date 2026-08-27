namespace VideoOS.Common.Integration.Security;

public static class BuiltInPermissions
{
	public static class DeviceSecurity
	{
		public static class Camera
		{
			public const string Visible = "DEVICEVISIBLE";

			public const string Live = "DEVICELIVEVIEW";

			public const string LiveRestricted = "DEVICELIVEVIEWRESTRICTED";

			public const string Recorded = "DEVICERECORDEDBROWSE";

			public const string RecordedRestricted = "DEVICERECORDEDBROWSERESTRICTED";

			public const string RestrictedMediaView = "DEVICEREADRESTRICTIONS";

			public const string RestrictedMediaCreate = "DEVICERESTRICTIONCREATE";

			public const string RestrictedMediaDelete = "DEVICERESTRICTIONREMOVE";

			public const string RecordedExport = "DEVICERECORDEDEXPORT";

			public const string RecordedSmartSearch = "DEVICERECORDEDSMARTSEARCH";

			public const string RecordedGetSequences = "DEVICERECORDEDGETSEQUENCES";

			public const string BookmarkView = "DEVICEBOOKMARKVIEW";

			public const string BookmarkAdd = "DEVICEBOOKMARKADD";

			public const string BookmarkEdit = "DEVICEBOOKMARKEDIT";

			public const string BookmarkDelete = "DEVICEBOOKMARKDELETE";

			public const string MarkedDataView = "DEVICEMARKEDDATAVIEW";

			public const string MarkedDataCreate = "DEVICEMARKEDDATACREATE";

			public const string MarkedDataRemove = "DEVICEMARKEDDATAREMOVE";

			public const string PtzAux = "DEVICEPTZAUX";

			public const string RetrieveEdgeRecordings = "DEVICERETRIEVEEDGERECORDINGS";

			public const string ManualRecordingStart = "DEVICEMANUALRECORDINGSTART";

			public const string ManualRecordingStop = "DEVICEMANUALRECORDINGSTOP";
		}

		public static class Input
		{
			public const string Visible = "DEVICEINPUTVISIBLE";

			public const string ReadValue = "DEVICEREADINPUT";
		}

		public static class Output
		{
			public const string Visible = "DEVICEOUTPUTVISIBLE";

			public const string Activate = "DEVICEACTIVEOUTPUT";
		}

		public static class Microphone
		{
			public const string Visible = "DEVICEMICVISIBLE";

			public const string Live = "DEVICEMICLIVEVIEW";

			public const string LiveRestricted = "DEVICEMICLIVEVIEWRESTRICTED";

			public const string Recorded = "DEVICEMICRECORDEDBROWSE";

			public const string RecordedRestricted = "DEVICEMICRECORDEDBROWSERESTRICTED";

			public const string RestrictedMediaView = "DEVICEMICREADRESTRICTIONS";

			public const string RestrictedMediaCreate = "DEVICEMICRESTRICTIONCREATE";

			public const string RestrictedMediaDelete = "DEVICEMICRESTRICTIONREMOVE";

			public const string RecordedExport = "DEVICEMICRECORDEDEXPORT";

			public const string RecordedGetSequences = "DEVICEMICRECORDEDGETSEQUENCES";

			public const string BookmarkView = "DEVICEMICBOOKMARKVIEW";

			public const string BookmarkAdd = "DEVICEMICBOOKMARKADD";

			public const string BookmarkEdit = "DEVICEMICBOOKMARKEDIT";

			public const string BookmarkDelete = "DEVICEMICBOOKMARKDELETE";

			public const string MarkedDataView = "DEVICEMICMARKEDDATAVIEW";

			public const string MarkedDataCreate = "DEVICEMICMARKEDDATACREATE";

			public const string MarkedDataRemove = "DEVICEMICMARKEDDATAREMOVE";

			public const string RetrieveEdgeRecordings = "DEVICEMICRETRIEVEEDGERECORDINGS";

			public const string ManualRecordingStart = "DEVICEMICMANUALRECORDINGSTART";

			public const string ManualRecordingStop = "DEVICEMICMANUALRECORDINGSTOP";
		}

		public static class Speaker
		{
			public const string Visible = "DEVICESPEAKERVISIBLE";

			public const string Live = "DEVICESPEAKERLIVEVIEW";

			public const string LiveRestricted = "DEVICESPEAKERLIVEVIEWRESTRICTED";

			public const string Recorded = "DEVICESPEAKERRECORDEDBROWSE";

			public const string RecordedRestricted = "DEVICESPEAKERRECORDEDBROWSERESTRICTED";

			public const string RestrictedMediaView = "DEVICESPEAKERREADRESTRICTIONS";

			public const string RestrictedMediaCreate = "DEVICESPEAKERRESTRICTIONCREATE";

			public const string RestrictedMediaDelete = "DEVICESPEAKERRESTRICTIONREMOVE";

			public const string RecordedExport = "DEVICESPEAKERRECORDEDEXPORT";

			public const string RecordedGetSequences = "DEVICESPEAKERRECORDEDGETSEQUENCES";

			public const string BookmarkView = "DEVICESPEAKERBOOKMARKVIEW";

			public const string BookmarkAdd = "DEVICESPEAKERBOOKMARKADD";

			public const string BookmarkEdit = "DEVICESPEAKERBOOKMARKEDIT";

			public const string BookmarkDelete = "DEVICESPEAKERBOOKMARKDELETE";

			public const string MarkedDataView = "DEVICESPEAKERMARKEDDATAVIEW";

			public const string MarkedDataCreate = "DEVICESPEAKERMARKEDDATACREATE";

			public const string MarkedDataRemove = "DEVICESPEAKERMARKEDDATAREMOVE";

			public const string RetrieveEdgeRecordings = "DEVICESPEAKERRETRIEVEEDGERECORDINGS";

			public const string ManualRecordingStart = "DEVICESPEAKERMANUALRECORDINGSTART";

			public const string ManualRecordingStop = "DEVICESPEAKERMANUALRECORDINGSTOP";
		}

		public static class Metadata
		{
			public const string Visible = "DEVICEMETADATAVISIBLE";

			public const string Live = "DEVICEMETADATALIVEVIEW";

			public const string LiveRestricted = "DEVICEMETADATALIVEVIEWRESTRICTED";

			public const string Recorded = "DEVICEMETADATARECORDEDBROWSE";

			public const string RecordedRestricted = "DEVICEMETADATARECORDEDBROWSERESTRICTED";

			public const string RestrictedMediaView = "DEVICEMETADATAREADRESTRICTIONS";

			public const string RestrictedMediaCreate = "DEVICEMETADATARESTRICTIONCREATE";

			public const string RestrictedMediaDelete = "DEVICEMETADATARESTRICTIONREMOVE";

			public const string RecordedExport = "DEVICEMETADATARECORDEDEXPORT";

			public const string RecordedGetSequences = "DEVICEMETADATARECORDEDGETSEQUENCES";

			public const string RetrieveEdgeRecordings = "DEVICEMETADATARETRIEVEEDGERECORDINGS";

			public const string ManualRecordingStart = "DEVICEMETADATAMANUALRECORDINGSTART";

			public const string ManualRecordingStop = "DEVICEMETADATAMANUALRECORDINGSTOP";

			public const string MarkedDataView = "DEVICEMETADATAMARKEDDATAVIEW";

			public const string MarkedDataCreate = "DEVICEMETADATAMARKEDDATACREATE";

			public const string MarkedDataRemove = "DEVICEMETADATAMARKEDDATAREMOVE";
		}
	}

	public static class Speak
	{
		public const string SpeakControl = "DEVICESPEAKCONTROL";

		public const string SpeakPriority = "DEVICESPEAKPRIORITY";
	}

	public static class Ptz
	{
		public const string ManualPtzControl = "DEVICEPTZCONTROL";

		public const string PresetPtzControl = "DEVICEPTZPRESETS";

		public const string PtzPriority = "DEVICEPTZPRIORITY";

		public const string PresetPtzManage = "DEVICEPTZMANAGE";

		public const string PresetPtzManageLock = "DEVICEPTZMANAGELOCK";

		public const string PtzReserveControl = "DEVICEPTZRESERVESESSION";

		public const string PtzReleaseControl = "DEVICEPTZRELEASESESSION";
	}

	public static class Matrix
	{
		public const string Visible = "MATRIXVISIBLE";
	}

	public static class TrustedServer
	{
		public const string Visible = "TRUSTEDSERVERVISIBLE";
	}

	public static class ViewGroup
	{
		public const string Visible = "VIEWGROUPVISIBLE";

		public const string ReadOnly = "VIEWGROUPREADONLY";
	}

	public static class ApplicationSecurity
	{
		public const string StatusAPIAccess = "APPLICATIONACCESSSTATUSAPI";

		public const string ClientLogin = "APPLICATIONACCESSCLIENTLOGIN";
	}

	public static class GroupSecurity
	{
		public const string HardwareEdit = "HARDWAREEDIT";

		public const string HardwareCommand = "HARDWARECOMMAND";
	}
}
