using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddMapParams : ParamsRespBase
{
	[DataProperty]
	public GisMapData Key { get; set; }
}
