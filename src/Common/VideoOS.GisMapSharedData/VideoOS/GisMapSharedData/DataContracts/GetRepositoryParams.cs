using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetRepositoryParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetRepositoryParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
