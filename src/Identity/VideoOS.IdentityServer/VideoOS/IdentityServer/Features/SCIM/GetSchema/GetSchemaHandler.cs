using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public class GetSchemaHandler(GetUserSchemaService schemaService) : IRequestHandler<GetSingleSchemaQuery, Result<SchemaResponse>>
{
	public Task<Result<SchemaResponse>> Handle(GetSingleSchemaQuery query, CancellationToken cancellationToken)
	{
		if (query.SchemaId != "urn:ietf:params:scim:schemas:core:2.0:User")
		{
			return Task.FromResult(Result<SchemaResponse>.Failure(Error.NotFound("NotFound", "Schema '" + query.SchemaId + "' not found")));
		}
		SchemaResponse value = schemaService.GenerateScimUserSchema();
		return Task.FromResult(Result<SchemaResponse>.Success(value));
	}
}
