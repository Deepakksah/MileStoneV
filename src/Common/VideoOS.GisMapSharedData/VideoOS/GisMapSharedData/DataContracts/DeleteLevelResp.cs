using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLevelResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLevelResp : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
