using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemsByKindParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemsByKindParams : ParamsRespBase
{
	[DataProperty]
	public Guid Kind { get; set; }
}
