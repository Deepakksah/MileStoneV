using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetItemsByLayerParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetItemsByLayerParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerBrowseData Key { get; set; }
}
