using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlDoorsResponse : BaseCommandResponse
{
	public AccessControlDoorsSearchResults AccessControlDoorsSearchResults { get; private set; }

	public GetAccessControlDoorsResponse(Command command)
		: base(command)
	{
		AccessControlDoorsSearchResults = new AccessControlDoorsSearchResults();
		AccessControlDoorsSearchResults.AccessControlDoors = SerializableAccessControl.GetAccessControlDoors(command.Items);
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("AccessControlSystemsCount"))
		{
			AccessControlDoorsSearchResults.AccessControlSystemsCount = Convert.ToInt32(dictionary["AccessControlSystemsCount"]);
		}
	}

	public GetAccessControlDoorsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
