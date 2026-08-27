using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public record RegisterClientCommand(string ClientId, string ClientName, string[] ClientScopes, string[] ClientGrantTypes, string[]? ClientRedirectUris, string[]? PostLogoutRedirectUris, bool? RequireSecretForAuthCodeFlow) : IRequest<Result<RegisterClientOperationResponse>>, IBaseRequest;
