using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AuditLogline", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class AuditLogline : LoglineBase
{
	[OptionalField]
	private PermissionType PermissionTypeField;

	[OptionalField]
	private string UserField;

	[OptionalField]
	private string UserLocationField;

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
}
