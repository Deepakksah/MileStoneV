using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceMappingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class DeviceMappingInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid DeviceAField;

	[OptionalField]
	private Guid DeviceBField;

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
	public Guid DeviceA
	{
		get
		{
			return DeviceAField;
		}
		set
		{
			if (!DeviceAField.Equals(value))
			{
				DeviceAField = value;
				RaisePropertyChanged("DeviceA");
			}
		}
	}

	[DataMember]
	public Guid DeviceB
	{
		get
		{
			return DeviceBField;
		}
		set
		{
			if (!DeviceBField.Equals(value))
			{
				DeviceBField = value;
				RaisePropertyChanged("DeviceB");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
