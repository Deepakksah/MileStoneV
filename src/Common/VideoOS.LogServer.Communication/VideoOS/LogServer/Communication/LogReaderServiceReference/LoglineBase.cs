using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LoglineBase", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
[KnownType(typeof(SystemLogline))]
[KnownType(typeof(RuleLogline))]
[KnownType(typeof(AuditLogline))]
public class LoglineBase : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CategoryField;

	[OptionalField]
	private DateTime ClientTimestampUtcField;

	[OptionalField]
	private string MessageTextField;

	[OptionalField]
	private string SourceNameField;

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
	public DateTime ClientTimestampUtc
	{
		get
		{
			return ClientTimestampUtcField;
		}
		set
		{
			if (!ClientTimestampUtcField.Equals(value))
			{
				ClientTimestampUtcField = value;
				RaisePropertyChanged("ClientTimestampUtc");
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

	[DataMember]
	public string SourceName
	{
		get
		{
			return SourceNameField;
		}
		set
		{
			if ((object)SourceNameField != value)
			{
				SourceNameField = value;
				RaisePropertyChanged("SourceName");
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
