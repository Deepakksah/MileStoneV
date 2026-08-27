using System;
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NSwag", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public interface IClient
{
	Task<GetAlarmAuthorizedRolesResponse> AlarmAuthorizedRoles_GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmCategoriesResponse> AlarmCategories_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmCategoryResponse> AlarmCategories_PostAsync(PostAlarmCategoryDto body, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmCategoryResponse> AlarmCategories_PatchByIdAsync(Guid id, PatchAlarmCategoryBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmCategoryResponse> AlarmCategories_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmCloseReasonsResponse> AlarmCloseReasons_GetAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<PutAlarmCloseReasonsResponse> AlarmCloseReasons_PutAsync(AlarmCloseReasons body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmDisablesResponse> AlarmDisables_GetAsync(string source_id = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostPutAlarmDisableResponse> AlarmDisables_PostAsync(SimpleAlarmDisable body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmDisableResponse> AlarmDisables_Get2Async(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostPutAlarmDisableResponse> AlarmDisables_PutByIdAsync(Guid id, SimpleAlarmDisable body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmDisableResponse> AlarmDisables_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmManagerColumnsResponse> AlarmManagerColumns_GetAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<PutAlarmManagerColumnsResponse> AlarmManagerColumns_PutAsync(AlarmManagerColumns body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmPrioritiesResponse> AlarmPriorities_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmPriorityResponse> AlarmPriorities_PostAsync(PostAlarmPriorityDto body, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmPriorityResponse> AlarmPriorities_PatchByIdAsync(Guid id, PatchAlarmPriorityBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmPriorityResponse> AlarmPriorities_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmResponse> Alarms_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmResponse> Alarms_PatchAsync(Guid id, PatchAlarmBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmResponse> Alarms_PostAsync(PostAlarmBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmsResponse> Alarms_GetAsync(string orderBy = null, string localId = null, string message = null, string assignedTo_displayName = null, string time = null, string lastUpdatedTime = null, string state_level = null, string state_name = null, string priority_level = null, string priority_name = null, string category_level = null, string category_name = null, string source_name = null, string source_id = null, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmResponse> Alarms_PatchAlarmsBulkAsync(BulkPatchAlarmBody body, string source_id = null, int? state_level = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmHistoryResponse> Alarms_GetHistoryAsync(Guid id, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmSnapshotsResponse> Alarms_GetSnapshotsAsync(Guid id, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmSnapshotResponse> Alarms_PostSnapshotAsync(Guid id, PostAlarmSnapshotBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmsSessionResponse> AlarmSessions_GetByIdAsync(Guid id, int? maxSize = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmSessionResponse> AlarmSessions_PostAsync(PostAlarmSessionBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteSessionResponse> AlarmSessions_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmSettingsResponse> AlarmSettings_GetAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmSettingsResponse> AlarmSettings_PatchAsync(PatchAlarmSettingsBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmMessagesResponse> AlarmsMessages_GetMessagesAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmSoundsResponse> AlarmSounds_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmSoundResponse> AlarmSounds_PostAsync(PostAlarmSoundDto body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmSoundResponse> AlarmSounds_GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmSoundResponse> AlarmSounds_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmStatesResponse> AlarmStates_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmStateResponse> AlarmStates_PostAsync(PostAlarmStateDto body, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmStateResponse> AlarmStates_PatchByIdAsync(Guid id, PatchAlarmStateBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmStateResponse> AlarmStates_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmStatisticsResponse> AlarmsStatistics_GetAlarmStatisticsAsync(string include = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PostAlarmSuppressionResponse> AlarmSuppression_PostAsync(PostAlarmSuppressionBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<GetAlarmSuppressionResponse> AlarmSuppression_GetAsync(string source_id = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken));

	Task<PatchAlarmSuppressionResponse> AlarmSuppression_PatchAsync(Guid id, PatchAlarmSuppressionBody body, CancellationToken cancellationToken = default(CancellationToken));

	Task<DeleteAlarmSuppressionResponse> AlarmSuppression_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
