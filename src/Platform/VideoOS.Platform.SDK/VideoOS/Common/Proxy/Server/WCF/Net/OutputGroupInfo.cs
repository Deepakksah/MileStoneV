using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "OutputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputGroupInfo : GroupInfo, IOutputGroupInfo, IGroupInfo
{
	private OutputGroupInfo[] OutputGroupsField;

	private Guid[] OutputsField;

	[DataMember]
	public OutputGroupInfo[] OutputGroups
	{
		get
		{
			return OutputGroupsField;
		}
		set
		{
			OutputGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] Outputs
	{
		get
		{
			return OutputsField;
		}
		set
		{
			OutputsField = value;
		}
	}

	IOutputGroupInfo[] IOutputGroupInfo.OutputGroups
	{
		get
		{
			return OutputGroups;
		}
		set
		{
			OutputGroups = (OutputGroupInfo[])value;
		}
	}
}
