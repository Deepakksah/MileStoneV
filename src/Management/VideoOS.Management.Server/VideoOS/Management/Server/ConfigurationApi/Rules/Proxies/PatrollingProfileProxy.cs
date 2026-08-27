using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class PatrollingProfileProxy : IPatrollingProfileProxy
{
	public PatrollingProfile QueryPatrollingProfile(IGeminiRequestContext requestContext, string id)
	{
		return PatrollingProfile.QueryPatrollingProfiles(requestContext, new string[1] { id })?.FirstOrDefault();
	}
}
