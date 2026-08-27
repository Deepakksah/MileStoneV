using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NSwag", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public interface IClient
{
	Task<GetMapResponse> Maps_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetMapsResponse> Maps_GetAsync(string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetVisualItemResponse> Maps_GetVisualItemOnMapByIdAsync(Guid mapId, Guid itemId, string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetVisualItemsResponse> Maps_GetVisualItemsOnMapAsync(Guid mapId, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));
}
