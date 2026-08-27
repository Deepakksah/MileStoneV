using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpManagementPolicy")]
[Route("api/[controller]")]
[ApiController]
public class TrustedIssuersController : BaseController
{
	private readonly IMapper _mapper;

	private readonly ITrustedIssuerRepository _repository;

	public TrustedIssuersController(ITrustedIssuerRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet("{id}", Name = "GetTrustedIssuer")]
	public async Task<ActionResult<AdminIdentityDto>> Get(int id)
	{
		TrustedIssuer trustedIssuer = await _repository.GetByIdAsync(id);
		if (trustedIssuer == null)
		{
			return NotFound(new ApiCodes().TrustedIssuerNotFound(id));
		}
		return Ok(_mapper.Map<TrustedIssuerDto>(trustedIssuer));
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<TrustedIssuerDto>>> Get()
	{
		IEnumerable<TrustedIssuer> source = await _repository.GetAsync();
		IEnumerable<TrustedIssuerDto> value = _mapper.Map<IEnumerable<TrustedIssuerDto>>(source);
		return Ok(value);
	}

	[HttpPost]
	public async Task<ActionResult<TrustedIssuerDto>> Add(TrustedIssuerForCreationModel model)
	{
		TrustedIssuer trustedIssuerEntity = _mapper.Map<TrustedIssuer>(model);
		if (await _repository.IssuerExistsAsync(trustedIssuerEntity.Issuer))
		{
			return BadRequest(new ApiCodes().DuplicateIssuer(trustedIssuerEntity.Issuer));
		}
		if (await _repository.AddressExistsAsync(trustedIssuerEntity.Address))
		{
			return BadRequest(new ApiCodes().DuplicateAddress(trustedIssuerEntity.Address.ToString()));
		}
		_repository.Add(trustedIssuerEntity);
		await _repository.SaveChangesAsync();
		TrustedIssuerDto trustedIssuerDto = _mapper.Map<TrustedIssuerDto>(trustedIssuerEntity);
		return CreatedAtRoute("GetTrustedIssuer", new { trustedIssuerDto.Id }, trustedIssuerDto);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(int id)
	{
		TrustedIssuer trustedIssuer = await _repository.GetByIdAsync(id);
		if (trustedIssuer == null)
		{
			return NotFound(new ApiCodes().TrustedIssuerNotFound(id));
		}
		_repository.Delete(trustedIssuer);
		await _repository.SaveChangesAsync();
		return NoContent();
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Put(int id, TrustedIssuerForUpdateModel model)
	{
		TrustedIssuer trustedIssuerEntity = _mapper.Map<TrustedIssuer>(model);
		TrustedIssuer currentIssuer = await _repository.GetByIdAsync(id);
		if (currentIssuer == null)
		{
			return NotFound(new ApiCodes().TrustedIssuerNotFound(id));
		}
		bool flag = !currentIssuer.Issuer.Equals(trustedIssuerEntity.Issuer);
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = await _repository.IssuerExistsAsync(trustedIssuerEntity.Issuer);
		}
		if (flag2)
		{
			return BadRequest(new ApiCodes().DuplicateIssuer(trustedIssuerEntity.Issuer));
		}
		bool flag3 = !currentIssuer.Address.Equals(trustedIssuerEntity.Address);
		bool flag4 = flag3;
		if (flag4)
		{
			flag4 = await _repository.AddressExistsAsync(trustedIssuerEntity.Address);
		}
		if (flag4)
		{
			return BadRequest(new ApiCodes().DuplicateAddress(trustedIssuerEntity.Address.ToString()));
		}
		currentIssuer.Issuer = trustedIssuerEntity.Issuer;
		currentIssuer.Address = trustedIssuerEntity.Address;
		await _repository.SaveChangesAsync();
		return Ok(_mapper.Map<TrustedIssuerDto>(currentIssuer));
	}
}
