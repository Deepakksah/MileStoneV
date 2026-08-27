using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CapabilityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CapabilityInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool AbsoluteField;

	[OptionalField]
	private bool AutomaticField;

	[OptionalField]
	private Guid CapabilityIdField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private bool RelativeField;

	[OptionalField]
	private bool SpeedField;

	[OptionalField]
	private bool StartField;

	[OptionalField]
	private bool StopField;

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
	public bool Absolute
	{
		get
		{
			return AbsoluteField;
		}
		set
		{
			if (!AbsoluteField.Equals(value))
			{
				AbsoluteField = value;
				RaisePropertyChanged("Absolute");
			}
		}
	}

	[DataMember]
	public bool Automatic
	{
		get
		{
			return AutomaticField;
		}
		set
		{
			if (!AutomaticField.Equals(value))
			{
				AutomaticField = value;
				RaisePropertyChanged("Automatic");
			}
		}
	}

	[DataMember]
	public Guid CapabilityId
	{
		get
		{
			return CapabilityIdField;
		}
		set
		{
			if (!CapabilityIdField.Equals(value))
			{
				CapabilityIdField = value;
				RaisePropertyChanged("CapabilityId");
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
	public bool Relative
	{
		get
		{
			return RelativeField;
		}
		set
		{
			if (!RelativeField.Equals(value))
			{
				RelativeField = value;
				RaisePropertyChanged("Relative");
			}
		}
	}

	[DataMember]
	public bool Speed
	{
		get
		{
			return SpeedField;
		}
		set
		{
			if (!SpeedField.Equals(value))
			{
				SpeedField = value;
				RaisePropertyChanged("Speed");
			}
		}
	}

	[DataMember]
	public bool Start
	{
		get
		{
			return StartField;
		}
		set
		{
			if (!StartField.Equals(value))
			{
				StartField = value;
				RaisePropertyChanged("Start");
			}
		}
	}

	[DataMember]
	public bool Stop
	{
		get
		{
			return StopField;
		}
		set
		{
			if (!StopField.Equals(value))
			{
				StopField = value;
				RaisePropertyChanged("Stop");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
