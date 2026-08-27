using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLevelsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLevelsParams : ParamsRespBase
{
	[DataProperty]
	public Guid[] Keys { get; set; }
}
