using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogMessageText", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class LogMessageText : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string LanguageCodeField;

	[OptionalField]
	private string MessageTextField;

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
	public string MessageText
	{
		get
		{
			return MessageTextField;
		}
		set
		{
			if ((object)MessageTextField != value)
			{
				MessageTextField = value;
				RaisePropertyChanged("MessageText");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
