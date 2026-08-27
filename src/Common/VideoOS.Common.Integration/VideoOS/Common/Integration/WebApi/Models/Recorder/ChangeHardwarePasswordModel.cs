using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class ChangeHardwarePasswordModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	[Required]
	public string NewPassword { get; set; }

	public string TargetUserName { get; set; }
}
