using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "AspectRatioInfo")]
public enum AspectRatio
{
	[EnumMember]
	Aspect16x9,
	[EnumMember]
	Aspect4x3
}
