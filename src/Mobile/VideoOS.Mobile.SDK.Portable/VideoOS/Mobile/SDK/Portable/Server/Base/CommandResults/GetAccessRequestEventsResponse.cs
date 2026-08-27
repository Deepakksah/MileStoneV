using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessRequestEventsResponse : BaseCommandResponse
{
	public AccessControlEventsSearchResults AccessControlRequestsSearchResults { get; private set; }

	public GetAccessRequestEventsResponse(Command command)
		: base(command)
	{
		AccessControlRequestsSearchResults = new AccessControlEventsSearchResults();
		AccessControlRequestsSearchResults.AccessControlEvents = SerializableAccessControl.GetAccessControlEventsDetail(command.Items);
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("AccessControlSystemsCount"))
		{
			AccessControlRequestsSearchResults.AccessControlSystemsCount = Convert.ToInt32(dictionary["AccessControlSystemsCount"]);
		}
	}

	public GetAccessRequestEventsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
