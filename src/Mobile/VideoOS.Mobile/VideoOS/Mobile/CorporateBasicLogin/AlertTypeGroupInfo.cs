using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlertTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertTypeGroupInfo : GroupInfo
{
	[OptionalField]
	private AlertTypeGroupInfo[] AlertTypeGroupsField;

	[OptionalField]
	private Guid[] AlertTypesField;

	[DataMember]
	public AlertTypeGroupInfo[] AlertTypeGroups
	{
		get
		{
			return AlertTypeGroupsField;
		}
		set
		{
			if (AlertTypeGroupsField != value)
			{
				AlertTypeGroupsField = value;
				RaisePropertyChanged("AlertTypeGroups");
			}
		}
	}

	[DataMember]
	public Guid[] AlertTypes
	{
		get
		{
			return AlertTypesField;
		}
		set
		{
			if (AlertTypesField != value)
			{
				AlertTypesField = value;
				RaisePropertyChanged("AlertTypes");
			}
		}
	}
}
