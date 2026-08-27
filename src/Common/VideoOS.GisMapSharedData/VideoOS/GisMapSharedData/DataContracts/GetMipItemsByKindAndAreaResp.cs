using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Mip;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemsByKindAndAreaResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemsByKindAndAreaResp : UnsupportedRespBase
{
	[DataProperty]
	public MipItemData[] Items { get; set; }
}
