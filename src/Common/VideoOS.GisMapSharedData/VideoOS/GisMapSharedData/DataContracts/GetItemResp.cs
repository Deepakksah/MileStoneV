using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetItemResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetItemResp : ParamsRespBase
{
	[DataProperty]
	public MapItemData Item { get; set; }
}
