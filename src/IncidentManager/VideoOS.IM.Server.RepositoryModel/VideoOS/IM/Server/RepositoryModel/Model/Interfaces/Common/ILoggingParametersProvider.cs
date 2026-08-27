using System.Collections.Generic;
using VideoOS.IM.Common.GlobalDefinitions.Localization;

namespace VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;

public interface ILoggingParametersProvider
{
	IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters();

	IEnumerable<LocalizableDescriptionParameter> GetLoggingParameters(IRepoBaseEntity previousValues);
}
