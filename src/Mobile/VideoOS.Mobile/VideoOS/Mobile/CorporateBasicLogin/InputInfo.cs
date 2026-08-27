using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class InputInfo : DeviceInfo
{
	[OptionalField]
	private InputSecurityInfo InputSecurityField;

	[DataMember]
	public InputSecurityInfo InputSecurity
	{
		get
		{
			return InputSecurityField;
		}
		set
		{
			if (InputSecurityField != value)
			{
				InputSecurityField = value;
				RaisePropertyChanged("InputSecurity");
			}
		}
	}
}
