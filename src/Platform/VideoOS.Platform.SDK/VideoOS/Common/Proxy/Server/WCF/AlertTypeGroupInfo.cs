using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlertTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class AlertTypeGroupInfo : GroupInfo, IAlertTypeGroupInfo, IGroupInfo
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

	IAlertTypeGroupInfo[] IAlertTypeGroupInfo.AlertTypeGroups
	{
		get
		{
			return AlertTypeGroups;
		}
		set
		{
			AlertTypeGroups = (AlertTypeGroupInfo[])value;
		}
	}
}
