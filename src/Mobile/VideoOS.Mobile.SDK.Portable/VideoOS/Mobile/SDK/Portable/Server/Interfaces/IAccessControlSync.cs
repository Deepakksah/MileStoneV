using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAccessControlSync
{
	GetAccessControlEventsResponse GetAccessControlEvents(GetAccessControlEventsParams getAccessControlEventsParams, TimeSpan timeout);

	GetAccessRequestEventsResponse GetAccessRequestEvents(TimeSpan timeout);

	GetAccessControlDoorsResponse GetAccessControlDoor(Guid doorId, TimeSpan timeout);

	GetAccessControlDoorsResponse GetAccessControlDoors(TimeSpan timeout);

	GetAccessControlDoorsResponse GetAccessControlDoors(GetAccessControlDoorsParams getAccessControlDoorsParams, TimeSpan timeout);

	GetAccessControlDoorStatesResponse GetAccessControlDoorStates(Guid accessControlSystemId, Guid doorId, TimeSpan timeout);

	GetAccessRequestEventTypesResponse GetAccessRequestEventTypes(List<Guid> accessControlSystemIds, TimeSpan timeout);

	GetAccessControlSystemsResponse GetAccessControlSystems(TimeSpan timeout);

	GetAccessControlDoorCategoriesResponse GetAccessControlDoorCategories(TimeSpan timeout);

	GetAccessControlDoorTypesResponse GetAccessControlDoorTypes(TimeSpan timeout);

	GetAccessControlEventCategoriesResponse GetAccessControlEventCategories(TimeSpan timeout);

	BaseCommandResponse ExecuteAccessControlCommand(ExecuteAccessControlCommandParams executeAccessControlCommandParams, TimeSpan timeout);
}
