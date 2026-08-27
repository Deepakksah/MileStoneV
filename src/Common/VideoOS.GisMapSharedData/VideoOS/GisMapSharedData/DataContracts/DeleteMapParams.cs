using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteMapParams : ParamsRespBase
{
	[DataProperty]
	public GisMapBrowseData Key { get; set; }
}
