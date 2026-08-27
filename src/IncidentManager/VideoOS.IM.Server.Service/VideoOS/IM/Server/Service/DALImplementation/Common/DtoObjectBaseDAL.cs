using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.Common;

public abstract class DtoObjectBaseDAL
{
	[NotNull]
	protected readonly IRepositoryDAL _dbService;

	protected DtoObjectBaseDAL(IRepositoryDAL dbService)
	{
		dbService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\Common\\DtoObjectBaseDAL.cs", 13);
		_dbService = dbService;
	}
}
