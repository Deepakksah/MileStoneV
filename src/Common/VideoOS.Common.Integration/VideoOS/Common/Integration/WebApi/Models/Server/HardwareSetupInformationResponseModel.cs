using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class HardwareSetupInformationResponseModel : ResponseModel
{
	[Required]
	public HardwareSetupInformationDto HardwareSetupInformation { get; set; }
}
