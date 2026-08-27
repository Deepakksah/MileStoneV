using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ImmerVisionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ImmerVisionInfo : IExtensibleDataObject, IImmerVisionInfo
{
	private ExtensionDataObject extensionDataField;

	private string LensProfileDataField;

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
