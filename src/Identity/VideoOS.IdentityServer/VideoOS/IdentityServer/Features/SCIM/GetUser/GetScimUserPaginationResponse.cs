using System.Collections.Generic;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public record GetScimUserPaginationResponse(int TotalResults, List<ScimUser> Resources, int ItemsPerPage, int StartIndex) : GetScimUserListResponse(TotalResults, Resources);
