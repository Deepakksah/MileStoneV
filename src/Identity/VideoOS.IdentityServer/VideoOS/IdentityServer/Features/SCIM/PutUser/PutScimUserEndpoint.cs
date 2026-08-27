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

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public class PutScimUserEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("scim/v2/Users/{id}", (Func<string, PutScimUserRequest, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)(async (string id, [FromBody] PutScimUserRequest request, IMediator mediator) =>
		{
			PutScimUserCommand request2 = new PutScimUserCommand(request.Schemas, id, request.Active);
			Result<ExpandoObject> result = await mediator.Send((IRequest<Result<ExpandoObject>>)request2, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		})).RequireAuthorization("ScimPolicy").AddEndpointFilter<ValidateHeadersFilter>()
			.AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
