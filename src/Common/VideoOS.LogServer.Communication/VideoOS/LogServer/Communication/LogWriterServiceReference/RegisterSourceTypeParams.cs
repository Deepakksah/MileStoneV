using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RegisterSourceTypeParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class RegisterSourceTypeParams : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private LogMessageSourceType[] SourceTypesField;

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
	public LogMessageSourceType[] SourceTypes
	{
		get
		{
			return SourceTypesField;
		}
		set
		{
			if (SourceTypesField != value)
			{
				SourceTypesField = value;
				RaisePropertyChanged("SourceTypes");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
