using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMapParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }

	[DataProperty]
	public string Language { get; set; }
}
