using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SmartClientVersion", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientVersion : IExtensibleDataObject
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
}
