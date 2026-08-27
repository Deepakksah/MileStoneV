using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VideoOS.IdentityServer.Endpoints;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Features.SCIM.Shared.Filters;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public class GetSchemaListEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder routes)
	{
		routes.MapGet("scim/v2/Schemas", (Func<IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)async delegate(IMediator mediator)
		{
			GetSchemaListQuery request = new GetSchemaListQuery();
			Result<SchemaListResponse> result = await mediator.Send((IRequest<Result<SchemaListResponse>>)request, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		}).RequireAuthorization("ScimPolicy").AddEndpointFilter<ResponseFilter>();
	}
}
