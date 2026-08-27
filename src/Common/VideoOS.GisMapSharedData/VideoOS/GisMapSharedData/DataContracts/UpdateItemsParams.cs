using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateItemsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateItemsParams : ParamsRespBase
{
	[DataProperty]
	public MapItemData[] Keys { get; set; }
}
