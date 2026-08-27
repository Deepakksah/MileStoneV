using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public class GetSchemaListHandler(GetUserSchemaService schemaService) : IRequestHandler<GetSchemaListQuery, Result<SchemaListResponse>>
{
	public Task<Result<SchemaListResponse>> Handle(GetSchemaListQuery query, CancellationToken cancellationToken)
	{
		SchemaResponse item = schemaService.GenerateScimUserSchema();
		List<SchemaResponse> list = new List<SchemaResponse> { item };
		SchemaListResponse value = new SchemaListResponse(list.Count, 1, list.Count, list.ToArray());
		return Task.FromResult(Result<SchemaListResponse>.Success(value));
	}
}
