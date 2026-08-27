using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class PtzSessionInfo
{
	private Guid deviceIdField;

	private Guid patrollingProfileIdField;

	private Guid manualPatrollingProfileIdField;

	private UserInfo userField;

	private int priorityField;

	private bool reservedField;

	private bool isPatrollingField;

	private bool isAnonymousField;

	private bool isPausedField;

	private int timeoutSecField;

	private int timeoutLeftSecField;

	private bool isLicensedField;

	[XmlElement(Order = 0)]
	public Guid DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	[XmlElement(Order = 1)]
	public Guid PatrollingProfileId
	{
		get
		{
			return patrollingProfileIdField;
		}
		set
		{
			patrollingProfileIdField = value;
		}
	}

	[XmlElement(Order = 2)]
	public Guid ManualPatrollingProfileId
	{
		get
		{
			return manualPatrollingProfileIdField;
		}
		set
		{
			manualPatrollingProfileIdField = value;
		}
	}

	[XmlElement(Order = 3)]
	public UserInfo User
	{
		get
		{
			return userField;
		}
		set
		{
			userField = value;
		}
	}

	[XmlElement(Order = 4)]
	public int Priority
	{
		get
		{
			return priorityField;
		}
		set
		{
			priorityField = value;
		}
	}

	[XmlElement(Order = 5)]
	public bool Reserved
	{
		get
		{
			return reservedField;
		}
		set
		{
			reservedField = value;
		}
	}

	[XmlElement(Order = 6)]
	public bool IsPatrolling
	{
		get
		{
			return isPatrollingField;
		}
		set
		{
			isPatrollingField = value;
		}
	}

	[XmlElement(Order = 7)]
	public bool IsAnonymous
	{
		get
		{
			return isAnonymousField;
		}
		set
		{
			isAnonymousField = value;
		}
	}

	[XmlElement(Order = 8)]
	public bool IsPaused
	{
		get
		{
			return isPausedField;
		}
		set
		{
			isPausedField = value;
		}
	}

	[XmlElement(Order = 9)]
	public int TimeoutSec
	{
		get
		{
			return timeoutSecField;
		}
		set
		{
			timeoutSecField = value;
		}
	}

	[XmlElement(Order = 10)]
	public int TimeoutLeftSec
	{
		get
		{
			return timeoutLeftSecField;
		}
		set
		{
			timeoutLeftSecField = value;
		}
	}

	[XmlElement(Order = 11)]
	public bool IsLicensed
	{
		get
		{
			return isLicensedField;
		}
		set
		{
			isLicensedField = value;
		}
	}
}
