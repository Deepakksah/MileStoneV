using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ReportMediaStatusModel : CommandModelBase
{
	public IEnumerable<MediaStatusEntryModel> MediaStatus { get; set; }

	public IEnumerable<TrackStatusEntryModel> TrackStatus { get; set; }
}
