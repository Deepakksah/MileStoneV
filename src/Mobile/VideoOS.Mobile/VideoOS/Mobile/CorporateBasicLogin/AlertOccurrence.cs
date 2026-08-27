using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlertOccurrence", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertOccurrence : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid AlertIdField;

	[OptionalField]
	private DateTime TimeField;

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
	public Guid AlertId
	{
		get
		{
			return AlertIdField;
		}
		set
		{
			if (!AlertIdField.Equals(value))
			{
				AlertIdField = value;
				RaisePropertyChanged("AlertId");
			}
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			if (!TimeField.Equals(value))
			{
				TimeField = value;
				RaisePropertyChanged("Time");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
