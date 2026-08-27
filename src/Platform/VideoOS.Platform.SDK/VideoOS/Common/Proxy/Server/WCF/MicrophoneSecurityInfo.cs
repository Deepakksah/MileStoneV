using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MicrophoneSecurityInfo : IExtensibleDataObject, IMicrophoneSecurityInfo, IDeviceSecurityInfo
{
	private ExtensionDataObject extensionDataField;

	private bool BookmarkAddField;

	private bool BookmarkDeleteField;

	private bool BookmarkEditField;

	private bool BookmarkViewField;

	private bool BrowseField;

	private bool ExportDatabaseField;

	private bool GetSequencesField;

	private bool LiveField;

	private bool RestrictedMediaCreateField;

	private bool RestrictedMediaRemoveField;

	private bool RestrictedMediaViewField;

	private bool RetentionCreateField;

	private bool RetentionRemoveField;

	private bool RetentionViewField;

	private bool RetrieveEdgeRecordingsField;

	private bool StartRecordingField;

	private bool StopRecordingField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public bool BookmarkAdd
	{
		get
		{
			return BookmarkAddField;
		}
		set
		{
			BookmarkAddField = value;
		}
	}

	[DataMember]
	public bool BookmarkDelete
	{
		get
		{
			return BookmarkDeleteField;
		}
		set
		{
			BookmarkDeleteField = value;
		}
	}

	[DataMember]
	public bool BookmarkEdit
	{
		get
		{
			return BookmarkEditField;
		}
		set
		{
			BookmarkEditField = value;
		}
	}

	[DataMember]
	public bool BookmarkView
	{
		get
		{
			return BookmarkViewField;
		}
		set
		{
			BookmarkViewField = value;
		}
	}

	[DataMember]
	public bool Browse
	{
		get
		{
			return BrowseField;
		}
		set
		{
			BrowseField = value;
		}
	}

	[DataMember]
	public bool ExportDatabase
	{
		get
		{
			return ExportDatabaseField;
		}
		set
		{
			ExportDatabaseField = value;
		}
	}

	[DataMember]
	public bool GetSequences
	{
		get
		{
			return GetSequencesField;
		}
		set
		{
			GetSequencesField = value;
		}
	}

	[DataMember]
	public bool Live
	{
		get
		{
			return LiveField;
		}
		set
		{
			LiveField = value;
		}
	}

	[DataMember]
	public bool RestrictedMediaCreate
	{
		get
		{
			return RestrictedMediaCreateField;
		}
		set
		{
			RestrictedMediaCreateField = value;
		}
	}

	[DataMember]
	public bool RestrictedMediaRemove
	{
		get
		{
			return RestrictedMediaRemoveField;
		}
		set
		{
			RestrictedMediaRemoveField = value;
		}
	}

	[DataMember]
	public bool RestrictedMediaView
	{
		get
		{
			return RestrictedMediaViewField;
		}
		set
		{
			RestrictedMediaViewField = value;
		}
	}

	[DataMember]
	public bool RetentionCreate
	{
		get
		{
			return RetentionCreateField;
		}
		set
		{
			RetentionCreateField = value;
		}
	}

	[DataMember]
	public bool RetentionRemove
	{
		get
		{
			return RetentionRemoveField;
		}
		set
		{
			RetentionRemoveField = value;
		}
	}

	[DataMember]
	public bool RetentionView
	{
		get
		{
			return RetentionViewField;
		}
		set
		{
			RetentionViewField = value;
		}
	}

	[DataMember]
	public bool RetrieveEdgeRecordings
	{
		get
		{
			return RetrieveEdgeRecordingsField;
		}
		set
		{
			RetrieveEdgeRecordingsField = value;
		}
	}

	[DataMember]
	public bool StartRecording
	{
		get
		{
			return StartRecordingField;
		}
		set
		{
			StartRecordingField = value;
		}
	}

	[DataMember]
	public bool StopRecording
	{
		get
		{
			return StopRecordingField;
		}
		set
		{
			StopRecordingField = value;
		}
	}
}
