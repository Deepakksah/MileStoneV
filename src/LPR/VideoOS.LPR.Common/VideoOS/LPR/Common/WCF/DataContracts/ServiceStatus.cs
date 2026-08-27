using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class ServiceStatus : IExtensibleDataObject
{
	[DataMember]
	public DateTime ServiceStartTime { get; set; }

	[DataMember]
	public double MachineCpuUsed { get; set; }

	[DataMember]
	public double MachineMemoryTotal { get; set; }

	[DataMember]
	public double MachineMemoryAvailable { get; set; }

	[DataMember]
	public int TotalDetectedLicensePlates { get; set; }

	[DataMember]
	public int PendingDetectedLicensePlates { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
