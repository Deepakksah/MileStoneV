using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipPluginsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipPluginsParams : ParamsRespBase
{
	[DataProperty]
	public string Language { get; set; }
}
