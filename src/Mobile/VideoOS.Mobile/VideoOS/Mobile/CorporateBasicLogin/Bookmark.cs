using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Bookmark", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Bookmark : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DescriptionField;

	[OptionalField]
	private Guid DeviceIdField;

	[OptionalField]
	private string HeaderField;

	[OptionalField]
	private Guid IdField;

	[OptionalField]
	private string ReferenceField;

	[OptionalField]
	private DateTime TimeBeginField;

	[OptionalField]
	private DateTime TimeEndField;

	[OptionalField]
	private DateTime TimeTriggedField;

	[OptionalField]
	private string UserField;

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
	public string Header
	{
		get
		{
			return HeaderField;
		}
		set
		{
			if ((object)HeaderField != value)
			{
				HeaderField = value;
				RaisePropertyChanged("Header");
			}
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
		}
	}

	[DataMember]
	public string Reference
	{
		get
		{
			return ReferenceField;
		}
		set
		{
			if ((object)ReferenceField != value)
			{
				ReferenceField = value;
				RaisePropertyChanged("Reference");
			}
		}
	}

	[DataMember]
	public DateTime TimeBegin
	{
		get
		{
			return TimeBeginField;
		}
		set
		{
			if (!TimeBeginField.Equals(value))
			{
				TimeBeginField = value;
				RaisePropertyChanged("TimeBegin");
			}
		}
	}

	[DataMember]
	public DateTime TimeEnd
	{
		get
		{
			return TimeEndField;
		}
		set
		{
			if (!TimeEndField.Equals(value))
			{
				TimeEndField = value;
				RaisePropertyChanged("TimeEnd");
			}
		}
	}

	[DataMember]
	public DateTime TimeTrigged
	{
		get
		{
			return TimeTriggedField;
		}
		set
		{
			if (!TimeTriggedField.Equals(value))
			{
				TimeTriggedField = value;
				RaisePropertyChanged("TimeTrigged");
			}
		}
	}

	[DataMember]
	public string User
	{
		get
		{
			return UserField;
		}
		set
		{
			if ((object)UserField != value)
			{
				UserField = value;
				RaisePropertyChanged("User");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
