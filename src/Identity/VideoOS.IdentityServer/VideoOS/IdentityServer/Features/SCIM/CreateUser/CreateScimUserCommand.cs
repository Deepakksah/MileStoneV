using System.Dynamic;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public record CreateScimUserCommand(string[] Schemas, string UserName, string ExternalId, bool Active) : IRequest<Result<ExpandoObject>>, IBaseRequest;
