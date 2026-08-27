using System.Dynamic;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public record PutScimUserCommand(string[] Schemas, string Id, bool Active) : IRequest<Result<ExpandoObject>>, IBaseRequest;
