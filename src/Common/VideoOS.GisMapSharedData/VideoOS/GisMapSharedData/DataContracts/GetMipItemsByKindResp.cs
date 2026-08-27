using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemsByKindResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemsByKindResp : ParamsRespBase
{
	[DataProperty]
	public bool AnyItemsOnMap { get; set; }
}
