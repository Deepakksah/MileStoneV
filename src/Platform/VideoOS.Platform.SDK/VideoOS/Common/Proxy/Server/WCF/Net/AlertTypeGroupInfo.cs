using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "AlertTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
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
