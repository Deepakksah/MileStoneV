using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ActivateArchivingModel : CommandModelBase
{
	public Guid ArchiveStorageId { get; set; }
}
