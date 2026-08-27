using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StartQuerySessionParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService")]
public class StartQuerySessionParams : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string LanguageField;

	[OptionalField]
	private QueryFilters QueryFiltersField;

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
	public QueryFilters QueryFilters
	{
		get
		{
			return QueryFiltersField;
		}
		set
		{
			if (QueryFiltersField != value)
			{
				QueryFiltersField = value;
				RaisePropertyChanged("QueryFilters");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
