using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public record GetSchemaListQuery : IRequest<Result<SchemaListResponse>>, IBaseRequest;
