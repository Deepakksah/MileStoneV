using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "BrowseMapsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class BrowseMapsResp : ParamsRespBase
{
	[DataProperty]
	public GisMapBrowseData[] Maps { get; set; }
}
