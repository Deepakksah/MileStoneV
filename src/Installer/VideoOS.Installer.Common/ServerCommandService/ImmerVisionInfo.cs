using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ImmerVisionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ImmerVisionInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string LensProfileDataField;

	private double LensProfileFieldOfViewField;

	private string LensProfileNameField;

	private string LensProfileRplField;

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
	public string LensProfileData
	{
		get
		{
			return LensProfileDataField;
		}
		set
		{
			LensProfileDataField = value;
		}
	}

	[DataMember]
	public double LensProfileFieldOfView
	{
		get
		{
			return LensProfileFieldOfViewField;
		}
		set
		{
			LensProfileFieldOfViewField = value;
		}
	}

	[DataMember]
	public string LensProfileName
	{
		get
		{
			return LensProfileNameField;
		}
		set
		{
			LensProfileNameField = value;
		}
	}

	[DataMember]
	public string LensProfileRpl
	{
		get
		{
			return LensProfileRplField;
		}
		set
		{
			LensProfileRplField = value;
		}
	}
}
