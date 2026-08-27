using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.WebApi.Models.Server;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class HardwareInitialSetupModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid RecorderId { get; set; }

	[Required]
	public Uri HardwareUri { get; set; }

	public int DriverNumber { get; set; }

	public HardwareUserModel HardwareUser { get; set; }

	public int? SecurePort { get; set; }
}
