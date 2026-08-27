using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SpeakerInfo : DeviceInfo
{
	[OptionalField]
	private bool EdgeStorageSupportedField;

	[OptionalField]
	private bool MulticastEnabledField;

	[OptionalField]
	private SpeakerSecurityInfo SpeakerSecurityField;

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

	[DataMember]
	public SpeakerSecurityInfo SpeakerSecurity
	{
		get
		{
			return SpeakerSecurityField;
		}
		set
		{
			if (SpeakerSecurityField != value)
			{
				SpeakerSecurityField = value;
				RaisePropertyChanged("SpeakerSecurity");
			}
		}
	}
}
