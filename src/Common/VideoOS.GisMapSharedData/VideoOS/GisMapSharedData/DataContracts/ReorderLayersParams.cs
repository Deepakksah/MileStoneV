using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "ReorderLayersParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class ReorderLayersParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerBrowseData[] Keys { get; set; }
}
