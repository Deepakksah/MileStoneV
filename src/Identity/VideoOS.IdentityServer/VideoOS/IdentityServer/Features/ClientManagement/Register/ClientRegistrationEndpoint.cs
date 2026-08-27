using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VideoOS.IdentityServer.Endpoints;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public sealed class ClientRegistrationEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("api/Clients/{clientId}", (Func<HttpContext, string, ClientRegistrationRequest, LinkGenerator, IMediator, CancellationToken, Task<Microsoft.AspNetCore.Http.IResult>>)(async (HttpContext context, string clientId, [FromBody] ClientRegistrationRequest request, LinkGenerator linkGenerator, IMediator mediator, CancellationToken cancellationToken) =>
		{
			RegisterClientCommand request2 = new RegisterClientCommand(clientId, request.ClientName, request.ClientScopes, request.ClientGrantTypes, request.ClientRedirectUris, request.PostLogoutRedirectUris, request.RequireSecretForAuthCodeFlow);
			Result<RegisterClientOperationResponse> result = await mediator.Send((IRequest<Result<RegisterClientOperationResponse>>)request2, cancellationToken);
			if (result.IsError)
			{
				return result.ToLegacyErrorResponse();
			}
			BaseResponse value = new BaseResponse(result.Value.Response);
			return result.Value.Operation switch
			{
				OperationType.Created => Results.Created(linkGenerator.GetUriByName(context, "ClientPut", new { clientId }), value), 
				OperationType.Updated => Results.Ok(value), 
				_ => throw new InvalidOperationException($"Unhandled operation type: {result.Value.Operation}"), 
			};
		})).WithName("ClientPut").RequireAuthorization("IdentityServerAccessToken");
	}
}
