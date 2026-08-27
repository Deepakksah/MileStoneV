using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LoginInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class LoginInfo : IExtensibleDataObject, ILoginInfo
{
	private ExtensionDataObject extensionDataField;

	private DateTime RegistrationTimeField;

	private TimeDuration TimeToLiveField;

	private bool TimeToLiveLimitedField;

	private string TokenField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public DateTime RegistrationTime
	{
		get
		{
			return RegistrationTimeField;
		}
		set
		{
			RegistrationTimeField = value;
		}
	}

	[DataMember]
	public TimeDuration TimeToLive
	{
		get
		{
			return TimeToLiveField;
		}
		set
		{
			TimeToLiveField = value;
		}
	}

	[DataMember]
	public bool TimeToLiveLimited
	{
		get
		{
			return TimeToLiveLimitedField;
		}
		set
		{
			TimeToLiveLimitedField = value;
		}
	}

	[DataMember]
	public string Token
	{
		get
		{
			return TokenField;
		}
		set
		{
			TokenField = value;
		}
	}

	ITimeDuration ILoginInfo.TimeToLive
	{
		get
		{
			return TimeToLive;
		}
		set
		{
			TimeToLive = (TimeDuration)value;
		}
	}
}
