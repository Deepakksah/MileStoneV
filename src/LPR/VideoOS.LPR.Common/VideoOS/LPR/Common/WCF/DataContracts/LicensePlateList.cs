using System;
using System.Runtime.Serialization;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlateList : IExtensibleDataObject
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public LicensePlatePropertyDefinition[] PropertyDefinitions { get; set; }

	[DataMember]
	public FQID[] TriggerEvents { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
