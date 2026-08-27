using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogConfigurationServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogRetentions", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class LogRetentions : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int AuditLogRetentionField;

	[OptionalField]
	private int RuleLogRetentionField;

	[OptionalField]
	private int SystemLogRetentionField;

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
	public int AuditLogRetention
	{
		get
		{
			return AuditLogRetentionField;
		}
		set
		{
			if (!AuditLogRetentionField.Equals(value))
			{
				AuditLogRetentionField = value;
				RaisePropertyChanged("AuditLogRetention");
			}
		}
	}

	[DataMember]
	public int RuleLogRetention
	{
		get
		{
			return RuleLogRetentionField;
		}
		set
		{
			if (!RuleLogRetentionField.Equals(value))
			{
				RuleLogRetentionField = value;
				RaisePropertyChanged("RuleLogRetention");
			}
		}
	}

	[DataMember]
	public int SystemLogRetention
	{
		get
		{
			return SystemLogRetentionField;
		}
		set
		{
			if (!SystemLogRetentionField.Equals(value))
			{
				SystemLogRetentionField = value;
				RaisePropertyChanged("SystemLogRetention");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
