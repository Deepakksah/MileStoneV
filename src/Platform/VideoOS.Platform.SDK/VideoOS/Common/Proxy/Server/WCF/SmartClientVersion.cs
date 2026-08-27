using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SmartClientVersion", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class SmartClientVersion : IExtensibleDataObject, ISmartClientVersion
{
	private ExtensionDataObject extensionDataField;

	private int BetaField;

	private string DisplayNameField;

	private bool ForceUpgradeField;

	private int MajorField;

	private int MinorField;

	private string RevisionField;

	private UpgradeInfo UpgradeInfoField;

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
	public int Beta
	{
		get
		{
			return BetaField;
		}
		set
		{
			BetaField = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public bool ForceUpgrade
	{
		get
		{
			return ForceUpgradeField;
		}
		set
		{
			ForceUpgradeField = value;
		}
	}

	[DataMember]
	public int Major
	{
		get
		{
			return MajorField;
		}
		set
		{
			MajorField = value;
		}
	}

	[DataMember]
	public int Minor
	{
		get
		{
			return MinorField;
		}
		set
		{
			MinorField = value;
		}
	}

	[DataMember]
	public string Revision
	{
		get
		{
			return RevisionField;
		}
		set
		{
			RevisionField = value;
		}
	}

	[DataMember]
	public UpgradeInfo UpgradeInfo
	{
		get
		{
			return UpgradeInfoField;
		}
		set
		{
			UpgradeInfoField = value;
		}
	}

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
}
