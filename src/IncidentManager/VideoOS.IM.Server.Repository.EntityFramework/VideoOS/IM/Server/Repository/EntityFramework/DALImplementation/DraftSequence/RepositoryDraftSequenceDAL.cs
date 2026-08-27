using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.DraftSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DraftSequence;

public class RepositoryDraftSequenceDAL : RepositoryEntityObjectBaseDAL<RepoDraftSequence>, IRepositoryDraftSequenceDAL, IRepositoryObjectBaseDAL<long, RepoDraftSequence>
{
	public RepositoryDraftSequenceDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}
}
