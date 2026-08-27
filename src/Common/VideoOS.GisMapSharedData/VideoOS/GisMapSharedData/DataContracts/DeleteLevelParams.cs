using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLevelParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLevelParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
