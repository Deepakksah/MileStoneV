using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Incident;

public class RepositoryIncidentDAL : RepositoryEntityObjectBaseDAL<RepoIncident>, IRepositoryIncidentDAL, IRepositoryObjectBaseDAL<long, RepoIncident>
{
	public RepositoryIncidentDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}

	public override async Task<RepositoryResponseBase<RepoIncident>> CreateAsync(RepoIncident inObject, CancellationToken cancellationToken)
	{
		if (inObject != null && !inObject.Start.HasValue)
		{
			inObject.Start = DateTime.UtcNow;
		}
		return await base.CreateAsync(inObject, cancellationToken);
	}
}
