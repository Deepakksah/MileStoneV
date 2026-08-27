using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Repository;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetRepositoryResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetRepositoryResp : ParamsRespBase
{
	[DataProperty]
	public RepositoryData Repository { get; set; }
}
