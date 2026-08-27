using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class LogEntryModel
{
	public IEnumerable<string> Arguments { get; set; }

	public DateTime TimestampLocal { get; set; }

	public DateTime TimestampUtc { get; set; }

	public int LogId { get; set; }

	[Obsolete("Property is no longer used - always null")]
	public string LogIdString { get; set; }

	public LogLevel LogLevel { get; set; }

	public string LogLevelValue { get; set; }

	public Guid SourceId { get; set; }

	public string SourceTypeValue { get; set; }

	public string SourceName { get; set; }

	public string Description { get; set; }

	public string MigratedLogDescription { get; set; }

	public LogEntryDefinition LogEntryDefinition { get; set; }
}
