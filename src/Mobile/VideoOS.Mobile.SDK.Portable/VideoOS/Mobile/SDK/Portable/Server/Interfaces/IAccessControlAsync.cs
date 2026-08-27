using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAccessControlAsync
{
	IAsyncResult GetAccessControlEvents(GetAccessControlEventsParams getAccessControlEventsParams, OnResponse<GetAccessControlEventsResponse> onSuccess = null, OnResponse<GetAccessControlEventsResponse> onFail = null);

	IAsyncResult GetAccessRequestEvents(OnResponse<GetAccessRequestEventsResponse> onSuccess = null, OnResponse<GetAccessRequestEventsResponse> onFail = null);

	IAsyncResult GetAccessControlDoor(Guid doorId, OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null);

	IAsyncResult GetAccessControlDoors(OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null);

	IAsyncResult GetAccessControlDoors(GetAccessControlDoorsParams getAccessControlDoorsParams, OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null);

	IAsyncResult GetAccessControlDoorStates(Guid accessControlSystemId, Guid doorId, OnResponse<GetAccessControlDoorStatesResponse> onSuccess = null, OnResponse<GetAccessControlDoorStatesResponse> onFail = null);

	IAsyncResult GetAccessRequestEventTypes(List<Guid> accessControlSystemIds, OnResponse<GetAccessRequestEventTypesResponse> onSuccess = null, OnResponse<GetAccessRequestEventTypesResponse> onFail = null);

	IAsyncResult GetAccessControlSystems(OnResponse<GetAccessControlSystemsResponse> onSuccess = null, OnResponse<GetAccessControlSystemsResponse> onFail = null);

	IAsyncResult GetAccessControlDoorCategories(OnResponse<GetAccessControlDoorCategoriesResponse> onSuccess = null, OnResponse<GetAccessControlDoorCategoriesResponse> onFail = null);

	IAsyncResult GetAccessControlDoorTypes(OnResponse<GetAccessControlDoorTypesResponse> onSuccess = null, OnResponse<GetAccessControlDoorTypesResponse> onFail = null);

	IAsyncResult GetAccessControlEventCategories(OnResponse<GetAccessControlEventCategoriesResponse> onSuccess = null, OnResponse<GetAccessControlEventCategoriesResponse> onFail = null);

	IAsyncResult ExecuteAccessControlCommand(ExecuteAccessControlCommandParams executeAccessControlCommandParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
