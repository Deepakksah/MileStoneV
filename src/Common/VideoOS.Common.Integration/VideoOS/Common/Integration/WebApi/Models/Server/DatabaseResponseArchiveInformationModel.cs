using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class DatabaseResponseArchiveInformationModel : ResponseModel
{
	[Required]
	public DatabaseResponseArchiveInformationDto DatabaseResponseArchiveInformation { get; set; }
}
