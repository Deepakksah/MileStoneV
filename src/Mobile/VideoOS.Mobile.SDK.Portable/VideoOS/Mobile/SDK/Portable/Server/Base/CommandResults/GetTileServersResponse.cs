using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetTileServersResponse : BaseCommandResponse
{
	public IEnumerable<Uri> ServerAddresses { get; }

	public GetTileServersResponse(Command command)
		: base(command)
	{
		ServerAddresses = (from prop in command.Items.EmptyIfNull().SelectMany((SerializableItem item) => item.Properties)
			where prop.Key == "Address"
			select new Uri(prop.Value)).ToArray();
	}

	public GetTileServersResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
