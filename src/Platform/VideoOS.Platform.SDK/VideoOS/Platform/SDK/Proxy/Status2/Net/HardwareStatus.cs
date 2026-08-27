using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HardwareStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class HardwareStatus : StatusBase
{
	private Guid HardwareIdField;

	private bool IsChangeField;

	private bool EnabledField;

	private bool StartedField;

	private bool ErrorField;

	private bool ErrorNotLicensedField;

	private bool ErrorNoConnectionField;

	[DataMember(IsRequired = true)]
	public Guid HardwareId
	{
		get
		{
			return HardwareIdField;
		}
		set
		{
			HardwareIdField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public bool IsChange
	{
		get
		{
			return IsChangeField;
		}
		set
		{
			IsChangeField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public bool Enabled
	{
		get
		{
			return EnabledField;
		}
		set
		{
			EnabledField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public bool Started
	{
		get
		{
			return StartedField;
		}
		set
		{
			StartedField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 4)]
	public bool Error
	{
		get
		{
			return ErrorField;
		}
		set
		{
			ErrorField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 5)]
	public bool ErrorNotLicensed
	{
		get
		{
			return ErrorNotLicensedField;
		}
		set
		{
			ErrorNotLicensedField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 6)]
	public bool ErrorNoConnection
	{
		get
		{
			return ErrorNoConnectionField;
		}
		set
		{
			ErrorNoConnectionField = value;
		}
	}
}
