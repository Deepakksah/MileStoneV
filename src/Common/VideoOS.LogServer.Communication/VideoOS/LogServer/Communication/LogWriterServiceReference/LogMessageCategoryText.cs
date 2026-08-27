using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogMessageCategoryText", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class LogMessageCategoryText : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string LanguageCodeField;

	[OptionalField]
	private string TextField;

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
	public string LanguageCode
	{
		get
		{
			return LanguageCodeField;
		}
		set
		{
			if ((object)LanguageCodeField != value)
			{
				LanguageCodeField = value;
				RaisePropertyChanged("LanguageCode");
			}
		}
	}

	[DataMember]
	public string Text
	{
		get
		{
			return TextField;
		}
		set
		{
			if ((object)TextField != value)
			{
				TextField = value;
				RaisePropertyChanged("Text");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
