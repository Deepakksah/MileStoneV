using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VideoOS.IdentityServer.Endpoints;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Features.SCIM.Shared.Filters;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public class PatchScimUserEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPatch("scim/v2/Users/{id}", (Func<string, PatchScimUserRequest, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)(async (string id, [FromBody] PatchScimUserRequest request, IMediator mediator) =>
		{
			PatchScimUserCommand request2 = new PatchScimUserCommand(request.Schemas, id, request.Operations);
			Result<ExpandoObject> result = await mediator.Send((IRequest<Result<ExpandoObject>>)request2, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		})).RequireAuthorization("ScimPolicy").AddEndpointFilter<ValidateHeadersFilter>()
			.AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
