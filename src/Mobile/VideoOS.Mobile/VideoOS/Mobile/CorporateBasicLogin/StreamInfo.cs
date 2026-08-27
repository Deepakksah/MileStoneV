using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StreamInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class StreamInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool DefaultField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private Guid StreamIdField;

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
	public bool Default
	{
		get
		{
			return DefaultField;
		}
		set
		{
			if (!DefaultField.Equals(value))
			{
				DefaultField = value;
				RaisePropertyChanged("Default");
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
	public Guid StreamId
	{
		get
		{
			return StreamIdField;
		}
		set
		{
			if (!StreamIdField.Equals(value))
			{
				StreamIdField = value;
				RaisePropertyChanged("StreamId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
