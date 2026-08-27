using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class HardwareScanProgressResponseModel : ResponseModel
{
	[Required]
	public string ScanResultXml { get; set; }
}
