using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SystemConfigurationHasChangedParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ManagementServerId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
