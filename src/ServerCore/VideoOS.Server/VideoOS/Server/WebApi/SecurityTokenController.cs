using System;
using System.Web.Http;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/tokens")]
public sealed class SecurityTokenController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public SecurityTokenController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("replacement")]
	public IHttpActionResult Replacement(Guid serviceId, [FromBody] ReplacementTokenModel replacementToken)
	{
		if (replacementToken == null)
		{
			return BadRequest();
		}
		SecurityTokenWithServerTimeDto2 securityTokenWithServerTimeDto = null;
		try
		{
			securityTokenWithServerTimeDto = _communicationManager.GetSecurityToken(serviceId, replacementToken.ReplacementToken);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok(new SecurityTokenModel
		{
			Token = securityTokenWithServerTimeDto
		});
	}
}
