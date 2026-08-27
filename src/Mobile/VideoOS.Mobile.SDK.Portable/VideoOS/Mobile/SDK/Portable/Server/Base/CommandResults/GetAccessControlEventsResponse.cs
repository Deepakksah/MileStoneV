using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlEventsResponse : BaseCommandResponse
{
	public AccessControlEventsSearchResults AccessControlEventsSearchResults { get; private set; }

	public GetAccessControlEventsResponse(Command command)
		: base(command)
	{
		AccessControlEventsSearchResults = new AccessControlEventsSearchResults();
		AccessControlEventsSearchResults.AccessControlEvents = SerializableAccessControl.GetAccessControlEventsDetail(command.Items);
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("AccessControlEventsCount"))
		{
			AccessControlEventsSearchResults.AccessControlEventsCount = Convert.ToInt32(dictionary["AccessControlEventsCount"]);
		}
		if (dictionary.ContainsKey("AccessControlSystemsCount"))
		{
			AccessControlEventsSearchResults.AccessControlSystemsCount = Convert.ToInt32(dictionary["AccessControlSystemsCount"]);
		}
	}

	public GetAccessControlEventsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
