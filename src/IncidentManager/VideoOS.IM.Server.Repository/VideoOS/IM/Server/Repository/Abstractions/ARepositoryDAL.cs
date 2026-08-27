using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Server.Repository.Interfaces;
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

namespace VideoOS.IM.Server.Repository.Abstractions;

public abstract class ARepositoryDAL : IRepositoryDAL
{
	public IRepositoryIncidentDAL IncidentDALService { get; set; }

	public IRepositoryDraftSequenceDAL DraftSequenceDALService { get; set; }

	public IRepositoryDynamicalDataTypesAndOptionsDAL DynamicalDataTypesAndOptionsDALService { get; set; }

	public IRepositoryIncidentCallDAL IncidentCallDALService { get; set; }

	public IRepositoryIncidentSequenceDAL IncidentSequenceDALService { get; set; }

	public IRepositoryIncidentCommentDAL IncidentCommentDALService { get; set; }

	public IRepositoryIncidentActivityLogDAL IncidentActivityLogDALService { get; set; }

	public IRepositoryGlobalSettingsDAL GlobalSettingsDALService { get; set; }

	public virtual IRepositoryIncidentDAL GetIncidentDAL()
	{
		return IncidentDALService;
	}

	public virtual IRepositoryDraftSequenceDAL GetDraftSequenceDAL()
	{
		return DraftSequenceDALService;
	}

	public virtual IRepositoryDynamicalDataTypesAndOptionsDAL GetDynamicalDataTypesAndOptionsDAL()
	{
		return DynamicalDataTypesAndOptionsDALService;
	}

	public virtual IRepositoryIncidentCallDAL GetIncidentCallDAL()
	{
		return IncidentCallDALService;
	}

	public virtual IRepositoryIncidentSequenceDAL GetIncidentSequenceDAL()
	{
		return IncidentSequenceDALService;
	}

	public virtual IRepositoryIncidentCommentDAL GetIncidentCommentDAL()
	{
		return IncidentCommentDALService;
	}

	public virtual IRepositoryIncidentActivityLogDAL GetIncidentActivityLogDAL()
	{
		return IncidentActivityLogDALService;
	}

	public virtual IRepositoryGlobalSettingsDAL GetGlobalSettingsDAL()
	{
		return GlobalSettingsDALService;
	}

	public abstract Task<RepositoryResponseBase<int>> SaveChangesAsync(CancellationToken cancellationToken);

	public abstract Task UpdateObjectPropertiesFromDbAsync(ARepoBaseEntity inObject, CancellationToken cancellationToken);
}
