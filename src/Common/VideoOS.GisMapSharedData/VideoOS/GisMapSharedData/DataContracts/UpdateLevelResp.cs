using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateLevelResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateLevelResp : UnsupportedRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
