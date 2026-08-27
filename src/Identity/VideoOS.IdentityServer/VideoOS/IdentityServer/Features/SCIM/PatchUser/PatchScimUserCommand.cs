using System.Dynamic;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public record PatchScimUserCommand(string[] Schemas, string Id, ScimOperation[] Operations) : IRequest<Result<ExpandoObject>>, IBaseRequest;
