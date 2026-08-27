using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteBuildingsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteBuildingsParams : ParamsRespBase
{
	[DataProperty]
	public Guid[] Keys { get; set; }
}
