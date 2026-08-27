using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HardwareInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class HardwareInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DescriptionField;

	[OptionalField]
	private Guid[] DeviceIdsField;

	[OptionalField]
	private Guid HardwareIdField;

	[OptionalField]
	private bool InterconnectedField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private Guid RecorderIdField;

	[OptionalField]
	private string UriField;

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
	public Guid[] DeviceIds
	{
		get
		{
			return DeviceIdsField;
		}
		set
		{
			if (DeviceIdsField != value)
			{
				DeviceIdsField = value;
				RaisePropertyChanged("DeviceIds");
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
	public bool Interconnected
	{
		get
		{
			return InterconnectedField;
		}
		set
		{
			if (!InterconnectedField.Equals(value))
			{
				InterconnectedField = value;
				RaisePropertyChanged("Interconnected");
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
	public string Uri
	{
		get
		{
			return UriField;
		}
		set
		{
			if ((object)UriField != value)
			{
				UriField = value;
				RaisePropertyChanged("Uri");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
