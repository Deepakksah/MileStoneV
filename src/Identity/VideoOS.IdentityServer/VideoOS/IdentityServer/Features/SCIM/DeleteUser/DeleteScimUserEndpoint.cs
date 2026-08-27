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

namespace VideoOS.IdentityServer.Features.SCIM.DeleteUser;

public class DeleteScimUserEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapDelete("scim/v2/Users/{id}", (Func<string, IMediator, Task<Microsoft.AspNetCore.Http.IResult>>)async delegate(string id, IMediator mediator)
		{
			DeleteScimUserCommand request = new DeleteScimUserCommand(id);
			Result result = await mediator.Send((IRequest<Result>)request, default(CancellationToken));
			return result.IsError ? ScimErrorMapper.ToScimErrorResult(result.Errors) : Results.NoContent();
		}).RequireAuthorization("ScimPolicy").AddEndpointFilter<ClientValidationFilter>()
			.AddEndpointFilter<ResponseFilter>();
	}
}
