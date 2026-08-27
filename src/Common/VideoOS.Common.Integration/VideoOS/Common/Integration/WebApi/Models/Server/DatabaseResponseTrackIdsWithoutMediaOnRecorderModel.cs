using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class DatabaseResponseTrackIdsWithoutMediaOnRecorderModel : ResponseModel
{
	[Required]
	public DatabaseResponseTrackIdsWithoutMediaOnRecorderDto DatabaseResponseTrackIdsWithoutMediaOnRecorder { get; set; }
}
