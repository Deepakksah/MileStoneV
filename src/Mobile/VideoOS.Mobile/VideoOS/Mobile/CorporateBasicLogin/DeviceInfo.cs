using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[KnownType(typeof(InputInfo))]
[KnownType(typeof(MicrophoneInfo))]
[KnownType(typeof(OutputInfo))]
[KnownType(typeof(SpeakerInfo))]
[KnownType(typeof(CameraInfo))]
public class DeviceInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DescriptionField;

	[OptionalField]
	private Guid DeviceIdField;

	[OptionalField]
	private int DeviceIndexField;

	[OptionalField]
	private Guid HardwareIdField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private Guid RecorderIdField;

	[OptionalField]
	private string ShortcutField;

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
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			if (!DeviceIdField.Equals(value))
			{
				DeviceIdField = value;
				RaisePropertyChanged("DeviceId");
			}
		}
	}

	[DataMember]
	public int DeviceIndex
	{
		get
		{
			return DeviceIndexField;
		}
		set
		{
			if (!DeviceIndexField.Equals(value))
			{
				DeviceIndexField = value;
				RaisePropertyChanged("DeviceIndex");
			}
		}
	}

	[DataMember]
	public Guid HardwareId
	{
		get
		{
			return HardwareIdField;
		}
		set
		{
			if (!HardwareIdField.Equals(value))
			{
				HardwareIdField = value;
				RaisePropertyChanged("HardwareId");
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
	public string Shortcut
	{
		get
		{
			return ShortcutField;
		}
		set
		{
			if ((object)ShortcutField != value)
			{
				ShortcutField = value;
				RaisePropertyChanged("Shortcut");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
