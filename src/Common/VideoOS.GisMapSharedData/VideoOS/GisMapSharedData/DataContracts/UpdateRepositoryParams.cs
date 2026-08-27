using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Repository;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateRepositoryParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateRepositoryParams : ParamsRespBase
{
	[DataProperty]
	public RepositoryData Key { get; set; }
}
