using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.GlobalSettings;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.GlobalSettings;

public class RepositoryGlobalSettingsDAL : RepositoryEntityObjectBaseDAL<RepoGlobalSettings>, IRepositoryGlobalSettingsDAL, IRepositoryObjectBaseDAL<long, RepoGlobalSettings>
{
	public RepositoryGlobalSettingsDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}
}
