using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OutputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputInfo : DeviceInfo
{
	[OptionalField]
	private OutputSecurityInfo OutputSecurityField;

	[DataMember]
	public OutputSecurityInfo OutputSecurity
	{
		get
		{
			return OutputSecurityField;
		}
		set
		{
			if (OutputSecurityField != value)
			{
				OutputSecurityField = value;
				RaisePropertyChanged("OutputSecurity");
			}
		}
	}
}
