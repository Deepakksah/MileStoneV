using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentActivityLog;

public class RepositoryIncidentActivityLogDAL : RepositoryEntityObjectBaseDAL<RepoIncidentActivityLog>, IRepositoryIncidentActivityLogDAL, IRepositoryObjectBaseDAL<long, RepoIncidentActivityLog>
{
	public RepositoryIncidentActivityLogDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}
}
