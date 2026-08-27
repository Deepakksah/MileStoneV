using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class FirmwareUpgradeModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	public string FileId { get; set; }
}
