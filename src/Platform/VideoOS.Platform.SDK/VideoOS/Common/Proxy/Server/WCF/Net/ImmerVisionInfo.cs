using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ImmerVisionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ImmerVisionInfo : IImmerVisionInfo
{
	private string LensProfileDataField;

	private double LensProfileFieldOfViewField;

	private string LensProfileNameField;

	private string LensProfileRplField;

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
