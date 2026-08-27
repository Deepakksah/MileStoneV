using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class InputGroupInfo : GroupInfo
{
	[OptionalField]
	private InputGroupInfo[] InputGroupsField;

	[OptionalField]
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
			if (InputGroupsField != value)
			{
				InputGroupsField = value;
				RaisePropertyChanged("InputGroups");
			}
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
			if (InputsField != value)
			{
				InputsField = value;
				RaisePropertyChanged("Inputs");
			}
		}
	}
}
