using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RecorderInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RecorderInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private CameraInfo[] CamerasField;

	[OptionalField]
	private int DefaultRecordingTimeSecondsField;

	[OptionalField]
	private string DescriptionField;

	[OptionalField]
	private HardwareInfo[] HardwareField;

	[OptionalField]
	private string HostNameField;

	[OptionalField]
	private InputInfo[] InputsField;

	[OptionalField]
	private DateTime LastModifiedField;

	[OptionalField]
	private MicrophoneInfo[] MicrophonesField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private OutputInfo[] OutputsField;

	[OptionalField]
	private Guid RecorderIdField;

	[OptionalField]
	private Guid ServiceIdField;

	[OptionalField]
	private SpeakerInfo[] SpeakersField;

	[OptionalField]
	private string TimeZoneNameField;

	[OptionalField]
	private string WebServerUriField;

	[OptionalField]
	private string XmlEncodingField;

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
	public CameraInfo[] Cameras
	{
		get
		{
			return CamerasField;
		}
		set
		{
			if (CamerasField != value)
			{
				CamerasField = value;
				RaisePropertyChanged("Cameras");
			}
		}
	}

	[DataMember]
	public int DefaultRecordingTimeSeconds
	{
		get
		{
			return DefaultRecordingTimeSecondsField;
		}
		set
		{
			if (!DefaultRecordingTimeSecondsField.Equals(value))
			{
				DefaultRecordingTimeSecondsField = value;
				RaisePropertyChanged("DefaultRecordingTimeSeconds");
			}
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			if ((object)DescriptionField != value)
			{
				DescriptionField = value;
				RaisePropertyChanged("Description");
			}
		}
	}

	[DataMember]
	public HardwareInfo[] Hardware
	{
		get
		{
			return HardwareField;
		}
		set
		{
			if (HardwareField != value)
			{
				HardwareField = value;
				RaisePropertyChanged("Hardware");
			}
		}
	}

	[DataMember]
	public string HostName
	{
		get
		{
			return HostNameField;
		}
		set
		{
			if ((object)HostNameField != value)
			{
				HostNameField = value;
				RaisePropertyChanged("HostName");
			}
		}
	}

	[DataMember]
	public InputInfo[] Inputs
	{
		get
		{
			return InputsField;
		}
		set
		{
			if (InputsField != value)
			{
				InputsField = value;
				RaisePropertyChanged("Inputs");
			}
		}
	}

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			if (!LastModifiedField.Equals(value))
			{
				LastModifiedField = value;
				RaisePropertyChanged("LastModified");
			}
		}
	}

	[DataMember]
	public MicrophoneInfo[] Microphones
	{
		get
		{
			return MicrophonesField;
		}
		set
		{
			if (MicrophonesField != value)
			{
				MicrophonesField = value;
				RaisePropertyChanged("Microphones");
			}
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
		}
	}

	[DataMember]
	public OutputInfo[] Outputs
	{
		get
		{
			return OutputsField;
		}
		set
		{
			if (OutputsField != value)
			{
				OutputsField = value;
				RaisePropertyChanged("Outputs");
			}
		}
	}

	[DataMember]
	public Guid RecorderId
	{
		get
		{
			return RecorderIdField;
		}
		set
		{
			if (!RecorderIdField.Equals(value))
			{
				RecorderIdField = value;
				RaisePropertyChanged("RecorderId");
			}
		}
	}

	[DataMember]
	public Guid ServiceId
	{
		get
		{
			return ServiceIdField;
		}
		set
		{
			if (!ServiceIdField.Equals(value))
			{
				ServiceIdField = value;
				RaisePropertyChanged("ServiceId");
			}
		}
	}

	[DataMember]
	public SpeakerInfo[] Speakers
	{
		get
		{
			return SpeakersField;
		}
		set
		{
			if (SpeakersField != value)
			{
				SpeakersField = value;
				RaisePropertyChanged("Speakers");
			}
		}
	}

	[DataMember]
	public string TimeZoneName
	{
		get
		{
			return TimeZoneNameField;
		}
		set
		{
			if ((object)TimeZoneNameField != value)
			{
				TimeZoneNameField = value;
				RaisePropertyChanged("TimeZoneName");
			}
		}
	}

	[DataMember]
	public string WebServerUri
	{
		get
		{
			return WebServerUriField;
		}
		set
		{
			if ((object)WebServerUriField != value)
			{
				WebServerUriField = value;
				RaisePropertyChanged("WebServerUri");
			}
		}
	}

	[DataMember]
	public string XmlEncoding
	{
		get
		{
			return XmlEncodingField;
		}
		set
		{
			if ((object)XmlEncodingField != value)
			{
				XmlEncodingField = value;
				RaisePropertyChanged("XmlEncoding");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
