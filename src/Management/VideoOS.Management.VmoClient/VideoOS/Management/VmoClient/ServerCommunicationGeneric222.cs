using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class ServerCommunicationGeneric222 : ServerCommunicationGeneric221
{
	public ServerCommunicationGeneric222(IVmoClient vmoClient)
		: base(vmoClient)
	{
	}

	public override VMOWebserviceObject[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string optFromBookmarkId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryBookmarksSearch2(centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, optionalSearchHeader, optionalSearchDescription, optionalSearchReference, optionalSearchUser, optionalHeaderFilter, optionalDescriptionFilter, optFromBookmarkId);
	}

	public override VMOWebserviceObject[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryMarkedDataSearch2(deviceIds, searchText, optionalSearchHeader, optionalSearchDescription, optionalSearchReference, optionalSearchUser, optionalHeaderFilter, optionalDescriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}
}
