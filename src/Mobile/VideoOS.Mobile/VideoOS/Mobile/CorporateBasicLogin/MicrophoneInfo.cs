using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MicrophoneInfo : DeviceInfo
{
	[OptionalField]
	private bool EdgeStorageSupportedField;

	[OptionalField]
	private MicrophoneSecurityInfo MicrophoneSecurityField;

	[OptionalField]
	private bool MulticastEnabledField;

	[DataMember]
	public bool EdgeStorageSupported
	{
		get
		{
			return EdgeStorageSupportedField;
		}
		set
		{
			if (!EdgeStorageSupportedField.Equals(value))
			{
				EdgeStorageSupportedField = value;
				RaisePropertyChanged("EdgeStorageSupported");
			}
		}
	}

	[DataMember]
	public MicrophoneSecurityInfo MicrophoneSecurity
	{
		get
		{
			return MicrophoneSecurityField;
		}
		set
		{
			if (MicrophoneSecurityField != value)
			{
				MicrophoneSecurityField = value;
				RaisePropertyChanged("MicrophoneSecurity");
			}
		}
	}

	[DataMember]
	public bool MulticastEnabled
	{
		get
		{
			return MulticastEnabledField;
		}
		set
		{
			if (!MulticastEnabledField.Equals(value))
			{
				MulticastEnabledField = value;
				RaisePropertyChanged("MulticastEnabled");
			}
		}
	}
}
