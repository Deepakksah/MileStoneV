using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddLayersParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddLayersParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerData[] Keys { get; set; }
}
