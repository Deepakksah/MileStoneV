using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetItemsByLayerResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetItemsByLayerResp : UnsupportedRespBase
{
	[DataProperty]
	public MapItemData[] Items { get; set; }
}
