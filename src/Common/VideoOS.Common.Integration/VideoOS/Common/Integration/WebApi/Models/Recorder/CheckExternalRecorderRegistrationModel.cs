using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class CheckExternalRecorderRegistrationModel : CommandModelBase
{
	[Required]
	public Guid HardwareId { get; set; }

	[Required]
	public string Address { get; set; }
}
