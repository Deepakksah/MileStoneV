using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class InputGroupInfo : GroupInfo, IInputGroupInfo, IGroupInfo
{
	private InputGroupInfo[] InputGroupsField;

	private Guid[] InputsField;

	[DataMember]
	public InputGroupInfo[] InputGroups
	{
		get
		{
			return InputGroupsField;
		}
		set
		{
			InputGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] Inputs
	{
		get
		{
			return InputsField;
		}
		set
		{
			InputsField = value;
		}
	}

	IInputGroupInfo[] IInputGroupInfo.InputGroups
	{
		get
		{
			return InputGroups;
		}
		set
		{
			InputGroups = (InputGroupInfo[])value;
		}
	}
}
