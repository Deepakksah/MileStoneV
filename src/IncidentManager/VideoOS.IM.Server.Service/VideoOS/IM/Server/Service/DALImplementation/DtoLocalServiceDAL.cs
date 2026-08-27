using VideoOS.IM.Common.Dto.DtoModels.Interfaces.User;
using VideoOS.IM.Common.Service.Abstractions;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.DraftSequence;
using VideoOS.IM.Server.Service.DALImplementation.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.Service.DALImplementation.GlobalSettings;
using VideoOS.IM.Server.Service.DALImplementation.Incident;
using VideoOS.IM.Server.Service.DALImplementation.IncidentActivityLog;
using VideoOS.IM.Server.Service.DALImplementation.IncidentCall;
using VideoOS.IM.Server.Service.DALImplementation.IncidentComment;
using VideoOS.IM.Server.Service.DALImplementation.IncidentSequence;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation;

public class DtoLocalServiceDAL : ADtoDAL
{
	public DtoLocalServiceDAL([NotNull] string connectionString, [NotNull] IDtoMilestoneUser dtoMilestoneUser)
		: this(connectionString)
	{
		dtoMilestoneUser.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\DtoLocalServiceDAL.cs", 21);
		InitializeGlobalSettingsManager(dtoMilestoneUser);
	}

	public DtoLocalServiceDAL([NotNull] string connectionString)
	{
		connectionString.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\DtoLocalServiceDAL.cs", 29);
		EntityFrameworkDAL inDbService = new EntityFrameworkDAL(connectionString);
		base.IncidentDALService = new DtoIncidentDAL(inDbService);
		base.DraftSequenceDALService = new DtoDraftSequenceDAL(inDbService);
		base.DynamicalDataTypesAndOptionsDALService = new DtoDynamicalDataTypesAndOptionsDAL(inDbService);
		base.IncidentCallDALService = new DtoIncidentCallDAL(inDbService);
		base.IncidentSequenceDALService = new DtoIncidentSequenceDAL(inDbService);
		base.IncidentCommentDALService = new DtoIncidentCommentDAL(inDbService);
		base.IncidentActivityLogDALService = new DtoIncidentActivityLogDAL(inDbService);
		base.GlobalSettingsDALService = new DtoGlobalSettingsDAL(inDbService);
	}
}
