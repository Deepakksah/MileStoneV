using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WriteLoglineParamsBase", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
[KnownType(typeof(WriteLegacyLoglineParams))]
[KnownType(typeof(WriteLoglineParams))]
public class WriteLoglineParamsBase : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private DateTime ClientTimestampField;

	[OptionalField]
	private string EventTypeField;

	[OptionalField]
	private LogLevel LogLevelField;

	[OptionalField]
	private Dictionary<string, string> ParametersField;

	[OptionalField]
	private PermissionType PermissionTypeField;

	[OptionalField]
	private string RuleNameField;

	[OptionalField]
	private string ServiceNameField;

	[OptionalField]
	private string SourceField;

	[OptionalField]
	private string UserField;

	[OptionalField]
	private string UserLocationField;

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
	public DateTime ClientTimestamp
	{
		get
		{
			return ClientTimestampField;
		}
		set
		{
			if (!ClientTimestampField.Equals(value))
			{
				ClientTimestampField = value;
				RaisePropertyChanged("ClientTimestamp");
			}
		}
	}

	[DataMember]
	public string EventType
	{
		get
		{
			return EventTypeField;
		}
		set
		{
			if ((object)EventTypeField != value)
			{
				EventTypeField = value;
				RaisePropertyChanged("EventType");
			}
		}
	}

	[DataMember]
	public LogLevel LogLevel
	{
		get
		{
			return LogLevelField;
		}
		set
		{
			if (!LogLevelField.Equals(value))
			{
				LogLevelField = value;
				RaisePropertyChanged("LogLevel");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> Parameters
	{
		get
		{
			return ParametersField;
		}
		set
		{
			if (ParametersField != value)
			{
				ParametersField = value;
				RaisePropertyChanged("Parameters");
			}
		}
	}

	[DataMember]
	public PermissionType PermissionType
	{
		get
		{
			return PermissionTypeField;
		}
		set
		{
			if (!PermissionTypeField.Equals(value))
			{
				PermissionTypeField = value;
				RaisePropertyChanged("PermissionType");
			}
		}
	}

	[DataMember]
	public string RuleName
	{
		get
		{
			return RuleNameField;
		}
		set
		{
			if ((object)RuleNameField != value)
			{
				RuleNameField = value;
				RaisePropertyChanged("RuleName");
			}
		}
	}

	[DataMember]
	public string ServiceName
	{
		get
		{
			return ServiceNameField;
		}
		set
		{
			if ((object)ServiceNameField != value)
			{
				ServiceNameField = value;
				RaisePropertyChanged("ServiceName");
			}
		}
	}

	[DataMember]
	public string Source
	{
		get
		{
			return SourceField;
		}
		set
		{
			if ((object)SourceField != value)
			{
				SourceField = value;
				RaisePropertyChanged("Source");
			}
		}
	}

	[DataMember]
	public string User
	{
		get
		{
			return UserField;
		}
		set
		{
			if ((object)UserField != value)
			{
				UserField = value;
				RaisePropertyChanged("User");
			}
		}
	}

	[DataMember]
	public string UserLocation
	{
		get
		{
			return UserLocationField;
		}
		set
		{
			if ((object)UserLocationField != value)
			{
				UserLocationField = value;
				RaisePropertyChanged("UserLocation");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
