using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class ExternalTokenValidator : IExternalTokenValidator
{
	private readonly IKeyConverter _keyConverter;

	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

	public ExternalTokenValidator(IKeyConverter keyConverter)
	{
		_keyConverter = keyConverter;
	}

	public ClaimsPrincipal ValidateTokenExternal(string subjectToken, DiscoveryDocumentResponse discoResponse)
	{
		TokenValidationParameters validationParameters = CreateValidationParameters(discoResponse);
		SecurityToken validatedToken;
		return _jwtSecurityTokenHandler.ValidateToken(subjectToken, validationParameters, out validatedToken);
	}

	public Result<SubjectTokenClaims> GetSubjectTokenClaims(string subjectToken)
	{
		if (string.IsNullOrWhiteSpace(subjectToken))
		{
			return Error.Failure("Subject token cannot be null or empty");
		}
		JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(subjectToken);
		string text = jwtSecurityToken.Claims.FirstOrDefault((Claim claim) => claim.Type == "iss")?.Value;
		if (text == null)
		{
			return Error.Failure("Subject token missing issuer claim");
		}
		string text2 = jwtSecurityToken.Claims.FirstOrDefault((Claim claim) => claim.Type == "idp")?.Value;
		if (text2 == null)
		{
			return Error.Failure("Subject token missing idp claim");
		}
		return new SubjectTokenClaims
		{
			Issuer = text,
			Idp = text2
		};
	}

	private TokenValidationParameters CreateValidationParameters(DiscoveryDocumentResponse discoResponse)
	{
		return new TokenValidationParameters
		{
			IssuerSigningKeys = _keyConverter.ConvertJsonWebKeyToSecurityKey(discoResponse.KeySet?.Keys),
			ValidIssuer = discoResponse.Issuer,
			ValidAudience = "managementserver"
		};
	}
}
