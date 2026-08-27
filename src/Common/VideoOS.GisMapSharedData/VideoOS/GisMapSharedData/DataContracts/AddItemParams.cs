using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddItemParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddItemParams : ParamsRespBase
{
	[DataProperty]
	public MapItemData Key { get; set; }
}
