using System.Dynamic;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public record GetScimUserListQuery(string[] Attributes, string Filter, int? StartIndex, int? Count) : IRequest<Result<ExpandoObject>>, IBaseRequest;
