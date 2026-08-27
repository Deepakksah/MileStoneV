using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public record PutScimUserResponse(ScimUser ScimUser) : IScimUserResponse;
