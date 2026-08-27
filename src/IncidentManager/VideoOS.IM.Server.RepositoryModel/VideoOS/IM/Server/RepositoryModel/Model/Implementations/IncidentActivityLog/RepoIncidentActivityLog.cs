using System;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.IncidentActivityLog;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;

public class RepoIncidentActivityLog : ARepoBaseEntity, IRepoIncidentActivityLog, IRepoBaseEntity
{
	public DateTime? TimeOfActivityLog { get; set; }

	public string Description { get; set; }

	public string EntityType { get; set; }

	public string EntityId { get; set; }

	public string JsonOldValues { get; set; }

	public string JsonNewValues { get; set; }

	public long IncidentId { get; set; }

	public virtual RepoIncident Incident { get; set; }
}
