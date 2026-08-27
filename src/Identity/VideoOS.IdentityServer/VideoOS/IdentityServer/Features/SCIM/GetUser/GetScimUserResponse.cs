using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public record GetScimUserResponse(ScimUser ScimUser) : IScimUserResponse;
