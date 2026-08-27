using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayerParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayerParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerBrowseData Key { get; set; }
}
