using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NSwag", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public interface IClient
{
	Task<GetEventResponse> Events_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetEventsResponse> Events_GetAsync(string orderBy = null, string time = null, string source_name = null, string message = null, string source_id = null, string type = null, string type_name = null, string typeGroup = null, string tag = null, string analyticData_object_data = null, string analyticData_object_value = null, string analyticData_rule_name = null, string analyticData_object_vendor = null, string localId = null, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostEventResponse> Events_PostAsync(PostEventBody body, bool? triggerRules = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostBulkEventsResponse> Events_PostBulkAsync(IEnumerable<PostEventBody> body, bool? triggerRules = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetEventsSessionResponse> EventSessions_GetByIdAsync(Guid id, int? maxSize = null, string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostEventSessionResponse> EventSessions_PostAsync(PostEventSessionBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteEventSessionResponse> EventSessions_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
