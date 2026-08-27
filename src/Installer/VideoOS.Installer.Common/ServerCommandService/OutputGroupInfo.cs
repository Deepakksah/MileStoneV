using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OutputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputGroupInfo : GroupInfo
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
}
