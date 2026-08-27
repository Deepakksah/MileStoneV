using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class UpdateRecorderTaskProgressModel
{
	public int PercentComplete { get; set; }

	public string Details { get; set; }

	public int Status { get; set; }

	public DateTime EndTime { get; set; }
}
