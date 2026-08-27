using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.IM.Common.GlobalDefinitions.Localization;
using VideoOS.IM.Server.RepositoryModel.CommonCode;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.IncidentCall;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;

public class RepoIncidentCall : ARepoBaseEntity, IRepoIncidentCall, IRepoBaseEntity, ILoggingParametersProvider
{
	public DateTime? Called { get; set; }

	public DateTime? Arrived { get; set; }

	public DateTime? Departed { get; set; }

	public string Notes { get; set; }

	public long? IncidentId { get; set; }

	public virtual RepoIncident Incident { get; set; }

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters()
	{
		return new List<LocalizableDescriptionParameter>
		{
			new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)),
			new LocalizableDescriptionParameter("Called", Called.GetValueOrDefault().GetType(), Called?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
			new LocalizableDescriptionParameter("Arrived", Arrived.GetValueOrDefault().GetType(), Arrived?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
			new LocalizableDescriptionParameter("Departed", Departed.GetValueOrDefault().GetType(), Departed?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
			new LocalizableDescriptionParameter("Notes", Notes.GetType(), Notes.Truncate())
		};
	}

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters(IRepoBaseEntity previousValues)
	{
		List<LocalizableDescriptionParameter> list = new List<LocalizableDescriptionParameter>();
		if (!(previousValues is RepoIncidentCall repoIncidentCall))
		{
			return GetLoggingParameters();
		}
		if (!object.Equals(repoIncidentCall.Id, base.Id))
		{
			list.Add(new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)));
		}
		if (!object.Equals(repoIncidentCall.Called, Called))
		{
			list.Add(new LocalizableDescriptionParameter("Called", Called.GetValueOrDefault().GetType(), Called?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
		}
		if (!object.Equals(repoIncidentCall.Arrived, Arrived))
		{
			list.Add(new LocalizableDescriptionParameter("Arrived", Arrived.GetValueOrDefault().GetType(), Arrived?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
		}
		if (!object.Equals(repoIncidentCall.Departed, Departed))
		{
			list.Add(new LocalizableDescriptionParameter("Departed", Departed.GetValueOrDefault().GetType(), Departed?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
		}
		if (!object.Equals(repoIncidentCall.Notes, Notes))
		{
			list.Add(new LocalizableDescriptionParameter("Notes", Notes.GetType(), Notes.Truncate()));
		}
		return list;
	}
}
