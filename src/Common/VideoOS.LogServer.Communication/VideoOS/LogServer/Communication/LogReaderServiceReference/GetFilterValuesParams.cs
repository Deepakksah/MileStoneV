using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetFilterValuesParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class GetFilterValuesParams : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private FilterType[] FilterTypesField;

	[OptionalField]
	private string LanguageField;

	[OptionalField]
	private LogType LogTypeField;

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
	public FilterType[] FilterTypes
	{
		get
		{
			return FilterTypesField;
		}
		set
		{
			if (FilterTypesField != value)
			{
				FilterTypesField = value;
				RaisePropertyChanged("FilterTypes");
			}
		}
	}

	[DataMember]
	public string Language
	{
		get
		{
			return LanguageField;
		}
		set
		{
			if ((object)LanguageField != value)
			{
				LanguageField = value;
				RaisePropertyChanged("Language");
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
