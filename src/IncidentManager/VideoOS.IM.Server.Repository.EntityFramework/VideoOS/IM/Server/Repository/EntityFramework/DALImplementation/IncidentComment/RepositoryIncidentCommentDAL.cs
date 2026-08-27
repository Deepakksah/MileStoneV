using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentComment;

public class RepositoryIncidentCommentDAL : RepositoryEntityObjectBaseDAL<RepoIncidentComment>, IRepositoryIncidentCommentDAL, IRepositoryObjectBaseDAL<long, RepoIncidentComment>
{
	public RepositoryIncidentCommentDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}
}
