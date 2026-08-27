namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IPatrollingProfileProxy
{
	PatrollingProfile QueryPatrollingProfile(IGeminiRequestContext requestContext, string patrollingProfileId);
}
