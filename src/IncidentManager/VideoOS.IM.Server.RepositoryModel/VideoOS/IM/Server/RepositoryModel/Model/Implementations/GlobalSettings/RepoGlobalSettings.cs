using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.GlobalSettings;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;

public class RepoGlobalSettings : ARepoBaseEntity, IRepoGlobalSettings, IRepoBaseEntity
{
	public string GlobalSettingsIndentificator { get; set; }

	public string ValueJSon { get; set; }
}
