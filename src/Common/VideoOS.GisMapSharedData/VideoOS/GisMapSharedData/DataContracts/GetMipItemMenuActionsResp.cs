using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Mip;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemMenuActionsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemMenuActionsResp : UnsupportedRespBase
{
	[DataProperty]
	public MipItemMenuActionsData[] PluginActions { get; set; }
}
