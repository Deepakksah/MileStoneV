using System;
using System.Dynamic;
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

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public class CreateScimUserEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("scim/v2/Users", (Func<CreateScimUserRequest, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)async delegate(CreateScimUserRequest request, IMediator mediator)
		{
			CreateScimUserCommand request2 = new CreateScimUserCommand(request.Schemas, request.UserName, request.ExternalId, request.Active);
			Result<ExpandoObject> result = await mediator.Send((IRequest<Result<ExpandoObject>>)request2, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.Created(ExpandoObjectHelper.GetMetaLocation(result.Value), result.Value);
		}).RequireAuthorization("ScimPolicy").AddEndpointFilter<ValidateHeadersFilter>()
			.AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
