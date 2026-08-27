using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OutputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputGroupInfo : GroupInfo
{
	[OptionalField]
	private OutputGroupInfo[] OutputGroupsField;

	[OptionalField]
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
			if (OutputGroupsField != value)
			{
				OutputGroupsField = value;
				RaisePropertyChanged("OutputGroups");
			}
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
			if (OutputsField != value)
			{
				OutputsField = value;
				RaisePropertyChanged("Outputs");
			}
		}
	}
}
