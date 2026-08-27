using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayersByMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayersByMapParams : ParamsRespBase
{
	[DataProperty]
	public GisMapBrowseData Key { get; set; }

	[DataProperty]
	public string Language { get; set; }
}
