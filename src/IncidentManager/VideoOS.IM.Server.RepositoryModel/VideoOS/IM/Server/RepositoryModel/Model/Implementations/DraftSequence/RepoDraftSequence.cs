using System;
using System.Collections.Generic;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.DraftSequence;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;

public class RepoDraftSequence : ARepoBaseEntity, IRepoDraftSequence, IRepoBaseEntity
{
	public string Title { get; set; }

	public DateTime? TimeStamp { get; set; }

	public string Description { get; set; }

	public string DraftSequenceIdentificationMsUSerId { get; set; }

	public virtual List<RepoIncidentSequence> SequenceList { get; set; }
}
