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
public class AdminIdentitiesController : BaseController
{
	private readonly IAdminIdentityRepository _repository;

	private readonly IMapper _mapper;

	public AdminIdentitiesController(IAdminIdentityRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet("{identity}", Name = "GetAdminIdentity")]
	public async Task<ActionResult<AdminIdentityDto>> Get(string identity)
	{
		AdminIdentity adminIdentity = await _repository.GetAdminIdentityAsync(identity);
		if (adminIdentity == null)
		{
			return NotFound(new ApiCodes().AdminIdentityNotFound(identity));
		}
		return Ok(_mapper.Map<AdminIdentityDto>(adminIdentity));
	}

	[HttpPost]
	public async Task<ActionResult<AdminIdentityDto>> Post(AdminIdentityForCreationModel model)
	{
		AdminIdentity adminIdentityEntity = _mapper.Map<AdminIdentity>(model);
		AdminIdentity adminIdentity = await _repository.GetAdminIdentityAsync(adminIdentityEntity.Identity);
		if (adminIdentity != null)
		{
			return BadRequest(new ApiCodes().DuplicateAdminIdentity(adminIdentity.Identity));
		}
		_repository.AddAdminIdentity(adminIdentityEntity);
		await _repository.SaveChangesAsync();
		AdminIdentityDto adminIdentityDto = _mapper.Map<AdminIdentityDto>(adminIdentityEntity);
		return CreatedAtRoute("GetAdminIdentity", new { adminIdentityDto.Identity }, adminIdentityDto);
	}

	[HttpDelete("{identity}")]
	public async Task<ActionResult> Delete(string identity)
	{
		AdminIdentity adminIdentity = await _repository.GetAdminIdentityAsync(identity);
		if (adminIdentity == null)
		{
			return NotFound(new ApiCodes().AdminIdentityNotFound(identity));
		}
		_repository.DeleteAdminIdentity(adminIdentity);
		await _repository.SaveChangesAsync();
		return NoContent();
	}
}
