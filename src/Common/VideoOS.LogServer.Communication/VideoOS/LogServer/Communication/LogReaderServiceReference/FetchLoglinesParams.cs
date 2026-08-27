using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FetchLoglinesParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class FetchLoglinesParams : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int BatchSizeField;

	[OptionalField]
	private int OffsetField;

	[OptionalField]
	private Guid QuerySessionIdField;

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
	public int BatchSize
	{
		get
		{
			return BatchSizeField;
		}
		set
		{
			if (!BatchSizeField.Equals(value))
			{
				BatchSizeField = value;
				RaisePropertyChanged("BatchSize");
			}
		}
	}

	[DataMember]
	public int Offset
	{
		get
		{
			return OffsetField;
		}
		set
		{
			if (!OffsetField.Equals(value))
			{
				OffsetField = value;
				RaisePropertyChanged("Offset");
			}
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
