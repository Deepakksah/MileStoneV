using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class DatabaseResponseDiskUsageModel : ResponseModel
{
	[Required]
	public DatabaseResponseDiskUsageDto DatabaseResponseDiskUsage { get; set; }
}
