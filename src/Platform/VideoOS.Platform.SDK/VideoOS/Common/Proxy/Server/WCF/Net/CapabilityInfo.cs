using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "CapabilityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CapabilityInfo : ICapabilityInfo
{
	private bool AbsoluteField;

	private bool AutomaticField;

	private Guid CapabilityIdField;

	private string NameField;

	private bool RelativeField;

	private bool SpeedField;

	private bool StartField;

	private bool StopField;

	[DataMember]
	public bool Absolute
	{
		get
		{
			return AbsoluteField;
		}
		set
		{
			AbsoluteField = value;
		}
	}

	[DataMember]
	public bool Automatic
	{
		get
		{
			return AutomaticField;
		}
		set
		{
			AutomaticField = value;
		}
	}

	[DataMember]
	public Guid CapabilityId
	{
		get
		{
			return CapabilityIdField;
		}
		set
		{
			CapabilityIdField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public bool Relative
	{
		get
		{
			return RelativeField;
		}
		set
		{
			RelativeField = value;
		}
	}

	[DataMember]
	public bool Speed
	{
		get
		{
			return SpeedField;
		}
		set
		{
			SpeedField = value;
		}
	}

	[DataMember]
	public bool Start
	{
		get
		{
			return StartField;
		}
		set
		{
			StartField = value;
		}
	}

	[DataMember]
	public bool Stop
	{
		get
		{
			return StopField;
		}
		set
		{
			StopField = value;
		}
	}
}
