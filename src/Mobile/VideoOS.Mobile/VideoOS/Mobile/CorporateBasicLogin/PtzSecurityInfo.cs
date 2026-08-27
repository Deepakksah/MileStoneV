using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PtzSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PtzSecurityInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool ManualControlField;

	[OptionalField]
	private bool PresetControlField;

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
	public bool ManualControl
	{
		get
		{
			return ManualControlField;
		}
		set
		{
			if (!ManualControlField.Equals(value))
			{
				ManualControlField = value;
				RaisePropertyChanged("ManualControl");
			}
		}
	}

	[DataMember]
	public bool PresetControl
	{
		get
		{
			return PresetControlField;
		}
		set
		{
			if (!PresetControlField.Equals(value))
			{
				PresetControlField = value;
				RaisePropertyChanged("PresetControl");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
