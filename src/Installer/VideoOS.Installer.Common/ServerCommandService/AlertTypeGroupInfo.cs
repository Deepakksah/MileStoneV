using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlertTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertTypeGroupInfo : GroupInfo
{
	private AlertTypeGroupInfo[] AlertTypeGroupsField;

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
			AlertTypeGroupsField = value;
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
			AlertTypesField = value;
		}
	}
}
