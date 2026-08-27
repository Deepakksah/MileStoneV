using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMipItemMenuActionsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMipItemMenuActionsParams : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }

	[DataProperty]
	public Guid Kind { get; set; }

	[DataProperty]
	public string Language { get; set; }
}
