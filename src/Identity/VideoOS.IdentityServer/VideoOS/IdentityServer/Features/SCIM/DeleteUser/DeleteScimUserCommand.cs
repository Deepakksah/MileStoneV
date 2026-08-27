using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.DeleteUser;

public record DeleteScimUserCommand(string Id) : IRequest<Result>, IBaseRequest;
