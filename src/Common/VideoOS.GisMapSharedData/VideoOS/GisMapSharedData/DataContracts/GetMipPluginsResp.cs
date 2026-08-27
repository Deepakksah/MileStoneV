using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Mip;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipPluginsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipPluginsResp : UnsupportedRespBase
{
	[DataProperty]
	public MipPluginData[] Plugins { get; set; }
}
