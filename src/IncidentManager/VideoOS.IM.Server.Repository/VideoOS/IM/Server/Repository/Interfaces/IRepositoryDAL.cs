using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Server.Repository.Interfaces.DraftSequence;
using VideoOS.IM.Server.Repository.Interfaces.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.Repository.Interfaces.GlobalSettings;
using VideoOS.IM.Server.Repository.Interfaces.Incident;
using VideoOS.IM.Server.Repository.Interfaces.IncidentActivityLog;
using VideoOS.IM.Server.Repository.Interfaces.IncidentCall;
using VideoOS.IM.Server.Repository.Interfaces.IncidentComment;
using VideoOS.IM.Server.Repository.Interfaces.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.Interfaces;

public interface IRepositoryDAL
{
	IRepositoryIncidentDAL GetIncidentDAL();

	IRepositoryDraftSequenceDAL GetDraftSequenceDAL();

	IRepositoryDynamicalDataTypesAndOptionsDAL GetDynamicalDataTypesAndOptionsDAL();

	IRepositoryIncidentCallDAL GetIncidentCallDAL();

	IRepositoryIncidentSequenceDAL GetIncidentSequenceDAL();

	IRepositoryIncidentCommentDAL GetIncidentCommentDAL();

	IRepositoryIncidentActivityLogDAL GetIncidentActivityLogDAL();

	IRepositoryGlobalSettingsDAL GetGlobalSettingsDAL();

	Task<RepositoryResponseBase<int>> SaveChangesAsync(CancellationToken cancellationToken);

	Task UpdateObjectPropertiesFromDbAsync(ARepoBaseEntity inObject, CancellationToken cancellationToken);
}
