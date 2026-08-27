using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class LogEntrySystemModel : LogEntryModel
{
	public Guid EventTypeId { get; set; }

	public string EventType { get; set; }
}
