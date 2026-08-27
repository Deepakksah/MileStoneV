using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdentityServerAccessToken")]
[Route("api/[controller]")]
[ApiController]
public class ClientsController : BaseController
{
	private readonly IConfigurationRepository _clientRepository;

	private readonly IMapper _mapper;

	public ClientsController(IConfigurationRepository clientRepository, IMapper mapper)
	{
		_clientRepository = clientRepository;
		_mapper = mapper;
	}

	[HttpGet("{clientId}", Name = "GetClient")]
	[ProducesErrorResponseType(typeof(ErrorResponse))]
	public async Task<ActionResult<ClientDto>> GetClient(string clientId)
	{
		Client client = await _clientRepository.GetByClientIdAsync(clientId, (Client c) => c.ClientSecrets, (Client c) => c.AllowedGrantTypes, (Client c) => c.AllowedScopes, (Client c) => c.RedirectUris).IgnoreContext();
		if (client == null)
		{
			return NotFound(new ApiCodes().ClientNotFound(clientId));
		}
		return Ok(_mapper.Map<ClientDto>(client));
	}

	[HttpGet(Name = "GetClients")]
	[ProducesErrorResponseType(typeof(ErrorResponse))]
	public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
	{
		IEnumerable<Client> source = await _clientRepository.GetClientsAsync((Client c) => c.ClientSecrets, (Client c) => c.AllowedGrantTypes, (Client c) => c.AllowedScopes, (Client c) => c.RedirectUris).IgnoreContext();
		return Ok(_mapper.Map<IEnumerable<ClientDto>>(source));
	}
}
