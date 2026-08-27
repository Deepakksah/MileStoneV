using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public class GetSingleSchemaQuery(string schemaId) : IRequest<Result<SchemaResponse>>, IBaseRequest
{
	public string SchemaId { get; } = schemaId;
}
