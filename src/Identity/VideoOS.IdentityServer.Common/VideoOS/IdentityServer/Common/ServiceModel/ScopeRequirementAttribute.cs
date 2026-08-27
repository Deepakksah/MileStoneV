using System;

namespace VideoOS.IdentityServer.Common.ServiceModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ScopeRequirementAttribute : ClaimRequirementAttribute
{
	public ScopeRequirementAttribute(string scope)
		: base("scope", scope)
	{
	}
}
