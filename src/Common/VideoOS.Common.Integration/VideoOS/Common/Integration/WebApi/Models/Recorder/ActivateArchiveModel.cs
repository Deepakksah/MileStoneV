using System;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class ActivateArchiveModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid ArchiveStorageId { get; set; }
}
