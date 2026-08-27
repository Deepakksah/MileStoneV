using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class InputGroupInfo : GroupInfo
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
}
