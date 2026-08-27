using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public record PatchScimUserResponse(ScimUser ScimUser) : IScimUserResponse;
