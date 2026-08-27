using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class DatabaseOperationModel : CommandModelBase
{
	public DatabaseOperationDto DatabaseOperation { get; set; }

	public Guid RecorderId { get; set; }

	public Guid DeviceId { get; set; }

	public IEnumerable<Guid> DeviceIds { get; set; }

	public IEnumerable<Guid> TrackIds { get; set; }

	public Guid StorageId { get; set; }

	public Guid ArchiveId { get; set; }

	public string Path { get; set; }

	public bool NewPath { get; set; }

	public string ArchiveDefinition { get; set; }
}
