using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "ActivatePluginActionParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class ActivatePluginActionParams : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }

	[DataProperty]
	public Guid Kind { get; set; }

	[DataProperty]
	public string ActionId { get; set; }
}
