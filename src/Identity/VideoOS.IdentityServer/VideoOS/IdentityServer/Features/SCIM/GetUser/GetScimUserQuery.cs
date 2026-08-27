using System.Dynamic;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public record GetScimUserQuery(string Id, string[] Attributes) : IRequest<Result<ExpandoObject>>, IBaseRequest;
