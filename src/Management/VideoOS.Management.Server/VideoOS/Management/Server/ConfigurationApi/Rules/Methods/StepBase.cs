using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public abstract class StepBase : IWizardStep
{
	protected readonly ActionDefinitionsProvider _definitionsProvider;

	public abstract bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item);

	public abstract List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey);

	public abstract List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey);

	protected StepBase(IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		_definitionsProvider = new ActionDefinitionsProvider(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
	}

	protected static IEnumerable<string> GetSelectedActionTypes(string actionsProperty)
	{
		return from s in actionsProperty.Split(';')
			select s.Trim();
	}
}
