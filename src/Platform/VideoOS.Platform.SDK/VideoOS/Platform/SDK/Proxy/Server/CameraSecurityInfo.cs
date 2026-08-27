using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class CameraSecurityInfo : ICameraSecurityInfo, IDeviceSecurityInfo
{
	private bool liveField;

	private bool browseField;

	private bool exportAviField;

	private bool exportJpegField;

	private bool smartSearchField;

	private bool getSequencesField;

	private bool exportDatabaseField;

	private bool startRecordingField;

	private bool stopRecordingField;

	private PtzSecurityInfo ptzSecurityField;

	private bool bookmarkViewField;

	private bool bookmarkAddField;

	private bool bookmarkEditField;

	private bool bookmarkDeleteField;

	private bool retentionViewField;

	private bool retentionCreateField;

	private bool retentionRemoveField;

	private bool managePresetsField;

	private bool managePatrollingProfilesField;

	private bool retrieveEdgeRecordingsField;

	IPtzSecurityInfo ICameraSecurityInfo.PtzSecurity
	{
		get
		{
			return PtzSecurity;
		}
		set
		{
			PtzSecurity = (PtzSecurityInfo)value;
		}
	}

	public bool Live
	{
		get
		{
			return liveField;
		}
		set
		{
			liveField = value;
		}
	}

	public bool Browse
	{
		get
		{
			return browseField;
		}
		set
		{
			browseField = value;
		}
	}

	public bool ExportAvi
	{
		get
		{
			return exportAviField;
		}
		set
		{
			exportAviField = value;
		}
	}

	public bool ExportJpeg
	{
		get
		{
			return exportJpegField;
		}
		set
		{
			exportJpegField = value;
		}
	}

	public bool SmartSearch
	{
		get
		{
			return smartSearchField;
		}
		set
		{
			smartSearchField = value;
		}
	}

	public bool GetSequences
	{
		get
		{
			return getSequencesField;
		}
		set
		{
			getSequencesField = value;
		}
	}

	public bool ExportDatabase
	{
		get
		{
			return exportDatabaseField;
		}
		set
		{
			exportDatabaseField = value;
		}
	}

	public bool StartRecording
	{
		get
		{
			return startRecordingField;
		}
		set
		{
			startRecordingField = value;
		}
	}

	public bool StopRecording
	{
		get
		{
			return stopRecordingField;
		}
		set
		{
			stopRecordingField = value;
		}
	}

	public PtzSecurityInfo PtzSecurity
	{
		get
		{
			return ptzSecurityField;
		}
		set
		{
			ptzSecurityField = value;
		}
	}

	public bool BookmarkView
	{
		get
		{
			return bookmarkViewField;
		}
		set
		{
			bookmarkViewField = value;
		}
	}

	public bool BookmarkAdd
	{
		get
		{
			return bookmarkAddField;
		}
		set
		{
			bookmarkAddField = value;
		}
	}

	public bool BookmarkEdit
	{
		get
		{
			return bookmarkEditField;
		}
		set
		{
			bookmarkEditField = value;
		}
	}

	public bool BookmarkDelete
	{
		get
		{
			return bookmarkDeleteField;
		}
		set
		{
			bookmarkDeleteField = value;
		}
	}

	public bool RetentionView
	{
		get
		{
			return retentionViewField;
		}
		set
		{
			retentionViewField = value;
		}
	}

	public bool RetentionCreate
	{
		get
		{
			return retentionCreateField;
		}
		set
		{
			retentionCreateField = value;
		}
	}

	public bool RetentionRemove
	{
		get
		{
			return retentionRemoveField;
		}
		set
		{
			retentionRemoveField = value;
		}
	}

	public bool ManagePresets
	{
		get
		{
			return managePresetsField;
		}
		set
		{
			managePresetsField = value;
		}
	}

	public bool ManagePatrollingProfiles
	{
		get
		{
			return managePatrollingProfilesField;
		}
		set
		{
			managePatrollingProfilesField = value;
		}
	}

	public bool RetrieveEdgeRecordings
	{
		get
		{
			return retrieveEdgeRecordingsField;
		}
		set
		{
			retrieveEdgeRecordingsField = value;
		}
	}
}
