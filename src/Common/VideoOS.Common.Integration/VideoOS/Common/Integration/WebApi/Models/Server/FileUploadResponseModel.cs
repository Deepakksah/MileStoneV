using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class FileUploadResponseModel : ResponseModel
{
	[Required]
	public string StorageId { get; set; }
}
