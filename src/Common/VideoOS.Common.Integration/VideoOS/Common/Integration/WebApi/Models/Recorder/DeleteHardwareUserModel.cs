using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class DeleteHardwareUserModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid HardwareId { get; set; }

	[Required]
	public string UserName { get; set; }
}
