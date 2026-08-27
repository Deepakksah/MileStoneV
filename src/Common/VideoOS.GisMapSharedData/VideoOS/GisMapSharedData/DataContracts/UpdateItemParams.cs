using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateItemParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateItemParams : ParamsRespBase
{
	[DataProperty]
	public MapItemData Key { get; set; }
}
