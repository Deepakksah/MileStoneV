using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternalData : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CustomSettingDataXmlField;

	[OptionalField]
	private int DataVersionField;

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
	public string CustomSettingDataXml
	{
		get
		{
			return CustomSettingDataXmlField;
		}
		set
		{
			if ((object)CustomSettingDataXmlField != value)
			{
				CustomSettingDataXmlField = value;
				RaisePropertyChanged("CustomSettingDataXml");
			}
		}
	}

	[DataMember]
	public int DataVersion
	{
		get
		{
			return DataVersionField;
		}
		set
		{
			if (!DataVersionField.Equals(value))
			{
				DataVersionField = value;
				RaisePropertyChanged("DataVersion");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
