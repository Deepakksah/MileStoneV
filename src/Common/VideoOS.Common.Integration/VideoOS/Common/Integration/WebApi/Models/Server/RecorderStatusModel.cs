using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RecorderStatusModel : CommandModelBase
{
	[EnumDataType(typeof(RecorderStatusesDto))]
	public RecorderStatusesDto Statuses { get; set; }
}
