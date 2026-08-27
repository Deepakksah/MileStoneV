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

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public class GetScimUserEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scim/v2/Users/{id}", (Func<string, string, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)(async (string id, [FromQuery] string attributes, IMediator mediator) =>
		{
			GetScimUserQuery request = new GetScimUserQuery(id, attributes?.Split(','));
			Result<ExpandoObject> result = await mediator.Send((IRequest<Result<ExpandoObject>>)request, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		})).RequireAuthorization("ScimPolicy").AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
