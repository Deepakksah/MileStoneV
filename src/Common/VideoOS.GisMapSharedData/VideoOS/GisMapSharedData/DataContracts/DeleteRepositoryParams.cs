using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteRepositoryParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteRepositoryParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
