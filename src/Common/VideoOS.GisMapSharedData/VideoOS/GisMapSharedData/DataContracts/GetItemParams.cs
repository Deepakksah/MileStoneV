using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetItemParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetItemParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
