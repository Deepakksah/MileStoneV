using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLayersParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLayersParams : ParamsRespBase
{
	[DataProperty]
	public Guid[] Keys { get; set; }
}
