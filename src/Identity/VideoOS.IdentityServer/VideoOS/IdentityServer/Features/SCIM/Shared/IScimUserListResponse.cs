using System.Collections.Generic;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public interface IScimUserListResponse
{
	List<ScimUser> Resources { get; init; }
}
