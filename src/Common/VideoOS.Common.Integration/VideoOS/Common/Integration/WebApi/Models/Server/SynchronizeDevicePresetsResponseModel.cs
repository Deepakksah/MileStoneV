using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class SynchronizeDevicePresetsResponseModel : ResponseModel
{
	[Required]
	public string PresetsXml { get; set; }
}
