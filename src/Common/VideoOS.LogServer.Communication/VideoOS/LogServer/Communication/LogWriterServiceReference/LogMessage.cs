using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogMessage", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class LogMessage : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CategoryField;

	[OptionalField]
	private Guid IdField;

	[OptionalField]
	private LogType LogTypeField;

	[OptionalField]
	private LogMessageText[] MessageTextsField;

	[OptionalField]
	private string SourceTypeField;

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
	public string Category
	{
		get
		{
			return CategoryField;
		}
		set
		{
			if ((object)CategoryField != value)
			{
				CategoryField = value;
				RaisePropertyChanged("Category");
			}
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
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
	public LogMessageText[] MessageTexts
	{
		get
		{
			return MessageTextsField;
		}
		set
		{
			if (MessageTextsField != value)
			{
				MessageTextsField = value;
				RaisePropertyChanged("MessageTexts");
			}
		}
	}

	[DataMember]
	public string SourceType
	{
		get
		{
			return SourceTypeField;
		}
		set
		{
			if ((object)SourceTypeField != value)
			{
				SourceTypeField = value;
				RaisePropertyChanged("SourceType");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
