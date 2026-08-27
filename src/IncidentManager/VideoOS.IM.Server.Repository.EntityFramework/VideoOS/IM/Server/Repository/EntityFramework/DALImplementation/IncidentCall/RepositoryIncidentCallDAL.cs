using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentCall;

public class RepositoryIncidentCallDAL : RepositoryEntityObjectBaseDAL<RepoIncidentCall>, IRepositoryIncidentCallDAL, IRepositoryObjectBaseDAL<long, RepoIncidentCall>
{
	public RepositoryIncidentCallDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}
}
