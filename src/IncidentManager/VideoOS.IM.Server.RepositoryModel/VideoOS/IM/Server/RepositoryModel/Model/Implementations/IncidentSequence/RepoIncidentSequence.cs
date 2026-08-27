using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.IM.Common.GlobalDefinitions.Localization;
using VideoOS.IM.Server.RepositoryModel.CommonCode;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.IncidentSequence;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;

public class RepoIncidentSequence : ARepoBaseEntity, IRepoIncidentSequence, IRepoBaseEntity, ILoggingParametersProvider
{
	public Guid CameraId { get; set; }

	public string CameraTitle { get; set; }

	public DateTime? StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public int OrderNumber { get; set; }

	public long? IncidentId { get; set; }

	public virtual RepoIncident Incident { get; set; }

	public long? DraftSequenceId { get; set; }

	public virtual RepoDraftSequence DraftSequence { get; set; }

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters()
	{
		return new List<LocalizableDescriptionParameter>
		{
			new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)),
			new LocalizableDescriptionParameter("CameraTitle", CameraTitle.GetType(), CameraTitle.Truncate()),
			new LocalizableDescriptionParameter("StartTime", StartTime.GetValueOrDefault().GetType(), StartTime?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
			new LocalizableDescriptionParameter("EndTime", EndTime.GetValueOrDefault().GetType(), EndTime?.ToString(CultureInfo.InvariantCulture) ?? string.Empty)
		};
	}

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters(IRepoBaseEntity previousValues)
	{
		List<LocalizableDescriptionParameter> list = new List<LocalizableDescriptionParameter>();
		if (!(previousValues is RepoIncidentSequence repoIncidentSequence))
		{
			return GetLoggingParameters();
		}
		if (!object.Equals(repoIncidentSequence.Id, base.Id))
		{
			list.Add(new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)));
		}
		if (!object.Equals(repoIncidentSequence.CameraTitle, CameraTitle))
		{
			list.Add(new LocalizableDescriptionParameter("CameraTitle", CameraTitle.GetType(), CameraTitle.Truncate()));
		}
		if (!object.Equals(repoIncidentSequence.StartTime, StartTime))
		{
			list.Add(new LocalizableDescriptionParameter("StartTime", StartTime.GetValueOrDefault().GetType(), StartTime?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
		}
		if (!object.Equals(repoIncidentSequence.EndTime, EndTime))
		{
			list.Add(new LocalizableDescriptionParameter("EndTime", EndTime.GetValueOrDefault().GetType(), EndTime?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
		}
		return list;
	}
}
