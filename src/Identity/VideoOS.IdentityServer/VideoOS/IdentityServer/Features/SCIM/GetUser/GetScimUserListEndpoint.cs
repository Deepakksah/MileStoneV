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

public class GetScimUserListEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scim/v2/Users", (Func<IMediator, int?, int?, string, string, Task<Microsoft.AspNetCore.Http.IResult>>)(async (IMediator mediator, [FromQuery] int? startIndex, [FromQuery] int? count, [FromQuery] string attributes, [FromQuery] string filter) =>
		{
			GetScimUserListQuery request = new GetScimUserListQuery(attributes?.Split(','), filter, startIndex, count);
			Result<ExpandoObject> result = await mediator.Send((IRequest<Result<ExpandoObject>>)request, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		})).RequireAuthorization("ScimPolicy").AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
