using System.Collections.Generic;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public record GetScimUserListResponse(int TotalResults, List<ScimUser> Resources) : IScimUserListResponse
{
	public string[] Schemas { get; } = new string[1] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" };
}
