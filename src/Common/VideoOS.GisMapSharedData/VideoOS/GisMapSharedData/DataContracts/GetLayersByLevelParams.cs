using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayersByLevelParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayersByLevelParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
