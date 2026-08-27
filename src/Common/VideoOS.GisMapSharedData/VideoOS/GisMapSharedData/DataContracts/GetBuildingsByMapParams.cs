using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetBuildingsByMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetBuildingsByMapParams : ParamsRespBase
{
	[DataProperty]
	public GisMapBrowseData Key { get; set; }
}
