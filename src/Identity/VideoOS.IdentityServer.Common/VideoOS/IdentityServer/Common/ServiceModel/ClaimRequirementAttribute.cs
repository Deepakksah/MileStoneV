using System;
using System.Security.Claims;

namespace VideoOS.IdentityServer.Common.ServiceModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ClaimRequirementAttribute : Attribute
{
	public Claim RequiredClaim { get; }

	public ClaimRequirementAttribute(string type, string value)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		RequiredClaim = new Claim(type, value);
	}
}
