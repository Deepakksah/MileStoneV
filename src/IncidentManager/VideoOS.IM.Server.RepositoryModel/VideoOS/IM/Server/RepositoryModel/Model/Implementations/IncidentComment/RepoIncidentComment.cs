using System.Collections.Generic;
using System.Globalization;
using VideoOS.IM.Common.GlobalDefinitions.Localization;
using VideoOS.IM.Server.RepositoryModel.CommonCode;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.IncidentComment;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;

public class RepoIncidentComment : ARepoBaseEntity, IRepoIncidentComment, IRepoBaseEntity, ILoggingParametersProvider
{
	public string User { get; set; }

	public string Comment { get; set; }

	public long IncidentId { get; set; }

	public virtual RepoIncident Incident { get; set; }

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters()
	{
		return new List<LocalizableDescriptionParameter>
		{
			new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)),
			new LocalizableDescriptionParameter("Comment", Comment.GetType(), Comment.Truncate())
		};
	}

	public IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters(IRepoBaseEntity previousValues)
	{
		List<LocalizableDescriptionParameter> list = new List<LocalizableDescriptionParameter>();
		if (!(previousValues is RepoIncidentComment repoIncidentComment))
		{
			return GetLoggingParameters();
		}
		if (!object.Equals(repoIncidentComment.Id, base.Id))
		{
			list.Add(new LocalizableDescriptionParameter("Id", base.Id.GetType(), base.Id.ToString(CultureInfo.InvariantCulture)));
		}
		if (!object.Equals(repoIncidentComment.Comment, Comment))
		{
			list.Add(new LocalizableDescriptionParameter("Comment", Comment.GetType(), Comment.Truncate()));
		}
		return list;
	}
}
