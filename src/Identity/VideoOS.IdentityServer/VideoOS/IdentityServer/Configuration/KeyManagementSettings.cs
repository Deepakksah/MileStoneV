using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Configuration;

public class KeyManagementSettings
{
	[Range(14, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
	public int RotationIntervalInDays { get; set; } = 90;

	[Range(7, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
	public int PropagationTimeInDays { get; set; } = 14;

	[Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
	public int RetentionDurationInDays { get; set; } = 14;
}
