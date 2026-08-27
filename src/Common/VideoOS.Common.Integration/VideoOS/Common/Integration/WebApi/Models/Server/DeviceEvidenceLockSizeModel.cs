using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class DeviceEvidenceLockSizeModel
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }

	[Required]
	public long Size { get; set; }
}
