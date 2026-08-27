using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HemisphereInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class HemisphereInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double CenterXField;

	private double CenterYField;

	private double RadiusXField;

	private double RadiusYField;

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
	public double CenterX
	{
		get
		{
			return CenterXField;
		}
		set
		{
			CenterXField = value;
		}
	}

	[DataMember]
	public double CenterY
	{
		get
		{
			return CenterYField;
		}
		set
		{
			CenterYField = value;
		}
	}

	[DataMember]
	public double RadiusX
	{
		get
		{
			return RadiusXField;
		}
		set
		{
			RadiusXField = value;
		}
	}

	[DataMember]
	public double RadiusY
	{
		get
		{
			return RadiusYField;
		}
		set
		{
			RadiusYField = value;
		}
	}
}
