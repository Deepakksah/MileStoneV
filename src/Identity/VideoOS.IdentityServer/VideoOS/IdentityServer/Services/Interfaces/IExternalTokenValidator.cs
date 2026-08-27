using System.Security.Claims;
using IdentityModel.Client;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface IExternalTokenValidator
{
	ClaimsPrincipal ValidateTokenExternal(string subjectToken, DiscoveryDocumentResponse discoResponse);

	Result<SubjectTokenClaims> GetSubjectTokenClaims(string subjectToken);
}
