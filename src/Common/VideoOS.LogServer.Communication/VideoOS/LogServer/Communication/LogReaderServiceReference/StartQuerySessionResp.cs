using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StartQuerySessionResp", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class StartQuerySessionResp : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid QuerySessionIdField;

	[OptionalField]
	private int TotalLoglinesCountField;

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
	public Guid QuerySessionId
	{
		get
		{
			return QuerySessionIdField;
		}
		set
		{
			if (!QuerySessionIdField.Equals(value))
			{
				QuerySessionIdField = value;
				RaisePropertyChanged("QuerySessionId");
			}
		}
	}

	[DataMember]
	public int TotalLoglinesCount
	{
		get
		{
			return TotalLoglinesCountField;
		}
		set
		{
			if (!TotalLoglinesCountField.Equals(value))
			{
				TotalLoglinesCountField = value;
				RaisePropertyChanged("TotalLoglinesCount");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
