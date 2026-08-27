using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "PanoramicLensInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PanoramicLensInfo : IPanoramicLensInfo
{
	private string CameraMountField;

	private ImmerVisionInfo ImmerVisionField;

	private bool PanoramicLensEnabledField;

	private string PanoramicLensTypeField;

	[DataMember]
	public string CameraMount
	{
		get
		{
			return CameraMountField;
		}
		set
		{
			CameraMountField = value;
		}
	}

	[DataMember]
	public ImmerVisionInfo ImmerVision
	{
		get
		{
			return ImmerVisionField;
		}
		set
		{
			ImmerVisionField = value;
		}
	}

	[DataMember]
	public bool PanoramicLensEnabled
	{
		get
		{
			return PanoramicLensEnabledField;
		}
		set
		{
			PanoramicLensEnabledField = value;
		}
	}

	[DataMember]
	public string PanoramicLensType
	{
		get
		{
			return PanoramicLensTypeField;
		}
		set
		{
			PanoramicLensTypeField = value;
		}
	}

	IImmerVisionInfo IPanoramicLensInfo.ImmerVision
	{
		get
		{
			return ImmerVision;
		}
		set
		{
			ImmerVision = (ImmerVisionInfo)value;
		}
	}
}
