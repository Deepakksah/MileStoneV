using System.Collections.Generic;
using VideoOS.IM.Common.GlobalDefinitions.Localization;

namespace VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;

public interface ILoggingMultilineProvider
{
	IEnumerable<LocalizableDescriptionWrapper> GetChildEntries();

	IEnumerable<LocalizableDescriptionWrapper> GetChildEntries(IRepoBaseEntity previousValues);
}
