using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ServerFileSystemResponseModel : ResponseModel
{
	public ServerFileSystemOperationDto Operation { get; set; }

	[Required]
	public ServerFileSystemResponseStatusDto Status { get; set; }

	public string Message { get; set; }
}
