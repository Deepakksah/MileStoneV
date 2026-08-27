using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class LoginInfo : ILoginInfo
{
	private string tokenField;

	private DateTime registrationTimeField;

	private TimeDuration timeToLiveField;

	private bool timeToLiveLimitedField;

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

	public string Token
	{
		get
		{
			return tokenField;
		}
		set
		{
			tokenField = value;
		}
	}

	public DateTime RegistrationTime
	{
		get
		{
			return registrationTimeField;
		}
		set
		{
			registrationTimeField = value;
		}
	}

	public TimeDuration TimeToLive
	{
		get
		{
			return timeToLiveField;
		}
		set
		{
			timeToLiveField = value;
		}
	}

	public bool TimeToLiveLimited
	{
		get
		{
			return timeToLiveLimitedField;
		}
		set
		{
			timeToLiveLimitedField = value;
		}
	}
}
