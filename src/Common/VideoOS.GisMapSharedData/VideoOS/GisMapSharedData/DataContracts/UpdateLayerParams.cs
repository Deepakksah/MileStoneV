using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateLayerParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateLayerParams : ParamsRespBase
{
	[DataProperty]
	public MapLayerData Key { get; set; }
}
