using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SpeakerSecurityInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool BookmarkAddField;

	[OptionalField]
	private bool BookmarkDeleteField;

	[OptionalField]
	private bool BookmarkEditField;

	[OptionalField]
	private bool BookmarkViewField;

	[OptionalField]
	private bool BrowseField;

	[OptionalField]
	private bool ExportDatabaseField;

	[OptionalField]
	private bool GetSequencesField;

	[OptionalField]
	private bool LiveField;

	[OptionalField]
	private bool RetrieveEdgeRecordingsField;

	[OptionalField]
	private bool SpeakField;

	[Browsable(false)]
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
			if (!BookmarkAddField.Equals(value))
			{
				BookmarkAddField = value;
				RaisePropertyChanged("BookmarkAdd");
			}
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
			if (!BookmarkDeleteField.Equals(value))
			{
				BookmarkDeleteField = value;
				RaisePropertyChanged("BookmarkDelete");
			}
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
			if (!BookmarkEditField.Equals(value))
			{
				BookmarkEditField = value;
				RaisePropertyChanged("BookmarkEdit");
			}
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
			if (!BookmarkViewField.Equals(value))
			{
				BookmarkViewField = value;
				RaisePropertyChanged("BookmarkView");
			}
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
			if (!BrowseField.Equals(value))
			{
				BrowseField = value;
				RaisePropertyChanged("Browse");
			}
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
			if (!ExportDatabaseField.Equals(value))
			{
				ExportDatabaseField = value;
				RaisePropertyChanged("ExportDatabase");
			}
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
			if (!GetSequencesField.Equals(value))
			{
				GetSequencesField = value;
				RaisePropertyChanged("GetSequences");
			}
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
			if (!LiveField.Equals(value))
			{
				LiveField = value;
				RaisePropertyChanged("Live");
			}
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
			if (!RetrieveEdgeRecordingsField.Equals(value))
			{
				RetrieveEdgeRecordingsField = value;
				RaisePropertyChanged("RetrieveEdgeRecordings");
			}
		}
	}

	[DataMember]
	public bool Speak
	{
		get
		{
			return SpeakField;
		}
		set
		{
			if (!SpeakField.Equals(value))
			{
				SpeakField = value;
				RaisePropertyChanged("Speak");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
