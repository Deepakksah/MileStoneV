using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "QueryFilters", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class QueryFilters : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Filter[] FiltersField;

	[OptionalField]
	private DateTime FromTimeField;

	[OptionalField]
	private LogType LogTypeField;

	[OptionalField]
	private DateTime ToTimeField;

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
	public Filter[] Filters
	{
		get
		{
			return FiltersField;
		}
		set
		{
			if (FiltersField != value)
			{
				FiltersField = value;
				RaisePropertyChanged("Filters");
			}
		}
	}

	[DataMember]
	public DateTime FromTime
	{
		get
		{
			return FromTimeField;
		}
		set
		{
			if (!FromTimeField.Equals(value))
			{
				FromTimeField = value;
				RaisePropertyChanged("FromTime");
			}
		}
	}

	[DataMember]
	public LogType LogType
	{
		get
		{
			return LogTypeField;
		}
		set
		{
			if (!LogTypeField.Equals(value))
			{
				LogTypeField = value;
				RaisePropertyChanged("LogType");
			}
		}
	}

	[DataMember]
	public DateTime ToTime
	{
		get
		{
			return ToTimeField;
		}
		set
		{
			if (!ToTimeField.Equals(value))
			{
				ToTimeField = value;
				RaisePropertyChanged("ToTime");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
