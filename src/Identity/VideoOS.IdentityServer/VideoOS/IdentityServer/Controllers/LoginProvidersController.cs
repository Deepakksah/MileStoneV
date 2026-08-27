using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpManagementPolicy")]
[Route("api/[controller]")]
[ApiController]
public class LoginProvidersController : BaseController
{
	private readonly ILoginProviderRepository _repository;

	private readonly IMapper _mapper;

	private readonly ILoginProviderRestrictions _restrictions;

	private readonly IExternalProviderManager _externalProvidersManager;

	private readonly IConfigurationRepository _clientRepository;

	private static SemaphoreSlim createAndupdateLock = new SemaphoreSlim(1, 1);

	public LoginProvidersController(ILoginProviderRepository repository, ILoginProviderRestrictions restrictions, IMapper mapper, IExternalProviderManager externalProvidersManager, IConfigurationRepository clientRepository)
	{
		_repository = repository;
		_restrictions = restrictions;
		_mapper = mapper;
		_externalProvidersManager = externalProvidersManager;
		_clientRepository = clientRepository;
	}

	[HttpGet("{providerId}", Name = "GetLoginProvider")]
	public async Task<ActionResult<LoginProviderDto>> GetLoginProvider(Guid providerId)
	{
		LoginProvider loginProvider = await _repository.GetLoginProvider(providerId);
		if (loginProvider == null)
		{
			return NotFound(new ApiCodes().LoginProviderNotFound(providerId));
		}
		return Ok(_mapper.Map<LoginProviderDto>(loginProvider));
	}

	[HttpGet(Name = "GetLoginProviders")]
	public async Task<ActionResult<IEnumerable<LoginProviderDto>>> GetLoginProviders()
	{
		IEnumerable<LoginProvider> source = await _repository.GetLoginProviders();
		IEnumerable<LoginProviderDto> value = _mapper.Map<IEnumerable<LoginProviderDto>>(source);
		return Ok(value);
	}

	[HttpPost(Name = "CreateLoginProvider")]
	public async Task<ActionResult<LoginProviderDto>> CreateLoginProvider(LoginProviderForCreationModel loginProviderModel)
	{
		await createAndupdateLock.WaitAsync();
		LoginProvider loginProviderEntity = _mapper.Map<LoginProvider>(loginProviderModel);
		try
		{
			if (await _repository.HasAny(loginProviderModel.Id))
			{
				return BadRequest(new ApiCodes().DuplicateLoginProvider(loginProviderModel.Id));
			}
			if (await _repository.HasAnyWithDisplayName(loginProviderModel.DisplayName))
			{
				return BadRequest(new ApiCodes().DuplicateDisplayName(loginProviderModel.DisplayName));
			}
			if (await _repository.HasAnyWithAuthority(loginProviderModel.Authority))
			{
				return BadRequest(new ApiCodes().DuplicateAuthority(loginProviderModel.Authority));
			}
			IEnumerable<string> scimClientIds = loginProviderModel.ScimClientIds;
			foreach (string clientId in scimClientIds ?? Enumerable.Empty<string>())
			{
				if (await _clientRepository.GetByClientIdAsync(clientId) == null)
				{
					return BadRequest(new ApiCodes().ClientNotFound(clientId));
				}
			}
			LoginProvider loginProvider = (await _repository.GetClientsMappedProviders(loginProviderModel.ScimClientIds))?.FirstOrDefault();
			if (loginProvider != null)
			{
				string clientId2 = (from f in loginProvider.ScimClients
					select f.ClientId into f
					where Enumerable.Contains(loginProviderModel.ScimClientIds, f)
					select f).FirstOrDefault();
				return Conflict(new ApiCodes().DuplicateClientProviderMapping(clientId2, loginProvider.Id));
			}
			bool allowOnlyOneProvider = _restrictions.AllowOnlyOneProvider;
			bool flag = allowOnlyOneProvider;
			if (flag)
			{
				flag = await _repository.HasAny();
			}
			if (flag)
			{
				return BadRequest(new ApiCodes().ThereCanBeOnlyOne());
			}
			_repository.CreateLoginProvider(loginProviderEntity);
			await _repository.SaveChangesAsync();
		}
		finally
		{
			createAndupdateLock.Release();
		}
		try
		{
			_externalProvidersManager.AddProvider(loginProviderEntity);
		}
		catch (ScopesFormatInvalidException)
		{
			return BadRequest(new ApiCodes().InvalidScopes());
		}
		LoginProviderDto loginProviderDto = _mapper.Map<LoginProviderDto>(loginProviderEntity);
		return CreatedAtRoute("GetLoginProvider", new
		{
			providerId = loginProviderDto.Id
		}, loginProviderDto);
	}

	[HttpPatch("{providerId}", Name = "UpdateLoginProvider")]
	public async Task<ActionResult> UpdateLoginProvider(Guid providerId, JsonPatchDocument<LoginProviderForUpdateModel> loginProviderPatchDocument)
	{
		await createAndupdateLock.WaitAsync();
		LoginProvider loginProviderEntity;
		try
		{
			loginProviderEntity = await _repository.GetLoginProvider(providerId);
			if (loginProviderEntity == null)
			{
				return NotFound(new ApiCodes().LoginProviderNotFound(providerId));
			}
			var before = new { loginProviderEntity.DisplayName, loginProviderEntity.Authority };
			LoginProviderForUpdateModel loginProviderForUpdateModel = _mapper.Map<LoginProviderForUpdateModel>(loginProviderEntity);
			loginProviderPatchDocument.ApplyTo(loginProviderForUpdateModel, base.ModelState);
			if (!base.ModelState.IsValid)
			{
				return BadRequest(base.ModelState);
			}
			if (!TryValidateModel(loginProviderForUpdateModel))
			{
				return BadRequest(base.ModelState);
			}
			loginProviderEntity = _mapper.Map(loginProviderForUpdateModel, loginProviderEntity, delegate(IMappingOperationOptions<LoginProviderForUpdateModel, LoginProvider> opts)
			{
				opts.Items["ProviderId"] = providerId;
			});
			bool flag = !string.Equals(loginProviderEntity.DisplayName, before.DisplayName, StringComparison.OrdinalIgnoreCase);
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await _repository.HasAnyWithDisplayName(loginProviderEntity.DisplayName);
			}
			if (flag2)
			{
				return BadRequest(new ApiCodes().DuplicateDisplayName(loginProviderEntity.DisplayName));
			}
			bool flag3 = !string.Equals(loginProviderEntity.Authority, before.Authority, StringComparison.OrdinalIgnoreCase);
			bool flag4 = flag3;
			if (flag4)
			{
				flag4 = await _repository.HasAnyWithAuthority(loginProviderEntity.Authority);
			}
			if (flag4)
			{
				return BadRequest(new ApiCodes().DuplicateAuthority(loginProviderEntity.Authority));
			}
			foreach (string clientId in loginProviderEntity.ScimClients?.Select((LoginProviderClient f) => f.ClientId) ?? Enumerable.Empty<string>())
			{
				if (await _clientRepository.GetByClientIdAsync(clientId, (Client f) => f.AllowedScopes) == null)
				{
					return BadRequest(new ApiCodes().ClientNotFound(clientId));
				}
			}
			LoginProvider loginProvider = (await _repository.GetClientsMappedProviders(loginProviderEntity.ScimClients?.Select((LoginProviderClient f) => f.ClientId).ToArray())).Where((LoginProvider f) => f.Id != loginProviderEntity.Id)?.FirstOrDefault();
			if (loginProvider != null)
			{
				string clientId2 = (from f in loginProvider.ScimClients
					select f.ClientId into f
					where loginProviderEntity.ScimClients.Select((LoginProviderClient loginProviderClient) => loginProviderClient.ClientId).Contains(f)
					select f).FirstOrDefault();
				return Conflict(new ApiCodes().DuplicateClientProviderMapping(clientId2, loginProvider.Id));
			}
			try
			{
				_repository.UpdateLoginProvider(loginProviderEntity);
			}
			catch (ScopesFormatInvalidException)
			{
				return BadRequest(new ApiCodes().InvalidScopes());
			}
			await _repository.SaveChangesAsync();
		}
		finally
		{
			createAndupdateLock.Release();
		}
		_externalProvidersManager.UpdateProvider(loginProviderEntity);
		return Ok(_mapper.Map<LoginProviderDto>(loginProviderEntity));
	}

	[HttpDelete("{providerId}", Name = "DeleteLoginProvider")]
	public async Task<ActionResult> DeleteLoginProvider(Guid providerId)
	{
		LoginProvider loginProviderEntity = await _repository.GetLoginProvider(providerId);
		if (loginProviderEntity == null)
		{
			return NotFound(new ApiCodes().LoginProviderNotFound(providerId));
		}
		_repository.DeleteLoginProvider(loginProviderEntity);
		await _repository.SaveChangesAsync();
		_externalProvidersManager.RemoveProvider(loginProviderEntity.Id.ToString(), loginProviderEntity.DisplayName);
		return NoContent();
	}
}
