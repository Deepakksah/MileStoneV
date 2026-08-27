using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IpixInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class IpixInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool CeilingMountedField;

	private HemisphereInfo HemisphereField;

	private PositionInfo HomepositionField;

	private bool IpixEnabledField;

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
	public bool CeilingMounted
	{
		get
		{
			return CeilingMountedField;
		}
		set
		{
			CeilingMountedField = value;
		}
	}

	[DataMember]
	public HemisphereInfo Hemisphere
	{
		get
		{
			return HemisphereField;
		}
		set
		{
			HemisphereField = value;
		}
	}

	[DataMember]
	public PositionInfo Homeposition
	{
		get
		{
			return HomepositionField;
		}
		set
		{
			HomepositionField = value;
		}
	}

	[DataMember]
	public bool IpixEnabled
	{
		get
		{
			return IpixEnabledField;
		}
		set
		{
			IpixEnabledField = value;
		}
	}
}
