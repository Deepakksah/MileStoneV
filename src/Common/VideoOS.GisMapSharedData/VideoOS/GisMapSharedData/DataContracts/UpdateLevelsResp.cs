using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateLevelsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateLevelsResp : UnsupportedRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
