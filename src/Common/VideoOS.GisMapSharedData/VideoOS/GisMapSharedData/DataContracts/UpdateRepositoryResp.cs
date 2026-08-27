using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateRepositoryResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateRepositoryResp : UnsupportedRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
