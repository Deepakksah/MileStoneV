using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
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
