using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public record CreateScimUserResponse(ScimUser ScimUser) : IScimUserResponse;
