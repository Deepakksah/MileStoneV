using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services.Interfaces;
using VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

namespace VideoOS.IdentityServer.GrantValidators;

public class TokenExchangeGrantValidator : IExtensionGrantValidator
{
	private readonly IExternalTokenValidator _externalTokenValidationService;

	private readonly ILogger<TokenExchangeGrantValidator> _logger;

	private readonly IWellKnownConfigurationRetriever _wellKnownConfigurationRetriever;

	private readonly ILoginProviderRepository _providerRepository;

	private readonly IUserProvisioner _userProvisioner;

	private readonly IUserInfoProvider _userInfoProvider;

	private readonly HttpContext _httpContext;

	private readonly ITrustedIssuerRepository _trustedIssuerRepository;

	public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";

	public TokenExchangeGrantValidator(IExternalTokenValidator externalTokenValidationService, ILogger<TokenExchangeGrantValidator> logger, IWellKnownConfigurationRetriever wellKnownConfigurationRetriever, ILoginProviderRepository providerRepository, IUserProvisioner autoProvisionUser, IUserInfoProvider userInfoProvider, IHttpContextAccessor httpContextAccessor, ITrustedIssuerRepository trustedIssuerRepository)
	{
		_externalTokenValidationService = externalTokenValidationService;
		_logger = logger;
		_wellKnownConfigurationRetriever = wellKnownConfigurationRetriever;
		_providerRepository = providerRepository;
		_userProvisioner = autoProvisionUser;
		_userInfoProvider = userInfoProvider;
		_httpContext = httpContextAccessor.HttpContext;
		_trustedIssuerRepository = trustedIssuerRepository;
	}

	public async Task ValidateAsync(ExtensionGrantValidationContext context)
	{
		if (_httpContext == null)
		{
			InvalidRequest(context, "Error no active HttpContext");
			return;
		}
		string text = context.Request.Raw.Get("exchange_style");
		if (text == null)
		{
			InvalidRequest(context, "Parameter 'exchange_style' missing in request");
			return;
		}
		if (text != "vms_federation")
		{
			InvalidRequest(context, "Not supported value for parameter 'exchange_style'");
			return;
		}
		string subjectToken = context.Request.Raw.Get("subject_token");
		string a = context.Request.Raw.Get("subject_token_type");
		if (!string.Equals(a, "urn:ietf:params:oauth:token-type:access_token"))
		{
			InvalidRequest(context, "Subject token should be of type access_token");
			return;
		}
		Result<SubjectTokenClaims> subjectTokenClaims = _externalTokenValidationService.GetSubjectTokenClaims(subjectToken);
		if (subjectTokenClaims.IsError)
		{
			InvalidRequest(context);
			return;
		}
		string issuer = subjectTokenClaims.Value.Issuer;
		TrustedIssuer trustedIssuer = await _trustedIssuerRepository.GetByIssuerAsync(issuer);
		if (trustedIssuer == null || trustedIssuer.Issuer == null || !trustedIssuer.Issuer.Equals(issuer, StringComparison.Ordinal))
		{
			InvalidRequest(context, "Error validating issuer. " + issuer + " is not in list of trusted servers");
			return;
		}
		if (_httpContext.Request.IsHttps && trustedIssuer.Address.Scheme == Uri.UriSchemeHttp)
		{
			InvalidRequest(context, $"Token exchange with non-secure server '{trustedIssuer.Address}' is not allowed when using HTTPS.");
			return;
		}
		string address = trustedIssuer.Address.ToString();
		Result<WellKnownConfigurationResponse> discoResponse = await _wellKnownConfigurationRetriever.GetWellKnownConfiguration(address, _httpContext.Request.IsHttps);
		if (discoResponse.IsError)
		{
			InvalidRequest(context, "Error getting discovery document. " + discoResponse.Errors.First().Description);
			return;
		}
		try
		{
			_externalTokenValidationService.ValidateTokenExternal(subjectToken, discoResponse.Value.DiscoveryDocumentResponse);
		}
		catch (Exception value)
		{
			InvalidRequest(context, $"Error validating token. {value}");
			return;
		}
		VideoOS.IdentityServer.Entities.LoginProvider provider = await FindMatchingProvider(subjectTokenClaims.Value.Idp, discoResponse.Value.LoginProviders);
		if (provider == null)
		{
			InvalidRequest(context, "Error matching external provider information with main IDP");
			return;
		}
		UserInfoResponse userInfoResponse = await _userInfoProvider.GetUserInformation(subjectToken, discoResponse.Value.DiscoveryDocumentResponse);
		if (!userInfoResponse.HttpResponse.IsSuccessStatusCode)
		{
			InvalidRequest(context, "Error getting claims for user");
			return;
		}
		string text2 = userInfoResponse.Claims.FirstOrDefault((Claim x) => x.Type == "vms_provider_key")?.Value;
		if (text2 == null)
		{
			InvalidRequest(context, "Missing vms_provider_key claim from userInfo");
			return;
		}
		UserLoginInfo info = new UserLoginInfo(provider.Id.ToString(), text2, provider.DisplayName);
		VmsUser vmsUser = await _userProvisioner.CreateOrUpdateUser(info, userInfoResponse.Claims.ToList());
		Dictionary<string, object> customResponse = new Dictionary<string, object> { { "issued_token_type", "urn:ietf:params:oauth:token-type:access_token" } };
		context.Result = new GrantValidationResult(vmsUser.Id, GrantType, null, provider.Id.ToString(), customResponse);
	}

	private void InvalidRequest(ExtensionGrantValidationContext context, string errorMessage = null)
	{
		if (!string.IsNullOrEmpty(errorMessage))
		{
			_logger.Log(LogLevel.Error, errorMessage);
		}
		context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
	}

	private async Task<VideoOS.IdentityServer.Entities.LoginProvider> FindMatchingProvider(string subjectTokenIdpClaim, IEnumerable<VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever.LoginProvider> loginProviders)
	{
		VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever.LoginProvider mainLoginProvider = loginProviders.FirstOrDefault((VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever.LoginProvider p) => p.Id == Guid.Parse(subjectTokenIdpClaim));
		VideoOS.IdentityServer.Entities.LoginProvider result = null;
		if (mainLoginProvider != null)
		{
			_logger.LogInformation("External provider top site - Name: '" + mainLoginProvider.Name + "'");
			foreach (VideoOS.IdentityServer.Entities.LoginProvider item in await _providerRepository.GetLoginProviders())
			{
				_logger.LogInformation($"Matching local external provider - Name: {item.DisplayName}, Enabled: {item.Enabled}");
				if (!(item.DisplayName != mainLoginProvider.Name) && item.Enabled)
				{
					result = item;
					break;
				}
			}
		}
		else
		{
			_logger.LogError("External provider with id '" + subjectTokenIdpClaim + "' not found on top site.");
		}
		return result;
	}
}
