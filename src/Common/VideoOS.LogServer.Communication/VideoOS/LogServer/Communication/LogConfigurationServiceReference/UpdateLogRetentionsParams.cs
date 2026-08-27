using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "UpdateLogRetentionsParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class UpdateLogRetentionsParams : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private LogRetentions LogRetentionsField;

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
	public LogRetentions LogRetentions
	{
		get
		{
			return LogRetentionsField;
		}
		set
		{
			if (LogRetentionsField != value)
			{
				LogRetentionsField = value;
				RaisePropertyChanged("LogRetentions");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
