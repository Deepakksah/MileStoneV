using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "BrowseMapsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class BrowseMapsParams : ParamsRespBase
{
	[DataProperty]
	public string NamePattern { get; set; }

	[DataProperty]
	public long MaxResults { get; set; }
}
