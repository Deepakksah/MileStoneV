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

public class GetSchemaEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scim/v2/Schemas/{schemaId}", (Func<string, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)async delegate(string schemaId, IMediator mediator)
		{
			GetSingleSchemaQuery request = new GetSingleSchemaQuery(schemaId);
			Result<SchemaResponse> result = await mediator.Send((IRequest<Result<SchemaResponse>>)request, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Ok(result.Value);
		}).RequireAuthorization("ScimPolicy").AddEndpointFilter<ResponseFilter>();
	}
}
