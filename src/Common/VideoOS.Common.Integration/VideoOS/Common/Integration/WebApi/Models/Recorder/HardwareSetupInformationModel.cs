using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class HardwareSetupInformationModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }

	[Required]
	public Uri Address { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string CustomDriverData { get; set; }

	[Required]
	public int DriverNumber { get; set; }

	public string AcceptMacAddress { get; set; }

	[Required]
	public bool AacSupported { get; set; }

	public int? HttpsPort { get; set; }
}
