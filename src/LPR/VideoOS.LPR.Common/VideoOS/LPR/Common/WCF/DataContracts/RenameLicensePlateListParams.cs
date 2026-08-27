using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class RenameLicensePlateListParams : IExtensibleDataObject
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
