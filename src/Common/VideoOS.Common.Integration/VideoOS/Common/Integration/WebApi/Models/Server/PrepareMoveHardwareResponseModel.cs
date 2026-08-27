using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class PrepareMoveHardwareResponseModel : ResponseModel
{
	[Required]
	public IEnumerable<DeviceTrackIdDto> DeviceTracksWithLiveTable { get; set; }
}
