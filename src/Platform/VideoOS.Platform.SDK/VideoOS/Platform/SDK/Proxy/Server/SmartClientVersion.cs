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
public class SmartClientVersion : ISmartClientVersion
{
	private int majorField;

	private int minorField;

	private string revisionField;

	private int betaField;

	private bool forceUpgradeField;

	private string displayNameField;

	private UpgradeInfo upgradeInfoField;

	IUpgradeInfo ISmartClientVersion.UpgradeInfo
	{
		get
		{
			return UpgradeInfo;
		}
		set
		{
			UpgradeInfo = (UpgradeInfo)value;
		}
	}

	public int Major
	{
		get
		{
			return majorField;
		}
		set
		{
			majorField = value;
		}
	}

	public int Minor
	{
		get
		{
			return minorField;
		}
		set
		{
			minorField = value;
		}
	}

	public string Revision
	{
		get
		{
			return revisionField;
		}
		set
		{
			revisionField = value;
		}
	}

	public int Beta
	{
		get
		{
			return betaField;
		}
		set
		{
			betaField = value;
		}
	}

	public bool ForceUpgrade
	{
		get
		{
			return forceUpgradeField;
		}
		set
		{
			forceUpgradeField = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return displayNameField;
		}
		set
		{
			displayNameField = value;
		}
	}

	public UpgradeInfo UpgradeInfo
	{
		get
		{
			return upgradeInfoField;
		}
		set
		{
			upgradeInfoField = value;
		}
	}
}
