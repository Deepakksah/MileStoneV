using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FilterValue", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class FilterValue : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DisplayValueField;

	[OptionalField]
	private string FilterIdField;

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
	public string DisplayValue
	{
		get
		{
			return DisplayValueField;
		}
		set
		{
			if ((object)DisplayValueField != value)
			{
				DisplayValueField = value;
				RaisePropertyChanged("DisplayValue");
			}
		}
	}

	[DataMember]
	public string FilterId
	{
		get
		{
			return FilterIdField;
		}
		set
		{
			if ((object)FilterIdField != value)
			{
				FilterIdField = value;
				RaisePropertyChanged("FilterId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
