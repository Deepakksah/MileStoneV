using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public record RegisterClientOperationResponse(RegisterClientResponse Response, OperationType Operation);
