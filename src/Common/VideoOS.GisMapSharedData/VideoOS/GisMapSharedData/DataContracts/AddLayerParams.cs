using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddLayerParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddLayerParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerData Key { get; set; }
}
