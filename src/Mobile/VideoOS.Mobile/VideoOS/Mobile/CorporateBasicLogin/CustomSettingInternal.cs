using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternal : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private CustomSettingInternalData CustomSettingDataField;

	[OptionalField]
	private CustomSettingInfo CustomSettingInfoField;

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
	public CustomSettingInternalData CustomSettingData
	{
		get
		{
			return CustomSettingDataField;
		}
		set
		{
			if (CustomSettingDataField != value)
			{
				CustomSettingDataField = value;
				RaisePropertyChanged("CustomSettingData");
			}
		}
	}

	[DataMember]
	public CustomSettingInfo CustomSettingInfo
	{
		get
		{
			return CustomSettingInfoField;
		}
		set
		{
			if (CustomSettingInfoField != value)
			{
				CustomSettingInfoField = value;
				RaisePropertyChanged("CustomSettingInfo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
